#include <iostream>
using namespace std;

#include <atlbase.h>
// Include the header for the simple collections
#include <atlsimpcoll.h>
using namespace ATL;

// Helper class
template <typename T> class EqualHelper
{
public:
   static bool IsEqual(const T& t1, const T& t2)
   {
      return t1 == t2;
   }
};

// Specialization of helper class for long*
template <> class EqualHelper<long*>
{
public:
   static bool IsEqual(const long* t1, const long* t2)
   {
      return *t1 == *t2;
   }
};

int main()
{
   CSimpleArray<long*, EqualHelper<long*> > myArray;

   long l1 = 40000L;
   long l2 = 50000L;

   myArray.Add(&l1);
   myArray.Add(&l2);

   cout << "Element 0 is " << *(myArray[0]) << endl;

   return 0;
}
