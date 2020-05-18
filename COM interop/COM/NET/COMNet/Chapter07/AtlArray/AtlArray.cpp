#include <iostream>
#include <string>
using namespace std;

#include <atlbase.h>
// Include the header for the ATL collections
#include <atlcoll.h>
using namespace ATL;

int main()
{
   // Declare two arrays and initialize them
   CAtlArray<string> strArray1;
   CAtlArray<string> strArray2;

   strArray1.Add(string("one"));
   strArray1.Add(string("two"));
   strArray1.Add(string("three"));

   strArray2.Add(string("giraffe"));
   strArray2.Add(string("zebra"));

   // Insert one array into the other
   strArray1.InsertArrayAt(1, &strArray2);

   cout << "Size of array 1 is " << strArray1.GetCount() << endl;
   for (unsigned int i=0; i<strArray1.GetCount(); i++)
      cout << "Element[" << i << "] = " << strArray1[i] << endl;

   return 0;
}
