#include <iostream>
#include <string>
using namespace std;

#include <atlbase.h>
// Include the header for CComSafeArray
#include <atlsafe.h>
using namespace ATL;

int main()
{
   // Create a 1D array with four elements, and a default
   // lower bound of zero.
   CComSafeArray<char> oneDArray(4);
   // Put an 'x' in element 0, a 'y' in element 1, and 'z' in 
   // the last two elements
   oneDArray.SetAt(0, 'x');
   oneDArray.SetAt(1, 'y');
   oneDArray.SetAt(2, 'z');
   oneDArray.SetAt(3, 'z');

   for(int i=0; i<oneDArray.GetCount(); i++)
      cout << oneDArray[i] << " ";
   cout << endl;

   // Create a CComSafeArrayBound object for each dimension
   // Set the lower bound to zero in each case
   CComSafeArrayBound bounds[2];
   bounds[0].SetLowerBound(0);
   bounds[0].SetCount(2);
   bounds[1].SetLowerBound(0);
   bounds[1].SetCount(5);

   // Create an array object
   CComSafeArray<int> twoDArray(bounds, 2);

   // Array to hold indices
   LONG index[2];

   for(i=0; i<2; i++)
   {
      index[0] = i;
      for(int j=0; j<5; j++)
      {
         index[1] = j;
         twoDArray.MultiDimSetAt(index, i*j);
      }
   }

   // Get an element
   index[0] = 2;
   index[1] = 3;
   int val = 0;
   twoDArray.MultiDimGetAt(index, val);
   cout << "Element[2,3] = " << val << endl;

   return 0;
}
