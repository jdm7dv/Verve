using System;
using System.Runtime.InteropServices;

namespace Delegates
{
	class Class1
	{
		// Delegate used to represent callback function
		public delegate Int32 EnumWinStaDelegate(
			[MarshalAs(UnmanagedType.LPTStr)] string name,
			IntPtr lparam);

		// Platform Invoke prototype for EnumWindowStations
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern Int32 EnumWindowStations(EnumWinStaDelegate callbackfunc, IntPtr lparam);

		// Delegate instance
		static EnumWinStaDelegate del;

		// The callback function that is bound to the delegate
		static Int32 Callback(string name, IntPtr lparam) {
			// Get the string back from the IntPtr
			string sp = Marshal.PtrToStringAuto(lparam);

			if (name.Equals(sp))
				Console.WriteLine("Found required WinStation: {0}", name);
			else
				Console.WriteLine("WinStation: {0}", name);

			return 1;   // non-zero equates to true
		}

		[STAThread]
		static void Main(string[] args)
		{
			// Create the delegate, and bind it to the callback function
			del = new EnumWinStaDelegate(Callback);

			// Call the function
			string s = "WinSta0";
			IntPtr ip = Marshal.StringToHGlobalAuto(s);

			//Int32 result = EnumWindowStations(del, (IntPtr)0);
			Int32 result = EnumWindowStations(del, ip);
			if (result != 0)
				Console.WriteLine("Enum OK");
			else
				Console.WriteLine("Enum failed");

			Marshal.FreeHGlobal(ip);
		}
	}
}
