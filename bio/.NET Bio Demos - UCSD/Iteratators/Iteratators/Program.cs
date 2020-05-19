using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Iteratators
{
    class Program
    {
        static void Main(string[] args)
        {
            //foreach (int value in GetNumbers(10, 10))
            //{
            //    if (value % 4 == 0)
            //        break;
            //    Console.WriteLine(value);
            //}

            IEnumerable<int> enumerable = GetNumbers(10, 10);
            IEnumerator<int> enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                int value = enumerator.Current;
                if (value % 4 == 0)
                    break;
                Console.WriteLine(value);
            }

        }

        static IEnumerable<int> GetNumbers(int start, int count)
        {
            //var vals = new List<int>();
            for (int i = start; i < start+count; i++)
            {
                //vals.Add(i);
                yield return i;
                Thread.Sleep(250);
            }
            //return vals;
        }
    }
}
