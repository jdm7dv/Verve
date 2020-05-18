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
   // Declare a wide string
   LPWSTR lpws = L"wide string";

   // Create a conversion object to let it be used as an
   // ANSI string. Use the Ex version to specify a 30-character
   // buffer
   CW2AEX<30> ca(lpws);

   // Call a function that expects an LPSTR, passing in the CW2AEX
   // instance
   func1(ca);

   return 0;
}
