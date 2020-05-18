using System;
using System.Runtime.InteropServices;

namespace Bank
{
	// Interface to allow COM clients to use events.
	// Define it as a pure dispatch interface
	[
	  Guid("ef81a831-fa49-4ede-b70b-08f2e9d602b2"),
	  InterfaceType(ComInterfaceType.InterfaceIsIDispatch)
	]
	public interface IAccountEvents {
		void NewRate(double val);
	}

	public interface IAccount {
		double Rate { get; }
		void SetRate(double d);
	}

	// Delegate for rate change event
	[ComVisible(false)]
	public delegate void RateDelegate(double val);

	// Export the Account class, exposing IAccountEvents
	// as a source interface
	[ClassInterface(ClassInterfaceType.None),
	ComSourceInterfaces(typeof(IAccountEvents))]
	public class Account : IAccount {
		private static double interestRate = 0;

		// Define the event
		public event RateDelegate NewRate;

		public Account() {
		}

		public double Rate {
			get {
				return interestRate;
			}
		}

		// Fire the event
		public void SetRate(double d) {
			NewRate(d);
		}
	}
}
