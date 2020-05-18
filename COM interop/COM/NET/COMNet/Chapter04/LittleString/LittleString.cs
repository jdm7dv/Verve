using System;
using System.Runtime.InteropServices;

// Auto-generated GUID, dual interface
[InterfaceType(ComInterfaceType.InterfaceIsDual)]
public interface IString {
	string Text { get; set; }
	string ToUpper();
	string ToLower();
	string SubString(int start, int length);
} 

// Auto-generated GUID, no class interface
[ClassInterface(ClassInterfaceType.None)]
public class LittleString : IString
{
	private string text;

	// Default constructor needed by COM
	public LittleString()
	{
	}

	// Accessor for the text
	public string Text {
		get {
			return text;
		}
		set {
			text = value;
		}
	}

	public string ToLower() {
		return text.ToLower();
	}

	public string ToUpper() {
		return text.ToUpper();
	}

	public string SubString(int start, int length) {
		return text.Substring(start, length);
	}
}
