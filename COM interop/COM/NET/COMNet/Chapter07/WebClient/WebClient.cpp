#include <iostream>
using namespace std;

#define _WIN32_WINNT 0x0500

// Include the generated header
#include "webservice.h"

// The namespace created for the service
using namespace AtlWebSrvService;

int main()
{
   // COM must be initialized before using web services
   CoInitialize(NULL);

   {
      CAtlWebSrvService theService;

      CComBSTR bResp;
      HRESULT hr = theService.HelloWorld(CComBSTR(L"foo"), &bResp);
      if (FAILED(hr))
      {
         wcout << L"HelloWorld failed (" << hex << hr << dec << L")" << endl;
      }

      wcout << static_cast<const wchar_t*>(bResp) << endl;
   }

   CoUninitialize();

   return 0;
}