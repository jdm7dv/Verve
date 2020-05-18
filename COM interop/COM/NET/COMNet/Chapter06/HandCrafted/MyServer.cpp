#include "Defs.h"

// The module attribute is specified in order to implement DllMain,
// DllRegisterServer and DllUnregisterServer
[ module(dll, name = "MyServer", helpstring = "MyServer 1.0 Type Library") ];
[ emitidl ];

// ISimplest
[
   object,
   uuid("103FF9D9-8BC9-4ea8-8CD4-C1E627D04358"),
   dual, helpstring("IObject1 Interface"),
   pointer_default(unique)
]
__interface ISimplest : IDispatch
{
   HRESULT Square([in] short val, [out, retval]long* pResult);
};

// CObject1
[
   coclass,
   threading("apartment"),
   vi_progid("MyServer.Object1"),
   progid("MyServer.Object1.1"),
   version(1.0),
   uuid("15615078-523C-43A0-BE6F-651E78A89213"),
   helpstring("Object1 Class")
]
class ATL_NO_VTABLE CObject1 : public ISimplest
{
public:
   CObject1() { }

   HRESULT Square(short val, long* pResult){
      *pResult = val * val;
      return S_OK;
   }

   DECLARE_PROTECT_FINAL_CONSTRUCT()
   HRESULT FinalConstruct()
   {
      return S_OK;
   }
   
   void FinalRelease() { }
};
