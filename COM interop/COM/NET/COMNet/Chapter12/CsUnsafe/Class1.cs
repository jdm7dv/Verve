using System;
using System.Runtime.InteropServices;


public class WinApi {
	// Flags for use with MessageBox
	public static int MB_OK = 0x00000000;
	public static int MB_OKCANCEL = 0x00000001;
	public static int MB_ABORTRETRYIGNORE = 0x00000002;
	public static int MB_YESNOCANCEL = 0x00000003;

	// Note the use of 'unsafe' on the prototype
	[DllImport("User32.dll", CharSet=CharSet.Ansi)]
	public extern static unsafe int MessageBox(int hWnd, char* txt, 
		char* caption, int type);
}

class Class1
{
	[STAThread]
	static void Main(string[] args)
	{
		unsafe {
			string sText = "Called from Unsafe Code";
			IntPtr pTxt = Marshal.StringToHGlobalAnsi(sText);
			string sCaption = "Testing...";
			IntPtr pCaption = Marshal.StringToHGlobalAnsi(sCaption);

			int nRet = WinApi.MessageBox(0, (char*)pTxt.ToPointer(), 
				(char*)pCaption.ToPointer(), WinApi.MB_OK);
		}
	}
}