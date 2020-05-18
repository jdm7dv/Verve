// SimpleObject.cpp : Implementation of DLL Exports.

#include "stdafx.h"
#include "resource.h"

// The module attribute causes DllMain, DllRegisterServer and DllUnregisterServer to be automatically implemented for you
[ module(dll, uuid = "{1D196988-3060-486E-A4AC-38F9685D3BF7}", 
		 name = "SimpleObject", 
		 helpstring = "SimpleObject 1.0 Type Library",
		 resource_name = "IDR_SIMPLEOBJECT") ];
