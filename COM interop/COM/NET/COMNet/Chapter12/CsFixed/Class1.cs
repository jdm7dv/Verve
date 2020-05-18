using System;

namespace CsFixed
{
	class Class1
	{
		static unsafe int Sum(int[] arr) 
		{
			int sum = 0;
			fixed (int* pa = &arr[0])
			{
				for(int i=0; i<arr.Length; i++)
					sum += *(pa + i);
			}

			return sum;
		}

		[STAThread]
		static void Main(string[] args)
		{
			int[] arr = new int[] { 2, 3, 4, 5, 6, 7 };

			Console.WriteLine("Sum = {0}", Sum(arr));
		}
	}
}
