using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CsLastError
{
	class TestPI
	{
		// Access modes, from Winnt.h
		const uint GENERIC_READ = 0x80000000;
		const uint GENERIC_WRITE = 0x40000000;
		const uint GENERIC_EXECUTE = 0x20000000;
		const uint GENERIC_ALL = 0x10000000;

		// Creation flags from WinBase.h
		const uint CREATE_NEW = 1;
		const uint CREATE_ALWAYS = 2;
		const uint OPEN_EXISTING = 3;
		const uint OPEN_ALWAYS = 4;
		const uint TRUNCATE_EXISTING = 5;

		// Attribute flags from Winnt.h
		const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

		// The Platform Invoke prototype
		[DllImport("kernel32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		public static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPTStr)]string name, 
			uint accessMode, uint shareMode, IntPtr secAtts,
			uint createFlags, uint attributes, IntPtr template);
		
		// FormatMessage API
		public const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;						

		[DllImport("kernel32.dll")]
		public static extern int FormatMessage(int flags, IntPtr source, int messageId,
			int langId, StringBuilder buff, int size, IntPtr args);	

		[STAThread]
		static void Main(string[] args)
		{
			int errCode = Marshal.GetLastWin32Error();
			Console.WriteLine( "GetLastError when program starts: {0}", errCode );

			IntPtr p = CreateFile(@"c:\temp\test.txt", TestPI.GENERIC_READ, 0, IntPtr.Zero,
				TestPI.OPEN_EXISTING, TestPI.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);

			errCode = Marshal.GetLastWin32Error();
			Console.WriteLine( "GetLastError after call to CreateFile: {0}", errCode );
		
			if (errCode != 0) {
				StringBuilder buff = new StringBuilder( 256 );
				FormatMessage(TestPI.FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, errCode, 0, 
					buff, buff.Capacity, IntPtr.Zero);
				Console.WriteLine("Error message: {0}", buff);
			}
		}
	}
}
