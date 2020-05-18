COCO = Coco.exe

# ###############################################################################
# The frame files are no longer in this directory. They must be downloaded
# from http://boogiepartners.codeplex.com/. Update the FRAME_DIR variable to
# point to whatever directory you install that into.
# ###############################################################################
FRAME_DIR = ..\..\..\..\boogiepartners\CocoR\Modified

# "all" depends on 2 files, really (Parser.cs and Scanner.cs), but they
# are both generated in one go and I don't know a better way to tell
# nmake that.  --KRML
all: DeserializerParser.cs

DeserializerParser.cs: $(FRAME_DIR)\Scanner.frame $(FRAME_DIR)\Parser.frame Deserializer.atg
	$(COCO) Deserializer.atg -namespace Omni -frames $(FRAME_DIR)
	copy parser.cs DeserializerParser.cs
	copy scanner.cs DeserializerScanner.cs

clean:
	rm -f DeserializerScanner.cs DeserializerParser.cs
