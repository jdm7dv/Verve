#include <iostream>
using namespace std;

#include <atlsimpstr.h>
#include <atlstr.h>
#include <atlmem.h> 
using namespace ATL;

int main()
{
   CCRTHeap hm;
   // Provides a default implementation of IAtlStringMgr
   CAtlStringMgr sm(&hm);
   CSimpleStringT<char> s1("hello", &sm);

   cout << "string is '" << s1 << "'"<< endl;
   cout << "length is '" << s1.GetLength() << "'"<< endl;

   s1.SetAt(0, 'H');
   cout << "string is '" << s1 << "'"<< endl;

   CSimpleStringT<char> s2("hello", new CAtlStringMgr(new CCRTHeap()));

   return 0;
}
