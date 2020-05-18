using System;

namespace CsDispose
{
	class Test : IDisposable {
		private bool bDisposed = false;

		#region IDisposable Members

		public void Dispose() {
			// Release the resources
			InternalDispose(true);

			// Suppress finalization for this object
			GC.SuppressFinalize(this);
		}

		#endregion

		protected virtual void InternalDispose(bool bFreeAll) {
			if (bFreeAll) {
				// Free managed resources
			}
			// Free unmanaged resources

			bDisposed = true;
		}

		// Destructor
		~Test() {
			InternalDispose(false);
		}

		// Methods must check if the object has been disposed
		public void DoSomething() {
			if (bDisposed)
				throw new ObjectDisposedException("Test");
		}
	}

	class Class1
	{
		[STAThread]
		static void Main(string[] args)
		{
		}
	}
}
