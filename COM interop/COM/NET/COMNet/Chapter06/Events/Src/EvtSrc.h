// EvtSrc.h

#pragma once

[ dual, uuid("d4efa6dc-bb8f-44f0-88cf-2ae663c76312") ]
__interface IEvents {
   [id(1)] HRESULT Values([in] short nVal1, [in] short nVal2);
};

[ dual, uuid("a3b7fea2-7396-4727-9691-7dc55acca27a") ]
__interface ISource {
   [id(1)] HRESULT Fire();
};

class DECLSPEC_UUID("530DF3AD-6936-3214-A83B-27B63C7997C4") CEventSource;
