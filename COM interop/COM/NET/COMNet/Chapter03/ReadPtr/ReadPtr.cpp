#include <iostream>
using namespace std;

#using <mscorlib.dll>

using namespace System;
using namespace System::Runtime::InteropServices;

void main()
{
   // Allocate memory for an int
   IntPtr ip = Marshal::AllocHGlobal(sizeof(int));
   // Grab the pointer and write to the memory
   int* pInt = (int*)ip.ToPointer();
   *pInt = 43;

   // Read it...
   Int32 n = Marshal::ReadInt32(ip);
   cout << "n is " << n << endl;
}

