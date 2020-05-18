// This is the main project file for VC++ application project 
// generated using an Application Wizard.

#include "stdafx.h"

#include <iostream>
using namespace std;

#using <mscorlib.dll>
using namespace System;
using namespace System::Runtime::InteropServices;

void main()
{
   // An instance of a managed type
   String* s1 = S"A string";

   // Create a pinned handle
   GCHandle gh = GCHandle::Alloc(s1, GCHandleType::Pinned);

   // Conversion to an IntPtr
   IntPtr ip = GCHandle::op_Explicit(gh);

   // Get the address and cast to a wchar_t
   wchar_t* buff = reinterpret_cast<wchar_t*>(gh.AddrOfPinnedObject().ToInt32());

   // Modify the buffer
   buff[2] = L'S';
   wcout << "buffer is '" << buff << "'" << endl;

   // Free the handle
   gh.Free();

   Console::WriteLine("String is {0}", s1);
}