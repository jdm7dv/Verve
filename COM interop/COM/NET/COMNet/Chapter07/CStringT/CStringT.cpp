#include <windows.h>

#include <iostream>
using namespace std;

#undef ATL_CSTRING_NO_CRT

#include <atlconv.h>
#include <atlstr.h>
using namespace ATL;

void func1(LPSTR lps)
{
   cout << "func1: string is '" << lps << "'" << endl;
}

int main()
{
   CAtlStringA s1("hello");

   cout << "string is '" << s1 << "'"<< endl;
   cout << "length is '" << s1.GetLength() << "'"<< endl;

   s1 += "world";

   s1.SetAt(0, 'H');
   s1.Insert(5, ' ');

   cout << "string is '" << s1 << "'"<< endl;

   LPWSTR lpws = L"wide string";
   CW2AEX<30> ca(lpws);
   func1(ca);

   return 0;
}
