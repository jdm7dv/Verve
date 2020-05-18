// Simplest.h : Declaration of the CSimplest

#pragma once
#include "resource.h"       // main symbols


// ISimplest
[
	object,
	uuid("FE19D164-DB7D-4A17-8D99-DFD57FB69E02"),
	dual,	helpstring("ISimplest Interface"),
	pointer_default(unique)
]
__interface ISimplest : IDispatch
{
	[propget, id(2), helpstring("property X")] HRESULT X([out, retval] short* pVal);
	[propput, id(2), helpstring("property X")] HRESULT X([in] short newVal);
	[id(3), helpstring("method Square")] HRESULT Square([in] SHORT val, [out,retval] LONG* pResult);
};



// CSimplest

[
	coclass,
	threading("apartment"),
	vi_progid("SimpleObject.Simplest"),
	progid("SimpleObject.Simplest.1"),
	version(1.0),
	uuid("5BDCE1E1-D79D-41E1-9500-E4ED3E64887A"),
	helpstring("Simplest Class")
]
class ATL_NO_VTABLE CSimplest : 
	public ISimplest
{
public:
	CSimplest()
	{
		n = 0;
	}


	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}
	
	void FinalRelease() 
	{
	}

	short n;

public:

	STDMETHOD(get_X)(short* pVal);
	STDMETHOD(put_X)(short newVal);
	STDMETHOD(Square)(SHORT val, LONG* pResult);
};

