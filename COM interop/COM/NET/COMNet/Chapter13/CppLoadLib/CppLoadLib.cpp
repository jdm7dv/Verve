// This is the main project file for VC++ application project 
// generated using an Application Wizard.

#include "stdafx.h"

#using <mscorlib.dll>
using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Text;

// The Platform Invoke prototype for LoadLibrary
[DllImport("kernel32.dll", CharSet=CharSet::Auto, SetLastError=true)]
extern "C" void* LoadLibrary([MarshalAs(UnmanagedType::LPTStr)] String* pLibName);

// Flag for use with FormatMessage
const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;                  

// The Platform Invoke prototype for FormatMessage
[DllImport("kernel32.dll", CharSet=CharSet::Auto)]
extern "C" int FormatMessage(int flags, 
               void* source, int messageId,
               int langId, StringBuilder* buff, 
               int size, void* args);   

// The DLL to be loaded is called TestDll.dll, but I want to specify the
// path at runtime... it isn't in any of the standard places
[DllImport("testdll.dll", CharSet=CharSet::Auto)]
extern "C" long Square(short nVal); 

void main()
{
   // Load the DLL
   void* pLib = LoadLibrary("c:\\temp\\testdll\\debug\\ testdll.dll");

   // If there is an error, display the message
   if (pLib == 0)
   {
       int nErrCode = Marshal::GetLastWin32Error();
       StringBuilder* pBuff = new StringBuilder(256);
       FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, 
                           0, nErrCode, 0, 
                           pBuff, pBuff->Capacity, 0);

       Console::WriteLine("Error from LoadLibrary: {0}", pBuff->ToString());
      return;
   }

   Console::WriteLine("Library loaded OK");

   // Else call the unmanaged function
   long lRes = Square(5);
   Console::WriteLine("Square of 5 is {0}", __box(lRes));
}
