#define _ATL_ATTRIBUTES 1
#include <atlbase.h>
#include <atlcom.h>
#include "EvtSrc.h"

[ module(DLL, name="EventSource", uuid="6E46B59E-89C3-4c15-A6D8-B8A1CEC98830") ];

[coclass, event_source(com), uuid("530DF3AD-6936-3214-A83B-27B63C7997C4")]
class CEventSource : public ISource {
public:
   __event __interface IEvents; 

   // This method fires the event
   HRESULT Fire() {
      __raise Values(10,12);
      return S_OK;
   }
};
