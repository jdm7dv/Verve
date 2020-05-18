// Created by Microsoft (R) C/C++ Compiler Version 13.10.2215.1
//
// c:\writing\ms press\com book\6-attributes\simpleobject\simpleobject.mrg.cpp
// compiler-generated file created 10/28/02 at 11:38:29
//
// This C++ source file is intended to be a representation of the
// source code injected by the compiler.  It may not compile or
// run exactly as the original source file.
//

// SimpleObject.cpp : Implementation of DLL Exports.

#include "stdafx.h"
//+++ Start Injected Code
[no_injected_text(true)];      // Suppress injected text, it has already been injected
#pragma warning(disable: 4543) // Suppress warnings about skipping injected text
#pragma warning(disable: 4199) // Suppress warnings from attribute providers

// M00PRAGMA("\n\nNOTE: This merged source file should be visually inspected for correctness.\n\n")
//--- End Injected Code


#include "resource.h"

// The module attribute causes DllMain, DllRegisterServer and DllUnregisterServer to be automatically implemented for you
[ module(dll, uuid = "{1D196988-3060-486E-A4AC-38F9685D3BF7}", 
		 name = "SimpleObject", 
		 helpstring = "SimpleObject 1.0 Type Library",
		 resource_name = "IDR_SIMPLEOBJECT") ];

//+++ Start Injected Code For Attribute 'module'
#injected_line 7 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simpleobject.cpp"

#ifndef GUID_DEFINED
#define GUID_DEFINED
typedef struct _GUID 
{
    unsigned long  Data1;
    unsigned short Data2;
    unsigned short Data3;
    unsigned char  Data4[ 8 ];
} 
GUID;
#endif

extern "C" const __declspec(selectany) GUID __LIBID_ = {0x1d196988,0x3060,0x486e,{0xa4,0xac,0x38,0xf9,0x68,0x5d,0x3b,0xf7}};
struct __declspec(uuid("1d196988-3060-486e-a4ac-38f9685d3bf7")) SimpleObject;

//--- End Injected Code For Attribute 'module'


//+++ Start Injected Code For Attribute 'module'
#injected_line 7 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simpleobject.cpp"

class CSimpleObjectModule;

//--- End Injected Code For Attribute 'module'


//+++ Start Injected Code For Attribute 'module'
#injected_line 7 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simpleobject.cpp"

extern CSimpleObjectModule _AtlModule;

//--- End Injected Code For Attribute 'module'


//+++ Start Injected Code For Attribute 'module'
#injected_line 7 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simpleobject.cpp"
extern "C" STDAPI DllGetClassObject(REFCLSID rclsid, REFIID riid, void** ppv);

//--- End Injected Code For Attribute 'module'


//+++ Start Injected Code For Attribute 'module'
#injected_line 7 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simpleobject.cpp"
extern "C" STDAPI DllRegisterServer(void);

//--- End Injected Code For Attribute 'module'


//+++ Start Injected Code For Attribute 'module'
#injected_line 7 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simpleobject.cpp"
extern "C" STDAPI DllUnregisterServer(void);

//--- End Injected Code For Attribute 'module'


//+++ Start Injected Code For Attribute 'module'
#injected_line 7 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simpleobject.cpp"
extern "C" STDAPI DllCanUnloadNow(void);

//--- End Injected Code For Attribute 'module'


//+++ Start Injected Code For Attribute 'module'
#injected_line 7 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simpleobject.cpp"
BOOL WINAPI DllMain(HINSTANCE hInstance, DWORD dwReason, LPVOID lpReserved);

//--- End Injected Code For Attribute 'module'


//+++ Start Injected Code For Attribute 'module'
#injected_line 7 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simpleobject.cpp"

class CSimpleObjectModule : public CAtlDllModuleT<CSimpleObjectModule>
    								
{
public:
    DECLARE_LIBID(__uuidof(SimpleObject))
        										DECLARE_REGISTRY_APPID_RESOURCEID(IDR_SIMPLEOBJECT, "{1D196988-3060-486E-A4AC-38F9685D3BF7}")
        								
};

#injected_line 7 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simpleobject.cpp"

CSimpleObjectModule _AtlModule;

#injected_line 7 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simpleobject.cpp"
extern "C" STDAPI DllGetClassObject(REFCLSID rclsid, REFIID riid, void** ppv) 
{
    return _AtlModule.DllGetClassObject(rclsid, riid, ppv);
}
#injected_line 7 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simpleobject.cpp"
extern "C" STDAPI DllRegisterServer(void) 
{
    return _AtlModule.DllRegisterServer();
}
#injected_line 7 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simpleobject.cpp"
extern "C" STDAPI DllUnregisterServer(void) 
{
    return _AtlModule.DllUnregisterServer();
}
#injected_line 7 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simpleobject.cpp"
extern "C" STDAPI DllCanUnloadNow(void) 
{
    return _AtlModule.DllCanUnloadNow();
}
#injected_line 7 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simpleobject.cpp"
BOOL WINAPI DllMain(HINSTANCE hInstance, DWORD dwReason, LPVOID lpReserved) 
{
    hInstance;
    return _AtlModule.DllMain(dwReason, lpReserved);
}
#injected_line 7 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simpleobject.cpp"

#if defined(_M_IX86)
#pragma comment(linker, "/EXPORT:DllMain=_DllMain@12,PRIVATE")
#pragma comment(linker, "/EXPORT:DllRegisterServer=_DllRegisterServer@0,PRIVATE")
#pragma comment(linker, "/EXPORT:DllUnregisterServer=_DllUnregisterServer@0,PRIVATE")
#pragma comment(linker, "/EXPORT:DllGetClassObject=_DllGetClassObject@12,PRIVATE")
#pragma comment(linker, "/EXPORT:DllCanUnloadNow=_DllCanUnloadNow@0,PRIVATE")
#elif defined(_M_IA64)
#pragma comment(linker, "/EXPORT:DllMain,PRIVATE")
#pragma comment(linker, "/EXPORT:DllRegisterServer,PRIVATE")
#pragma comment(linker, "/EXPORT:DllUnregisterServer,PRIVATE")
#pragma comment(linker, "/EXPORT:DllGetClassObject,PRIVATE")
#pragma comment(linker, "/EXPORT:DllCanUnloadNow,PRIVATE")
#endif	

//--- End Injected Code For Attribute 'module'

