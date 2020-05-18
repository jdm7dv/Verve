#define _ATL_ATTRIBUTES 1
#include <atlbase.h>
#include <atlcom.h>

#include <iostream>
using namespace std;

#include "..\Src\EvtSrc.h"

[ module(name="EventTest") ];

[ event_receiver(com) ]
class EventHandlerClass {
public:
   HRESULT Handler1(short nVal1, short nVal2) {
      cout << "Handler1 called with values " << nVal1 << " and " << nVal2 << endl;
      return S_OK;
   }

   HRESULT Handler2(short nVal1, short nVal2) {
      cout << "Handler2 called with values " << nVal1 << " and " << nVal2 << endl;
      return S_OK;
   }

   // Hook up two handlers to the same event source
   void HookEvent(ISource* pSource) {
      __hook(&IEvents::Values, pSource, &EventHandlerClass::Handler1);
      __hook(&IEvents::Values, pSource, &EventHandlerClass::Handler2);
   }

   // Unhook the event handlers
   void UnhookEvent(ISource* pSource) {
      __unhook(&IEvents::Values, pSource, &EventHandlerClass::Handler1);
      __unhook(&IEvents::Values, pSource, &EventHandlerClass::Handler2);
   }
};

int main() {
   // Create COM object
   CoInitialize(NULL);
   ISource* pSource = 0;
   HRESULT hr = CoCreateInstance(__uuidof(CEventSource), NULL, CLSCTX_ALL, 
      __uuidof(ISource), (void **) &pSource);
   if (FAILED(hr)) {
      cout << "Error creating CEventSource object: " << hex << hr << dec << endl;
      return -1;
   }

   // Create the handler object, and set up the event notification
   EventHandlerClass theHandler;
   theHandler.HookEvent(pSource);

   // Fire the event
   pSource->Fire();

   // Unadvise
   theHandler.UnhookEvent(pSource);

   CoUninitialize();

   return 0;
}
