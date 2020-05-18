#include "stdafx.h"

#using <mscorlib.dll>
using namespace System;
using namespace System::Text;

// Needed for interop
using namespace System::Runtime::InteropServices;

// Flags for use with MessageBox
#define MB_OK                       0x00000000L
#define MB_OKCANCEL                 0x00000001L
#define MB_ABORTRETRYIGNORE         0x00000002L
#define MB_YESNOCANCEL              0x00000003L
#define MB_NOTAREALFLAG             0x00000999L

// Set up the import
typedef void* HWND;
[DllImport("User32.dll", CharSet=CharSet::Auto, SetLastError=true)]
extern "C" int MessageBox(HWND hw, String* text, 
                      String* caption, unsigned int type);

// Flag for use with FormatMessage
const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;                  

// The Platform Invoke prototype for FormatMessage
[DllImport("kernel32.dll", CharSet=CharSet::Auto)]
extern "C" int FormatMessage(int flags, 
               void* source, int messageId,
               int langId, StringBuilder* buff, 
               int size, void* args);   

void main()
{
    String* theText = S"Hello World!";
    String* theCaption = S"A Message Box...";

    // Provide an invalid 
    int nRet = MessageBox(0, theText, theCaption, MB_NOTAREALFLAG);
    if (nRet == 0)
    {
       int nErrCode = Marshal::GetLastWin32Error();
       StringBuilder* pBuff = new StringBuilder(256);
       FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, 
                           0, nErrCode, 0, 
                           pBuff, pBuff->Capacity, 0);

       Console::WriteLine("Error from MessageBox: {0}", pBuff->ToString());
    }
}