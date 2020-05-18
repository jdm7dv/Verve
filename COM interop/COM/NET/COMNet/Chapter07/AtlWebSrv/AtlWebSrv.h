// AtlWebSrv.h : Defines the ATL Server request handler class
//
#pragma once

namespace AtlWebSrvService
{
// all struct, enum, and typedefs for your webservice should go inside the namespace

// IAtlWebSrvService - web service interface declaration
//
[
	uuid("5DA24F49-9FF5-44B3-A638-C0B3EDBB28D2"), 
	object
]
__interface IAtlWebSrvService
{
	// HelloWorld is a sample ATL Server web service method.  It shows how to
	// declare a web service method and its in-parameters and out-parameters
	[id(1)] HRESULT HelloWorld([in] BSTR bstrInput, [out, retval] BSTR *bstrOutput);
	[id(2)] HRESULT Square([in] short nVal, [out, retval] long* pResult);
};


// AtlWebSrvService - web service implementation
//
[
	request_handler(name="Default", sdl="GenAtlWebSrvWSDL"),
	soap_handler(
		name="AtlWebSrvService", 
		namespace="urn:AtlWebSrvService",
		protocol="soap"
	)
]
class CAtlWebSrvService :
	public IAtlWebSrvService
{
public:
	// This is a sample web service method that shows how to use the 
	// soap_method attribute to expose a method as a web method
	[ soap_method ]
	HRESULT HelloWorld(/*[in]*/ BSTR bstrInput, /*[out, retval]*/ BSTR *bstrOutput)
	{
		CComBSTR bstrOut(L"Hello ");
		bstrOut += bstrInput;
		bstrOut += L"!";
		*bstrOutput = bstrOut.Detach();
		
		return S_OK;
	}

	[ soap_method ]
	HRESULT Square(/*[in]*/ short nVal, /*[out, retval]*/ long* pResult)
	{
      *pResult = nVal * nVal;

		return S_OK;
	}
}; // class CAtlWebSrvService

} // namespace AtlWebSrvService
