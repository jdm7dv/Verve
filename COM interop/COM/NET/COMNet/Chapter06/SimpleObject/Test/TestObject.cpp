#include <iostream>
#include "atlbase.h"
#import "..\_SimpleObject.tlb" no_namespace
using namespace std;

void main()
{
   CoInitialize(NULL);
   {
      //CComPtr<IUnknown> spUnknown;
      //spUnknown.CoCreateInstance(__uuidof(CSimplest));
      //CComPtr<ISimplest> pI;
      //spUnknown.QueryInterface(&pI);


      CComPtr<ISimplest> pI;
      pI.CoCreateInstance(__uuidof(CSimplest));

      long res = 0;
      res = pI->Square(3);
      cout << "Result is " << res << endl;
   }
   CoUninitialize();
}
