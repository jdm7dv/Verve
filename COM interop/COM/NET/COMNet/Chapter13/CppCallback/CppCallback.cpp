#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
#include <windows.h>

#include <iostream>
using namespace std;

BOOL CALLBACK EWSProc(LPTSTR name, LPARAM lp)
{
   cout << "EWSProc: " << name << endl;
   return TRUE;   // continue enumeration
}

int main(int argc, char* argv[])
{
   BOOL bOK = EnumWindowStations(EWSProc, (LPARAM)0);
   cout << "Enum " << ((bOK) ? "OK" : "failed") << endl;

	return 0;
}

