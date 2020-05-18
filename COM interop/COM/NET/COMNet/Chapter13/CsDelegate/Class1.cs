using System;

namespace CsDelegate
{
	class Class1
	{
		public delegate double TrigDelegate(double angleInRadians);

		public static double Sin(double angleInRadians) {
			double d = Math.Sin(angleInRadians);
			Console.WriteLine("Sin of 1.0 radians is {0}", d);
			return d;
		}

		public static double Tan(double angleInRadians) {
			double d = Math.Tan(angleInRadians);
			Console.WriteLine("Tan of 1.0 radians is {0}", d);
			return d;
		}

		[STAThread]
		static void Main(string[] args)
		{
			TrigDelegate delSin = new TrigDelegate(Class1.Sin);
			TrigDelegate delTan = new TrigDelegate(Class1.Tan);

			delSin += delTan;

			double d = delSin(1.0);
		}
	}
}
