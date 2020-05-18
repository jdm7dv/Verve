// Application to test marshaling of structs
using System;
using System.Runtime.InteropServices;

namespace Structs
{
	class Class1
	{
		// The Point and Rect structures
		[StructLayout(LayoutKind.Sequential)]
		public struct Point {
			public int x,y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Rect {
			public Point p1,p2;
		}


		// The Person structure
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		public struct Person {
			public string firstName;
			public string lastName;
			public int age;
		}

		// The Bank account structure
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
		public struct Account {
			public IntPtr accountHolder;
			public string accountName;
			public long accountNumber;
			public double balance;
		}

		// Platform Invoke prototypes
		[DllImport("TestInterop.dll")]
		public static extern string GetAccountDetails(ref Account act);

		[DllImport("TestInterop.dll")]
		public static extern int GetArea(ref Rect rct);

		[STAThread]
		static void Main(string[] args)
		{
			Rect r;
			r.p1.x = r.p1.y = 10;
			r.p2.x = r.p2.y = 15;

			int area = GetArea(ref r);
			Console.WriteLine("Area is {0}", area);

			Person p;
			p.firstName = "Fred";
			p.lastName = "Smith";
			p.age = 45;

			Account a;
			a.accountName = "Fred's account";
			a.accountNumber = 10000;
			a.balance = 0.0;

			// Allocate memory for a Person
			IntPtr ip = Marshal.AllocHGlobal(Marshal.SizeOf(p));
			// Marshal the person structure into the memory, but don't delete the original
			Marshal.StructureToPtr(p, ip, false);

			a.accountHolder = ip;

			// Make the call
			string details = GetAccountDetails(ref a);

			Console.WriteLine("Account: {0}", details);

			// Free the memory
			Marshal.FreeHGlobal(ip);
		}
	}
}
