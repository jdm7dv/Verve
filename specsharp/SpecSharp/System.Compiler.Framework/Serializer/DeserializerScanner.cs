
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Contracts;
using Microsoft.Boogie;


namespace Omni {

//-----------------------------------------------------------------------------------
// Buffer
//-----------------------------------------------------------------------------------
public class Buffer {
	// This Buffer supports the following cases:
	// 1) seekable stream (file)
	//    a) whole stream in buffer
	//    b) part of stream in buffer
	// 2) non seekable stream (network, console)

	public const int EOF = 65535 + 1; // char.MaxValue + 1;
	const int MIN_BUFFER_LENGTH = 1024; // 1KB
	const int MAX_BUFFER_LENGTH = MIN_BUFFER_LENGTH * 64; // 64KB
	byte[]/*!*/ buf;         // input buffer
	int bufStart;       // position of first byte in buffer relative to input stream
	int bufLen;         // length of buffer
	int fileLen;        // length of input stream (may change if the stream is no file)
	int bufPos;         // current position in buffer
	Stream/*!*/ stream;      // input stream (seekable)
	bool isUserStream;  // was the stream opened by the user?

	[ContractInvariantMethod]
	void ObjectInvariant(){
		Contract.Invariant(buf != null);
		Contract.Invariant(stream != null);
	}

//  [NotDelayed]
	public Buffer (Stream/*!*/ s, bool isUserStream) : base() {
	  Contract.Requires(s != null);
		stream = s; this.isUserStream = isUserStream;

		int fl, bl;
		if (s.CanSeek) {
			fl = (int) s.Length;
			bl = fl < MAX_BUFFER_LENGTH ? fl : MAX_BUFFER_LENGTH; // Math.Min(fileLen, MAX_BUFFER_LENGTH);
			bufStart = Int32.MaxValue; // nothing in the buffer so far
		} else {
			fl = bl = bufStart = 0;
		}

		buf = new byte[(bl>0) ? bl : MIN_BUFFER_LENGTH];
		fileLen = fl;  bufLen = bl;

		if (fileLen > 0) Pos = 0; // setup buffer to position 0 (start)
		else bufPos = 0; // index 0 is already after the file, thus Pos = 0 is invalid
		if (bufLen == fileLen && s.CanSeek) Close();
	}

	protected Buffer(Buffer/*!*/ b) { // called in UTF8Buffer constructor
	  Contract.Requires(b != null);
		buf = b.buf;
		bufStart = b.bufStart;
		bufLen = b.bufLen;
		fileLen = b.fileLen;
		bufPos = b.bufPos;
		stream = b.stream;
		// keep destructor from closing the stream
		//b.stream = null;
		isUserStream = b.isUserStream;
		// keep destructor from closing the stream
		b.isUserStream = true;
	}

	~Buffer() { Close(); }

	protected void Close() {
		if (!isUserStream && stream != null) {
			stream.Close();
			//stream = null;
		}
	}

	public virtual int Read () {
		if (bufPos < bufLen) {
			return buf[bufPos++];
		} else if (Pos < fileLen) {
			Pos = Pos; // shift buffer start to Pos
			return buf[bufPos++];
		} else if (stream != null && !stream.CanSeek && ReadNextStreamChunk() > 0) {
			return buf[bufPos++];
		} else {
			return EOF;
		}
	}

	public int Peek () {
		int curPos = Pos;
		int ch = Read();
		Pos = curPos;
		return ch;
	}

	public string/*!*/ GetString (int beg, int end) {
	  Contract.Ensures(Contract.Result<string>() != null);
		int len = 0;
		char[] buf = new char[end - beg];
		int oldPos = Pos;
		Pos = beg;
		while (Pos < end) buf[len++] = (char) Read();
		Pos = oldPos;
		return new String(buf, 0, len);
	}

	public int Pos {
		get { return bufPos + bufStart; }
		set {
			if (value >= fileLen && stream != null && !stream.CanSeek) {
				// Wanted position is after buffer and the stream
				// is not seek-able e.g. network or console,
				// thus we have to read the stream manually till
				// the wanted position is in sight.
				while (value >= fileLen && ReadNextStreamChunk() > 0);
			}

			if (value < 0 || value > fileLen) {
				throw new FatalError("buffer out of bounds access, position: " + value);
			}

			if (value >= bufStart && value < bufStart + bufLen) { // already in buffer
				bufPos = value - bufStart;
			} else if (stream != null) { // must be swapped in
				stream.Seek(value, SeekOrigin.Begin);
				bufLen = stream.Read(buf, 0, buf.Length);
				bufStart = value; bufPos = 0;
			} else {
				// set the position to the end of the file, Pos will return fileLen.
				bufPos = fileLen - bufStart;
			}
		}
	}

	// Read the next chunk of bytes from the stream, increases the buffer
	// if needed and updates the fields fileLen and bufLen.
	// Returns the number of bytes read.
	private int ReadNextStreamChunk() {
		int free = buf.Length - bufLen;
		if (free == 0) {
			// in the case of a growing input stream
			// we can neither seek in the stream, nor can we
			// foresee the maximum length, thus we must adapt
			// the buffer size on demand.
			byte[] newBuf = new byte[bufLen * 2];
			Array.Copy(buf, newBuf, bufLen);
			buf = newBuf;
			free = bufLen;
		}
		int read = stream.Read(buf, bufLen, free);
		if (read > 0) {
			fileLen = bufLen = (bufLen + read);
			return read;
		}
		// end of stream reached
		return 0;
	}
}

//-----------------------------------------------------------------------------------
// UTF8Buffer
//-----------------------------------------------------------------------------------
public class UTF8Buffer: Buffer {
	public UTF8Buffer(Buffer/*!*/ b): base(b) {Contract.Requires(b != null);}

	public override int Read() {
		int ch;
		do {
			ch = base.Read();
			// until we find a utf8 start (0xxxxxxx or 11xxxxxx)
		} while ((ch >= 128) && ((ch & 0xC0) != 0xC0) && (ch != EOF));
		if (ch < 128 || ch == EOF) {
			// nothing to do, first 127 chars are the same in ascii and utf8
			// 0xxxxxxx or end of file character
		} else if ((ch & 0xF0) == 0xF0) {
			// 11110xxx 10xxxxxx 10xxxxxx 10xxxxxx
			int c1 = ch & 0x07; ch = base.Read();
			int c2 = ch & 0x3F; ch = base.Read();
			int c3 = ch & 0x3F; ch = base.Read();
			int c4 = ch & 0x3F;
			ch = (((((c1 << 6) | c2) << 6) | c3) << 6) | c4;
		} else if ((ch & 0xE0) == 0xE0) {
			// 1110xxxx 10xxxxxx 10xxxxxx
			int c1 = ch & 0x0F; ch = base.Read();
			int c2 = ch & 0x3F; ch = base.Read();
			int c3 = ch & 0x3F;
			ch = (((c1 << 6) | c2) << 6) | c3;
		} else if ((ch & 0xC0) == 0xC0) {
			// 110xxxxx 10xxxxxx
			int c1 = ch & 0x1F; ch = base.Read();
			int c2 = ch & 0x3F;
			ch = (c1 << 6) | c2;
		}
		return ch;
	}
}

//-----------------------------------------------------------------------------------
// Scanner
//-----------------------------------------------------------------------------------
public class Scanner {
	const char EOL = '\n';
	const int eofSym = 0; /* pdt */
	const int maxT = 108;
	const int noSym = 108;


	[ContractInvariantMethod]
	void objectInvariant(){
		Contract.Invariant(buffer!=null);
		Contract.Invariant(t != null);
		Contract.Invariant(start != null);
		Contract.Invariant(tokens != null);
		Contract.Invariant(pt != null);
		Contract.Invariant(tval != null);
		Contract.Invariant(Filename != null);
		Contract.Invariant(errorHandler != null);
	}

	public Buffer/*!*/ buffer; // scanner buffer

	Token/*!*/ t;          // current token
	int ch;           // current input character
	int pos;          // byte position of current character
	int charPos;
	int col;          // column number of current character
	int line;         // line number of current character
	int oldEols;      // EOLs that appeared in a comment;
	static readonly Hashtable/*!*/ start; // maps first token character to start state

	Token/*!*/ tokens;     // list of tokens already peeked (first token is a dummy)
	Token/*!*/ pt;         // current peek token

	char[]/*!*/ tval = new char[128]; // text of current token
	int tlen;         // length of current token

	private string/*!*/ Filename;
	private Errors/*!*/ errorHandler;

	static Scanner() {
		start = new Hashtable(128);
		for (int i = 65; i <= 90; ++i) start[i] = 1;
		for (int i = 95; i <= 95; ++i) start[i] = 1;
		for (int i = 97; i <= 122; ++i) start[i] = 1;
		for (int i = 50; i <= 57; ++i) start[i] = 2;
		for (int i = 34; i <= 34; ++i) start[i] = 3;
		for (int i = 39; i <= 39; ++i) start[i] = 10;
		for (int i = 92; i <= 92; ++i) start[i] = 9;
		start[40] = 192; 
		start[44] = 12; 
		start[41] = 13; 
		start[36] = 193; 
		start[38] = 194; 
		start[91] = 34; 
		start[93] = 35; 
		start[42] = 36; 
		start[60] = 195; 
		start[62] = 196; 
		start[46] = 197; 
		start[58] = 198; 
		start[43] = 47; 
		start[64] = 48; 
		start[123] = 199; 
		start[125] = 86; 
		start[37] = 200; 
		start[59] = 142; 
		start[124] = 201; 
		start[45] = 146; 
		start[61] = 202; 
		start[33] = 203; 
		start[126] = 154; 
		start[94] = 155; 
		start[47] = 156; 
		start[48] = 204; 
		start[49] = 205; 
		start[Buffer.EOF] = -1;

	}

//	[NotDelayed]
	public Scanner (string/*!*/ fileName, Errors/*!*/ errorHandler) : base() {
	  Contract.Requires(fileName != null);
	  Contract.Requires(errorHandler != null);
		this.errorHandler = errorHandler;
		pt = tokens = new Token();  // first token is a dummy
		t = new Token(); // dummy because t is a non-null field
		try {
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			buffer = new Buffer(stream, false);
			Filename = fileName;
			Init();
		} catch (IOException) {
			throw new FatalError("Cannot open file " + fileName);
		}
	}

//	[NotDelayed]
	public Scanner (Stream/*!*/ s, Errors/*!*/ errorHandler, string/*!*/ fileName) : base() {
	  Contract.Requires(s != null);
	  Contract.Requires(errorHandler != null);
	  Contract.Requires(fileName != null);
		pt = tokens = new Token();  // first token is a dummy
		t = new Token(); // dummy because t is a non-null field
		buffer = new Buffer(s, true);
		this.errorHandler = errorHandler;
		this.Filename = fileName;
		Init();
	}

	void Init() {
		pos = -1; line = 1; col = 0;
		oldEols = 0;
		NextCh();
		if (ch == 0xEF) { // check optional byte order mark for UTF-8
			NextCh(); int ch1 = ch;
			NextCh(); int ch2 = ch;
			if (ch1 != 0xBB || ch2 != 0xBF) {
				throw new FatalError(String.Format("illegal byte order mark: EF {0,2:X} {1,2:X}", ch1, ch2));
			}
			buffer = new UTF8Buffer(buffer); col = 0;
			NextCh();
		}
		pt = tokens = new Token();  // first token is a dummy
	}

	string/*!*/ ReadToEOL(){
	Contract.Ensures(Contract.Result<string>() != null);
	  int p = buffer.Pos;
	  int ch = buffer.Read();
	  // replace isolated '\r' by '\n' in order to make
	  // eol handling uniform across Windows, Unix and Mac
	  if (ch == '\r' && buffer.Peek() != '\n') ch = EOL;
	  while (ch != EOL && ch != Buffer.EOF){
		ch = buffer.Read();
		// replace isolated '\r' by '\n' in order to make
		// eol handling uniform across Windows, Unix and Mac
		if (ch == '\r' && buffer.Peek() != '\n') ch = EOL;
	  }
	  string/*!*/ s = buffer.GetString(p, buffer.Pos);
	  Contract.Assert(s!=null);
	  return s;
	}

	void NextCh() {
		if (oldEols > 0) { ch = EOL; oldEols--; }
		else {
//			pos = buffer.Pos;
//			ch = buffer.Read(); col++;
//			// replace isolated '\r' by '\n' in order to make
//			// eol handling uniform across Windows, Unix and Mac
//			if (ch == '\r' && buffer.Peek() != '\n') ch = EOL;
//			if (ch == EOL) { line++; col = 0; }

			while (true) {
				pos = buffer.Pos;
				ch = buffer.Read(); col++;
				// replace isolated '\r' by '\n' in order to make
				// eol handling uniform across Windows, Unix and Mac
				if (ch == '\r' && buffer.Peek() != '\n') ch = EOL;
				if (ch == EOL) {
					line++; col = 0;
				} else if (ch == '#' && col == 1) {
				  int prLine = line;
				  int prColumn = 0;

				  string/*!*/ hashLine = ReadToEOL();
				  Contract.Assert(hashLine!=null);
				  col = 0;
				  line++;

				  hashLine = hashLine.TrimEnd(null);
				  if (hashLine.StartsWith("line ") || hashLine == "line") {
					// parse #line pragma:  #line num [filename]
					string h = hashLine.Substring(4).TrimStart(null);
					int x = h.IndexOf(' ');
					if (x == -1) {
					  x = h.Length;  // this will be convenient below when we look for a filename
					}
					try {
					  int li = int.Parse(h.Substring(0, x));

					  h = h.Substring(x).Trim();

					  // act on #line
					  line = li;
					  if (h.Length != 0) {
						// a filename was specified
						Filename = h;
					  }
					  continue;  // successfully parsed and acted on the #line pragma

					} catch (FormatException) {
					  // just fall down through to produce an error message
					}
					this.errorHandler.SemErr(Filename, prLine, prColumn, "Malformed (#line num [filename]) pragma: #" + hashLine);
					continue;
				  }

				  this.errorHandler.SemErr(Filename, prLine, prColumn, "Unrecognized pragma: #" + hashLine);
				  continue;
				}
				return;
			  }


		}

	}

	void AddCh() {
		if (tlen >= tval.Length) {
			char[] newBuf = new char[2 * tval.Length];
			Array.Copy(tval, 0, newBuf, 0, tval.Length);
			tval = newBuf;
		}
		if (ch != Buffer.EOF) {
			tval[tlen++] = (char) ch;
			NextCh();
		}
	}



	bool Comment0() {
		int level = 1, pos0 = pos, line0 = line, col0 = col, charPos0 = charPos;
		NextCh();
		if (ch == '/') {
			NextCh();
			for(;;) {
				if (ch == 10) {
					level--;
					if (level == 0) { oldEols = line - line0; NextCh(); return true; }
					NextCh();
				} else if (ch == Buffer.EOF) return false;
				else NextCh();
			}
		} else {
			buffer.Pos = pos0; NextCh(); line = line0; col = col0; charPos = charPos0;
		}
		return false;
	}

	bool Comment1() {
		int level = 1, pos0 = pos, line0 = line, col0 = col, charPos0 = charPos;
		NextCh();
		if (ch == '*') {
			NextCh();
			for(;;) {
				if (ch == '*') {
					NextCh();
					if (ch == '/') {
						level--;
						if (level == 0) { oldEols = line - line0; NextCh(); return true; }
						NextCh();
					}
				} else if (ch == '/') {
					NextCh();
					if (ch == '*') {
						level++; NextCh();
					}
				} else if (ch == Buffer.EOF) return false;
				else NextCh();
			}
		} else {
			buffer.Pos = pos0; NextCh(); line = line0; col = col0; charPos = charPos0;
		}
		return false;
	}


	void CheckLiteral() {
		switch (t.val) {
			case "object": t.kind = 6; break;
			case "string": t.kind = 7; break;
			case "char": t.kind = 8; break;
			case "void": t.kind = 9; break;
			case "bool": t.kind = 10; break;
			case "i8": t.kind = 11; break;
			case "i16": t.kind = 12; break;
			case "i32": t.kind = 13; break;
			case "i64": t.kind = 14; break;
			case "u8": t.kind = 15; break;
			case "u16": t.kind = 16; break;
			case "u32": t.kind = 17; break;
			case "u64": t.kind = 18; break;
			case "single": t.kind = 19; break;
			case "double": t.kind = 20; break;
			case "optional": t.kind = 21; break;
			case "old": t.kind = 38; break;
			case "True": t.kind = 71; break;
			case "False": t.kind = 72; break;
			case "null": t.kind = 73; break;
			case "this": t.kind = 74; break;
			case "new": t.kind = 75; break;
			default: break;
		}
	}

	Token/*!*/ NextToken() {
	  Contract.Ensures(Contract.Result<Token>() != null);
		while (ch == ' ' ||
			ch >= 9 && ch <= 10 || ch == 13
		) NextCh();
		if (ch == '/' && Comment0() ||ch == '/' && Comment1()) return NextToken();
		int recKind = noSym;
		int recEnd = pos;
		t = new Token();
		t.pos = pos; t.col = col; t.line = line;
		t.filename = this.Filename;
		int state;
		if (start.ContainsKey(ch)) {
			Contract.Assert(start[ch] != null);
			state = (int) start[ch];
		}
		else { state = 0; }
		tlen = 0; AddCh();

		switch (state) {
			case -1: { t.kind = eofSym; break; } // NextCh already done
			case 0: {
				if (recKind != noSym) {
					tlen = recEnd - t.pos;
					SetScannerBehindT();
				}
				t.kind = recKind; break;
			} // NextCh already done
			case 1:
				recEnd = pos; recKind = 1;
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch >= '_' && ch <= 'z') {AddCh(); goto case 1;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 2:
				recEnd = pos; recKind = 2;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 2;}
				else {t.kind = 2; break;}
			case 3:
				if (ch == '"') {AddCh(); goto case 4;}
				else if (ch >= 9 && ch <= 10 || ch == 13 || ch >= ' ' && ch <= '!' || ch >= '#' && ch <= '[' || ch >= ']' && ch <= '~') {AddCh(); goto case 3;}
				else {goto case 0;}
			case 4:
				{t.kind = 3; break;}
			case 5:
				if (ch == 39) {AddCh(); goto case 8;}
				else {goto case 0;}
			case 6:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 7;}
				else {goto case 0;}
			case 7:
				if (ch == 39) {AddCh(); goto case 8;}
				else if (ch >= '0' && ch <= '9') {AddCh(); goto case 7;}
				else {goto case 0;}
			case 8:
				{t.kind = 4; break;}
			case 9:
				{t.kind = 5; break;}
			case 10:
				if (ch <= '[' || ch >= ']' && ch <= 16383) {AddCh(); goto case 5;}
				else if (ch == 92) {AddCh(); goto case 11;}
				else {goto case 0;}
			case 11:
				if (ch == 39) {AddCh(); goto case 8;}
				else if (ch == 'u') {AddCh(); goto case 6;}
				else {goto case 0;}
			case 12:
				{t.kind = 23; break;}
			case 13:
				{t.kind = 24; break;}
			case 14:
				if (ch == 'a') {AddCh(); goto case 15;}
				else {goto case 0;}
			case 15:
				if (ch == 'r') {AddCh(); goto case 16;}
				else {goto case 0;}
			case 16:
				if (ch == 'a') {AddCh(); goto case 17;}
				else {goto case 0;}
			case 17:
				if (ch == 'm') {AddCh(); goto case 18;}
				else {goto case 0;}
			case 18:
				{t.kind = 25; break;}
			case 19:
				if (ch == 'e') {AddCh(); goto case 20;}
				else {goto case 0;}
			case 20:
				if (ch == 't') {AddCh(); goto case 21;}
				else {goto case 0;}
			case 21:
				if (ch == 'h') {AddCh(); goto case 22;}
				else {goto case 0;}
			case 22:
				if (ch == 'o') {AddCh(); goto case 23;}
				else {goto case 0;}
			case 23:
				if (ch == 'd') {AddCh(); goto case 24;}
				else {goto case 0;}
			case 24:
				if (ch == 'T') {AddCh(); goto case 25;}
				else {goto case 0;}
			case 25:
				if (ch == 'y') {AddCh(); goto case 26;}
				else {goto case 0;}
			case 26:
				if (ch == 'p') {AddCh(); goto case 27;}
				else {goto case 0;}
			case 27:
				if (ch == 'e') {AddCh(); goto case 28;}
				else {goto case 0;}
			case 28:
				if (ch == 'P') {AddCh(); goto case 29;}
				else {goto case 0;}
			case 29:
				if (ch == 'a') {AddCh(); goto case 30;}
				else {goto case 0;}
			case 30:
				if (ch == 'r') {AddCh(); goto case 31;}
				else {goto case 0;}
			case 31:
				if (ch == 'a') {AddCh(); goto case 32;}
				else {goto case 0;}
			case 32:
				if (ch == 'm') {AddCh(); goto case 33;}
				else {goto case 0;}
			case 33:
				{t.kind = 26; break;}
			case 34:
				{t.kind = 28; break;}
			case 35:
				{t.kind = 29; break;}
			case 36:
				{t.kind = 30; break;}
			case 37:
				if (ch == 'e') {AddCh(); goto case 38;}
				else {goto case 0;}
			case 38:
				if (ch == 'i') {AddCh(); goto case 39;}
				else {goto case 0;}
			case 39:
				if (ch == 'r') {AddCh(); goto case 40;}
				else {goto case 0;}
			case 40:
				if (ch == 'd') {AddCh(); goto case 41;}
				else {goto case 0;}
			case 41:
				if (ch == 'I') {AddCh(); goto case 42;}
				else {goto case 0;}
			case 42:
				if (ch == 'd') {AddCh(); goto case 43;}
				else {goto case 0;}
			case 43:
				if (ch == 'e') {AddCh(); goto case 44;}
				else {goto case 0;}
			case 44:
				if (ch == 'n') {AddCh(); goto case 45;}
				else {goto case 0;}
			case 45:
				if (ch == 't') {AddCh(); goto case 46;}
				else {goto case 0;}
			case 46:
				{t.kind = 35; break;}
			case 47:
				{t.kind = 36; break;}
			case 48:
				{t.kind = 37; break;}
			case 49:
				if (ch == 'e') {AddCh(); goto case 50;}
				else {goto case 0;}
			case 50:
				if (ch == 'r') {AddCh(); goto case 51;}
				else {goto case 0;}
			case 51:
				if (ch == 'c') {AddCh(); goto case 52;}
				else {goto case 0;}
			case 52:
				if (ch == 'e') {AddCh(); goto case 53;}
				else {goto case 0;}
			case 53:
				{t.kind = 39; break;}
			case 54:
				if (ch == 'e') {AddCh(); goto case 55;}
				else {goto case 0;}
			case 55:
				if (ch == 's') {AddCh(); goto case 56;}
				else {goto case 0;}
			case 56:
				if (ch == 't') {AddCh(); goto case 57;}
				else {goto case 0;}
			case 57:
				{t.kind = 40; break;}
			case 58:
				if (ch == 's') {AddCh(); goto case 59;}
				else {goto case 0;}
			case 59:
				if (ch == 't') {AddCh(); goto case 60;}
				else {goto case 0;}
			case 60:
				if (ch == 'c') {AddCh(); goto case 61;}
				else {goto case 0;}
			case 61:
				if (ch == 'l') {AddCh(); goto case 62;}
				else {goto case 0;}
			case 62:
				if (ch == 'a') {AddCh(); goto case 63;}
				else {goto case 0;}
			case 63:
				if (ch == 's') {AddCh(); goto case 64;}
				else {goto case 0;}
			case 64:
				if (ch == 's') {AddCh(); goto case 65;}
				else {goto case 0;}
			case 65:
				{t.kind = 41; break;}
			case 66:
				if (ch == 'f') {AddCh(); goto case 67;}
				else {goto case 0;}
			case 67:
				{t.kind = 42; break;}
			case 68:
				if (ch == 't') {AddCh(); goto case 69;}
				else {goto case 0;}
			case 69:
				if (ch == 'e') {AddCh(); goto case 70;}
				else {goto case 0;}
			case 70:
				{t.kind = 43; break;}
			case 71:
				if (ch == 'x') {AddCh(); goto case 72;}
				else {goto case 0;}
			case 72:
				{t.kind = 44; break;}
			case 73:
				if (ch == 'e') {AddCh(); goto case 74;}
				else {goto case 0;}
			case 74:
				if (ch == 'f') {AddCh(); goto case 75;}
				else {goto case 0;}
			case 75:
				{t.kind = 46; break;}
			case 76:
				{t.kind = 47; break;}
			case 77:
				if (ch == 'd') {AddCh(); goto case 78;}
				else {goto case 0;}
			case 78:
				if (ch == 'd') {AddCh(); goto case 79;}
				else {goto case 0;}
			case 79:
				if (ch == 'r') {AddCh(); goto case 80;}
				else {goto case 0;}
			case 80:
				if (ch == 'e') {AddCh(); goto case 81;}
				else {goto case 0;}
			case 81:
				if (ch == 's') {AddCh(); goto case 82;}
				else {goto case 0;}
			case 82:
				if (ch == 's') {AddCh(); goto case 83;}
				else {goto case 0;}
			case 83:
				if (ch == 'O') {AddCh(); goto case 84;}
				else {goto case 0;}
			case 84:
				if (ch == 'f') {AddCh(); goto case 85;}
				else {goto case 0;}
			case 85:
				{t.kind = 48; break;}
			case 86:
				{t.kind = 50; break;}
			case 87:
				if (ch == 'e') {AddCh(); goto case 88;}
				else {goto case 0;}
			case 88:
				if (ch == 'r') {AddCh(); goto case 89;}
				else {goto case 0;}
			case 89:
				if (ch == 'e') {AddCh(); goto case 90;}
				else {goto case 0;}
			case 90:
				if (ch == 'f') {AddCh(); goto case 91;}
				else {goto case 0;}
			case 91:
				{t.kind = 51; break;}
			case 92:
				if (ch == 's') {AddCh(); goto case 93;}
				else {goto case 0;}
			case 93:
				if (ch == 'i') {AddCh(); goto case 94;}
				else {goto case 0;}
			case 94:
				if (ch == 'n') {AddCh(); goto case 95;}
				else {goto case 0;}
			case 95:
				if (ch == 's') {AddCh(); goto case 96;}
				else {goto case 0;}
			case 96:
				if (ch == 't') {AddCh(); goto case 97;}
				else {goto case 0;}
			case 97:
				{t.kind = 52; break;}
			case 98:
				if (ch == 's') {AddCh(); goto case 99;}
				else {goto case 0;}
			case 99:
				if (ch == 'i') {AddCh(); goto case 100;}
				else {goto case 0;}
			case 100:
				if (ch == 'n') {AddCh(); goto case 101;}
				else {goto case 0;}
			case 101:
				if (ch == 's') {AddCh(); goto case 102;}
				else {goto case 0;}
			case 102:
				if (ch == 't') {AddCh(); goto case 103;}
				else {goto case 0;}
			case 103:
				{t.kind = 53; break;}
			case 104:
				if (ch == 'o') {AddCh(); goto case 105;}
				else {goto case 0;}
			case 105:
				if (ch == 'c') {AddCh(); goto case 106;}
				else {goto case 0;}
			case 106:
				if (ch == 'k') {AddCh(); goto case 107;}
				else {goto case 0;}
			case 107:
				if (ch == 'V') {AddCh(); goto case 108;}
				else {goto case 0;}
			case 108:
				if (ch == 'a') {AddCh(); goto case 109;}
				else {goto case 0;}
			case 109:
				if (ch == 'r') {AddCh(); goto case 110;}
				else {goto case 0;}
			case 110:
				{t.kind = 54; break;}
			case 111:
				if (ch == 't') {AddCh(); goto case 112;}
				else {goto case 0;}
			case 112:
				if (ch == 'o') {AddCh(); goto case 113;}
				else {goto case 0;}
			case 113:
				if (ch == 'r') {AddCh(); goto case 114;}
				else {goto case 0;}
			case 114:
				{t.kind = 56; break;}
			case 115:
				if (ch == 'o') {AddCh(); goto case 116;}
				else {goto case 0;}
			case 116:
				if (ch == 'r') {AddCh(); goto case 117;}
				else {goto case 0;}
			case 117:
				if (ch == 'a') {AddCh(); goto case 118;}
				else {goto case 0;}
			case 118:
				if (ch == 'l') {AddCh(); goto case 119;}
				else {goto case 0;}
			case 119:
				if (ch == 'l') {AddCh(); goto case 120;}
				else {goto case 0;}
			case 120:
				{t.kind = 57; break;}
			case 121:
				{t.kind = 59; break;}
			case 122:
				if (ch == 'o') {AddCh(); goto case 123;}
				else {goto case 0;}
			case 123:
				if (ch == 'u') {AddCh(); goto case 124;}
				else {goto case 0;}
			case 124:
				if (ch == 'n') {AddCh(); goto case 125;}
				else {goto case 0;}
			case 125:
				if (ch == 't') {AddCh(); goto case 126;}
				else {goto case 0;}
			case 126:
				{t.kind = 60; break;}
			case 127:
				if (ch == 'x') {AddCh(); goto case 128;}
				else {goto case 0;}
			case 128:
				{t.kind = 61; break;}
			case 129:
				if (ch == 'n') {AddCh(); goto case 130;}
				else {goto case 0;}
			case 130:
				{t.kind = 62; break;}
			case 131:
				if (ch == 'r') {AddCh(); goto case 132;}
				else {goto case 0;}
			case 132:
				if (ch == 'o') {AddCh(); goto case 133;}
				else {goto case 0;}
			case 133:
				if (ch == 'd') {AddCh(); goto case 134;}
				else {goto case 0;}
			case 134:
				if (ch == 'u') {AddCh(); goto case 135;}
				else {goto case 0;}
			case 135:
				if (ch == 'c') {AddCh(); goto case 136;}
				else {goto case 0;}
			case 136:
				if (ch == 't') {AddCh(); goto case 137;}
				else {goto case 0;}
			case 137:
				{t.kind = 63; break;}
			case 138:
				if (ch == 'u') {AddCh(); goto case 139;}
				else {goto case 0;}
			case 139:
				if (ch == 'm') {AddCh(); goto case 140;}
				else {goto case 0;}
			case 140:
				{t.kind = 64; break;}
			case 141:
				{t.kind = 65; break;}
			case 142:
				{t.kind = 66; break;}
			case 143:
				{t.kind = 67; break;}
			case 144:
				{t.kind = 68; break;}
			case 145:
				{t.kind = 69; break;}
			case 146:
				{t.kind = 70; break;}
			case 147:
				{t.kind = 77; break;}
			case 148:
				{t.kind = 79; break;}
			case 149:
				{t.kind = 80; break;}
			case 150:
				{t.kind = 81; break;}
			case 151:
				{t.kind = 83; break;}
			case 152:
				if (ch == '>') {AddCh(); goto case 153;}
				else {goto case 0;}
			case 153:
				{t.kind = 84; break;}
			case 154:
				{t.kind = 86; break;}
			case 155:
				{t.kind = 87; break;}
			case 156:
				{t.kind = 88; break;}
			case 157:
				{t.kind = 90; break;}
			case 158:
				{t.kind = 91; break;}
			case 159:
				{t.kind = 92; break;}
			case 160:
				if (ch == '>') {AddCh(); goto case 161;}
				else {goto case 0;}
			case 161:
				{t.kind = 93; break;}
			case 162:
				if (ch == '>') {AddCh(); goto case 163;}
				else {goto case 0;}
			case 163:
				{t.kind = 94; break;}
			case 164:
				if (ch == 'e') {AddCh(); goto case 165;}
				else {goto case 0;}
			case 165:
				if (ch == 'f') {AddCh(); goto case 166;}
				else {goto case 0;}
			case 166:
				if (ch == 'a') {AddCh(); goto case 167;}
				else {goto case 0;}
			case 167:
				if (ch == 'u') {AddCh(); goto case 168;}
				else {goto case 0;}
			case 168:
				if (ch == 'l') {AddCh(); goto case 169;}
				else {goto case 0;}
			case 169:
				if (ch == 't') {AddCh(); goto case 170;}
				else {goto case 0;}
			case 170:
				if (ch == 'V') {AddCh(); goto case 171;}
				else {goto case 0;}
			case 171:
				if (ch == 'a') {AddCh(); goto case 172;}
				else {goto case 0;}
			case 172:
				if (ch == 'l') {AddCh(); goto case 173;}
				else {goto case 0;}
			case 173:
				if (ch == 'u') {AddCh(); goto case 174;}
				else {goto case 0;}
			case 174:
				if (ch == 'e') {AddCh(); goto case 175;}
				else {goto case 0;}
			case 175:
				{t.kind = 95; break;}
			case 176:
				if (ch == 'n') {AddCh(); goto case 177;}
				else {goto case 0;}
			case 177:
				if (ch == 'y') {AddCh(); goto case 178;}
				else {goto case 0;}
			case 178:
				{t.kind = 96; break;}
			case 179:
				{t.kind = 97; break;}
			case 180:
				{t.kind = 98; break;}
			case 181:
				{t.kind = 99; break;}
			case 182:
				{t.kind = 100; break;}
			case 183:
				{t.kind = 102; break;}
			case 184:
				{t.kind = 103; break;}
			case 185:
				{t.kind = 104; break;}
			case 186:
				{t.kind = 105; break;}
			case 187:
				if (ch == 'd') {AddCh(); goto case 188;}
				else {goto case 0;}
			case 188:
				if (ch == 'l') {AddCh(); goto case 189;}
				else {goto case 0;}
			case 189:
				if (ch == 'e') {AddCh(); goto case 190;}
				else {goto case 0;}
			case 190:
				if (ch == 'n') {AddCh(); goto case 191;}
				else {goto case 0;}
			case 191:
				{t.kind = 107; break;}
			case 192:
				recEnd = pos; recKind = 22;
				if (ch == '|') {AddCh(); goto case 144;}
				else {t.kind = 22; break;}
			case 193:
				recEnd = pos; recKind = 55;
				if (ch == 't') {AddCh(); goto case 206;}
				else if (ch == 'm') {AddCh(); goto case 19;}
				else if (ch == 'w') {AddCh(); goto case 37;}
				else if (ch == 'c') {AddCh(); goto case 207;}
				else if (ch == 'i') {AddCh(); goto case 68;}
				else if (ch == 'b') {AddCh(); goto case 208;}
				else if (ch == 'u') {AddCh(); goto case 209;}
				else if (ch == 'r') {AddCh(); goto case 73;}
				else if (ch == 'A') {AddCh(); goto case 77;}
				else if (ch == 'D') {AddCh(); goto case 87;}
				else if (ch == 'I') {AddCh(); goto case 92;}
				else if (ch == '_') {AddCh(); goto case 210;}
				else if (ch == 'd') {AddCh(); goto case 164;}
				else if (ch == 'C') {AddCh(); goto case 211;}
				else if (ch == 'L') {AddCh(); goto case 187;}
				else {t.kind = 55; break;}
			case 194:
				recEnd = pos; recKind = 27;
				if (ch == '&') {AddCh(); goto case 150;}
				else {t.kind = 27; break;}
			case 195:
				recEnd = pos; recKind = 31;
				if (ch == '=') {AddCh(); goto case 212;}
				else if (ch == '<') {AddCh(); goto case 159;}
				else {t.kind = 31; break;}
			case 196:
				recEnd = pos; recKind = 32;
				if (ch == '=') {AddCh(); goto case 147;}
				else {t.kind = 32; break;}
			case 197:
				recEnd = pos; recKind = 33;
				if (ch == 'c') {AddCh(); goto case 111;}
				else {t.kind = 33; break;}
			case 198:
				recEnd = pos; recKind = 34;
				if (ch == ':') {AddCh(); goto case 76;}
				else {t.kind = 34; break;}
			case 199:
				recEnd = pos; recKind = 49;
				if (ch == '|') {AddCh(); goto case 141;}
				else {t.kind = 49; break;}
			case 200:
				recEnd = pos; recKind = 89;
				if (ch == 'I') {AddCh(); goto case 98;}
				else {t.kind = 89; break;}
			case 201:
				recEnd = pos; recKind = 85;
				if (ch == '}') {AddCh(); goto case 143;}
				else if (ch == ')') {AddCh(); goto case 145;}
				else if (ch == '|') {AddCh(); goto case 149;}
				else {t.kind = 85; break;}
			case 202:
				if (ch == '=') {AddCh(); goto case 213;}
				else {goto case 0;}
			case 203:
				recEnd = pos; recKind = 82;
				if (ch == '=') {AddCh(); goto case 148;}
				else {t.kind = 82; break;}
			case 204:
				recEnd = pos; recKind = 2;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 2;}
				else if (ch == '-') {AddCh(); goto case 157;}
				else if (ch == '+') {AddCh(); goto case 158;}
				else if (ch == '>') {AddCh(); goto case 162;}
				else {t.kind = 2; break;}
			case 205:
				recEnd = pos; recKind = 2;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 2;}
				else if (ch == '>') {AddCh(); goto case 160;}
				else {t.kind = 2; break;}
			case 206:
				if (ch == 'y') {AddCh(); goto case 214;}
				else {goto case 0;}
			case 207:
				if (ch == 'o') {AddCh(); goto case 49;}
				else if (ch == 'a') {AddCh(); goto case 58;}
				else {goto case 0;}
			case 208:
				if (ch == 'o') {AddCh(); goto case 71;}
				else if (ch == 'l') {AddCh(); goto case 104;}
				else {goto case 0;}
			case 209:
				if (ch == 'n') {AddCh(); goto case 215;}
				else {goto case 0;}
			case 210:
				if (ch == 'f') {AddCh(); goto case 115;}
				else if (ch == 'e') {AddCh(); goto case 216;}
				else if (ch == 'c') {AddCh(); goto case 122;}
				else if (ch == 'm') {AddCh(); goto case 217;}
				else if (ch == 'p') {AddCh(); goto case 131;}
				else if (ch == 's') {AddCh(); goto case 138;}
				else {goto case 0;}
			case 211:
				if (ch == 'o') {AddCh(); goto case 218;}
				else {goto case 0;}
			case 212:
				recEnd = pos; recKind = 76;
				if (ch == '=') {AddCh(); goto case 152;}
				else {t.kind = 76; break;}
			case 213:
				recEnd = pos; recKind = 78;
				if (ch == '>') {AddCh(); goto case 151;}
				else {t.kind = 78; break;}
			case 214:
				if (ch == 'p') {AddCh(); goto case 219;}
				else {goto case 0;}
			case 215:
				if (ch == 'b') {AddCh(); goto case 220;}
				else {goto case 0;}
			case 216:
				if (ch == 'x') {AddCh(); goto case 221;}
				else {goto case 0;}
			case 217:
				if (ch == 'a') {AddCh(); goto case 127;}
				else if (ch == 'i') {AddCh(); goto case 129;}
				else {goto case 0;}
			case 218:
				if (ch == 'n') {AddCh(); goto case 222;}
				else {goto case 0;}
			case 219:
				if (ch == 'e') {AddCh(); goto case 223;}
				else {goto case 0;}
			case 220:
				if (ch == 'o') {AddCh(); goto case 224;}
				else {goto case 0;}
			case 221:
				if (ch == 'i') {AddCh(); goto case 225;}
				else {goto case 0;}
			case 222:
				if (ch == 'v') {AddCh(); goto case 226;}
				else {goto case 0;}
			case 223:
				if (ch == 'P') {AddCh(); goto case 14;}
				else if (ch == 't') {AddCh(); goto case 54;}
				else if (ch == 'o') {AddCh(); goto case 66;}
				else {goto case 0;}
			case 224:
				if (ch == 'x') {AddCh(); goto case 227;}
				else {goto case 0;}
			case 225:
				if (ch == 's') {AddCh(); goto case 228;}
				else {goto case 0;}
			case 226:
				if (ch == '_') {AddCh(); goto case 229;}
				else {goto case 0;}
			case 227:
				recEnd = pos; recKind = 45;
				if (ch == 'A') {AddCh(); goto case 176;}
				else {t.kind = 45; break;}
			case 228:
				if (ch == 't') {AddCh(); goto case 230;}
				else {goto case 0;}
			case 229:
				if (ch == 'I') {AddCh(); goto case 231;}
				else if (ch == 'U') {AddCh(); goto case 232;}
				else {goto case 0;}
			case 230:
				if (ch == 's') {AddCh(); goto case 233;}
				else {goto case 0;}
			case 231:
				recEnd = pos; recKind = 101;
				if (ch == '1') {AddCh(); goto case 179;}
				else if (ch == '2') {AddCh(); goto case 180;}
				else if (ch == '4') {AddCh(); goto case 181;}
				else if (ch == '8') {AddCh(); goto case 182;}
				else {t.kind = 101; break;}
			case 232:
				recEnd = pos; recKind = 106;
				if (ch == '1') {AddCh(); goto case 183;}
				else if (ch == '2') {AddCh(); goto case 184;}
				else if (ch == '4') {AddCh(); goto case 185;}
				else if (ch == '8') {AddCh(); goto case 186;}
				else {t.kind = 106; break;}
			case 233:
				recEnd = pos; recKind = 58;
				if (ch == '1') {AddCh(); goto case 121;}
				else {t.kind = 58; break;}

		}
		t.val = new String(tval, 0, tlen);
		return t;
	}

	private void SetScannerBehindT() {
		buffer.Pos = t.pos;
		NextCh();
		line = t.line; col = t.col;
		for (int i = 0; i < tlen; i++) NextCh();
	}

	// get the next token (possibly a token already seen during peeking)
	public Token/*!*/ Scan () {
	 Contract.Ensures(Contract.Result<Token>() != null);
		if (tokens.next == null) {
			return NextToken();
		} else {
			pt = tokens = tokens.next;
			return tokens;
		}
	}

	// peek for the next token, ignore pragmas
	public Token/*!*/ Peek () {
	  Contract.Ensures(Contract.Result<Token>() != null);
		do {
			if (pt.next == null) {
				pt.next = NextToken();
			}
			pt = pt.next;
		} while (pt.kind > maxT); // skip pragmas

		return pt;
	}

	// make sure that peeking starts at the current scan position
	public void ResetPeek () { pt = tokens; }

} // end Scanner

public delegate void ErrorProc(int n, string filename, int line, int col);


}