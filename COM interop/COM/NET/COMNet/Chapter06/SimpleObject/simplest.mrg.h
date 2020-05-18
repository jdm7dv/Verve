// Created by Microsoft (R) C/C++ Compiler Version 13.10.2215.1
//
// c:\writing\ms press\com book\6-attributes\simpleobject\simplest.mrg.h
// compiler-generated file created 10/28/02 at 11:38:29
//
// This C++ source file is intended to be a representation of the
// source code injected by the compiler.  It may not compile or
// run exactly as the original source file.
//


//+++ Start Injected Code
[no_injected_text(true)];      // Suppress injected text, it has already been injected
#pragma warning(disable: 4543) // Suppress warnings about skipping injected text
#pragma warning(disable: 4199) // Suppress warnings from attribute providers

// M00PRAGMA("\n\nNOTE: This merged source file should be visually inspected for correctness.\n\n")
//--- End Injected Code

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
,
    /*+++ Added Baseclass */ public CComCoClass<CSimplest, &__uuidof(CSimplest)>,
    /*+++ Added Baseclass */ public CComObjectRootEx<CComSingleThreadModel>,
    /*+++ Added Baseclass */ public IProvideClassInfoImpl<&__uuidof(CSimplest)>
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

	//+++ Start Injected Code For Attribute 'coclass'
#injected_line 32 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simplest.h"
    virtual HRESULT STDMETHODCALLTYPE ISimplest::Invoke(
                /* [in] */ DISPID dispIdMember,
                /* [in] */ REFIID riid,
                /* [in] */ LCID lcid,
                /* [in] */ WORD wFlags,
                /* [out][in] */ DISPPARAMS *pDispParams,
                /* [out] */ VARIANT *pVarResult,
                /* [out] */ EXCEPINFO *pExcepInfo,
                /* [out] */ UINT *puArgErr) 
    {
        (void) riid;
        (void) dispIdMember;
        (void) lcid;
        (void) wFlags;
        (void) pExcepInfo;
        (void) puArgErr;
        HRESULT hr = S_OK;
        if (pDispParams == 0) {
            return DISP_E_BADVARTYPE;
        }
        if (pDispParams->cArgs > 2) {
            return DISP_E_BADPARAMCOUNT;
        }
        if (pVarResult != 0) {
            ::VariantInit(pVarResult);
        }
        VARIANT* rgpVars[2];
        UINT index = 0;
        if (pDispParams->cNamedArgs > 0) {
            if (pDispParams->rgdispidNamedArgs[0] == DISPID_PROPERTYPUT) {
                rgpVars[0] = &pDispParams->rgvarg[0];
                index = 1;
            }
            for (; index < pDispParams->cNamedArgs; ++index) {
                if (pDispParams->rgdispidNamedArgs[index] >= (int) pDispParams->cArgs || pDispParams->rgdispidNamedArgs[index] < 0) {
                    if (puArgErr != 0) {
                        *puArgErr = index;
                    }
                    return DISP_E_PARAMNOTFOUND;
                }
                rgpVars[pDispParams->cArgs - pDispParams->rgdispidNamedArgs[index] - 1] = &pDispParams->rgvarg[index];
            }
        }
        for (; index < pDispParams->cArgs; ++index) {
            rgpVars[index] = &pDispParams->rgvarg[index];
        }
        VARIANT v0;
        VARIANT* v;
        switch (dispIdMember) {
        case 2:
            {
                if (wFlags & 2) {
                    if (pDispParams->cArgs != 0) {
                        return DISP_E_BADPARAMCOUNT;
                    }
                    short i1;
                    hr = ((::ISimplest*) this)->get_X(&i1);
                    if (pVarResult != 0) {
                        V_VT(pVarResult) = VT_I2;
                        V_I2(pVarResult) = i1;
                    }
                    break;
                }
                else 
        if (wFlags & 4) {
                    if (pDispParams->cArgs != 1) {
                        return DISP_E_BADPARAMCOUNT;
                    }
                    v = rgpVars[0];
                    if (v->vt != VT_I2 && FAILED(__VariantChangeType(v, &v0, VT_I2))) {
                        if (puArgErr != 0) {
                            *puArgErr = 0;
                        }
                        return DISP_E_TYPEMISMATCH;
                    }
                    short i1 = V_I2(v);
                    hr = ((::ISimplest*) this)->put_X(i1);
                    break;
                }
            }
        case 3:
            {
                if (pDispParams->cArgs != 1) {
                    return DISP_E_BADPARAMCOUNT;
                }
                v = rgpVars[0];
                if (v->vt != VT_I2 && FAILED(__VariantChangeType(v, &v0, VT_I2))) {
                    if (puArgErr != 0) {
                        *puArgErr = 0;
                    }
                    return DISP_E_TYPEMISMATCH;
                }
                short i1 = V_I2(v);
                long i2;
                hr = ((::ISimplest*) this)->Square(i1, &i2);
                if (pVarResult != 0) {
                    V_VT(pVarResult) = VT_I4;
                    V_I4(pVarResult) = i2;
                }
                break;
            }
        default:
            return DISP_E_MEMBERNOTFOUND;
        }
        return hr;
    }
    virtual HRESULT STDMETHODCALLTYPE ISimplest::GetIDsOfNames(
                /* [in] */ REFIID riid,
                /* [size_is][in] */ LPOLESTR *rgszNames,
                /* [in] */ UINT cNames,
                /* [in] */ LCID lcid,
                /* [size_is][out] */ DISPID *rgDispId) 
    {
        (void) riid;
        (void) rgszNames;
        (void) cNames;
        (void) lcid;
        (void) rgDispId;
        static LPOLESTR names[] = { L"X", L"pVal", L"get_X", L"put_X", L"val", L"pResult", L"Square" };
        static DISPID dids[] = { 2, 0, 2, 2, 0, 1, 3 };
        for (unsigned int i = 0; i < cNames; ++i) {
            int fFoundIt = 0;
            for (unsigned int j = 0; j < sizeof(names)/sizeof(LPOLESTR); ++j) {
                if (_wcsicmp(rgszNames[i], names[j]) == 0) {
                    fFoundIt = 1;
                    rgDispId[i] = dids[j];
                    break;
                }
            }
            if (fFoundIt == 0) {
                return DISP_E_UNKNOWNNAME;
            }
        }
        return S_OK;
    }
    HRESULT TypeInfoHelper(REFIID iidDisp, LCID /*lcid*/, ITypeInfo** ppTypeInfo) 
    {
        if (ppTypeInfo == NULL) {
            return E_POINTER;
        }
        *ppTypeInfo = NULL;
        TCHAR szModule1[_MAX_PATH];
        int nLen = ::GetModuleFileName(_AtlBaseModule.GetModuleInstance(), szModule1, _MAX_PATH);
        if (nLen == 0 || nLen == _MAX_PATH) {
            return E_FAIL;
        }
        USES_CONVERSION_EX;
        LPOLESTR pszModule = T2OLE_EX(szModule1, _ATL_SAFE_ALLOCA_DEF_THRESHOLD);
#ifndef _UNICODE
        if (pszModule == NULL) {
            return E_OUTOFMEMORY;
        }
#endif
        CComPtr<ITypeLib> spTypeLib;
        HRESULT hr = LoadTypeLib(pszModule, &spTypeLib);
        if (SUCCEEDED(hr)) {
            CComPtr<ITypeInfo> spTypeInfo;
            hr = spTypeLib->GetTypeInfoOfGuid(iidDisp, &spTypeInfo);
            if (SUCCEEDED(hr)) {
                *ppTypeInfo = spTypeInfo.Detach();
            }
        }
        return hr;
    }
    virtual HRESULT STDMETHODCALLTYPE ISimplest::GetTypeInfoCount(unsigned int*  pctinfo) 
    {
        if (pctinfo == NULL) {
            return E_POINTER;
        }
        CComPtr<ITypeInfo> spTypeInfo;
        *pctinfo = 
                       (SUCCEEDED(TypeInfoHelper(__uuidof(ISimplest), 0, &spTypeInfo))) ? 1 : 0;
        return S_OK;
    }
    virtual HRESULT STDMETHODCALLTYPE ISimplest::GetTypeInfo(unsigned int iTInfo, LCID lcid, ITypeInfo** ppTInfo) 
    {
        if (iTInfo != 0) {
            return DISP_E_BADINDEX;
        }
        return TypeInfoHelper(__uuidof(ISimplest), lcid, ppTInfo);
    }
#injected_line 32 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simplest.h"
    static DWORD* GetOpCodes() 
    {
        static DWORD rgOps[] = 
            					{
            						0x70000000,
            0x30004000,
            0x80000002,
            0x50000000,
            0x3000c000,
            0x80000004,
            0x60000000,
            0x30014000,
            0x80000002,
            0x50000000,
            0x3000c000,
            0x80000004,
            0x30018000,
            0x80000001,
            0x60000000,
            0x1000c000,
            0x3000c000,
            0x50000000,
            0x20010000,
            0x30010000,
            0x80000002,
            0x50000000,
            0x3001c000,
            0x80000001,
            0x30020000,
            0x80000005,
            0x20024000,
            0x30024000,
            0x30028000,
            0x8000000b,
            0x50000000,
            0x8003000d,
            0x60000000,
            0x8003800f,
            0x30040000,
            0x80000011,
            0x60000000,
            0x60000000,
            0x60000000,
            0
            					};
        return rgOps;
    }
#injected_line 32 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simplest.h"
    static RGSDWORD* GetOpcodeDWORDVals() 
    {
        static RGSDWORD rgDWORDS[] = 
            					{
            						{0, 0}
            					};
        return rgDWORDS;
    }
#injected_line 32 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simplest.h"
    static RGSBinary* GetOpcodeBinaryVals() 
    {

        static RGSBinary rgBinary[] = 
            					{
            						{0, 0}
            					};
        return rgBinary;
    }
#injected_line 32 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simplest.h"
    static RGSStrings* GetOpcodeStringVals() 
    {
        static RGSStrings rgStrings[] = 
            					{
            						{_T(""),0 }, 
            {_T("SimpleObject.Simplest.1"),0 }, 
            {_T("%FriendlyName%"),1 }, 
            {_T("CLSID"),0 }, 
            {_T("{5BDCE1E1-D79D-41E1-9500-E4ED3E64887A}"),0 }, 
            {_T("SimpleObject.Simplest"),0 }, 
            {_T("CurVer"),0 }, 
            {_T("ProgID"),0 }, 
            {_T("VersionIndependentProgID"),0 }, 
            {_T("Programmable"),0 }, 
            {_T("%MODULETYPE%"),1 }, 
            {_T("%MODULE%"),1 }, 
            {_T("ThreadingModel"),0 }, 
            {_T("apartment"),0 }, 
            {_T("AppID"),0 }, 
            {_T("%APPID%"),1 }, 
            {_T("TypeLib"),0 }, 
            {_T("%MODULEGUID%"),1 }, 
            {NULL, 0}
            					};
        return rgStrings;
    }
#injected_line 32 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simplest.h"
    static HRESULT WINAPI UpdateRegistry(BOOL bRegister) 
    {
        CRegistryVirtualMachine rvm;
        HRESULT hr;
        if (FAILED(hr = rvm.AddStandardReplacements()))
            return hr;
        rvm.AddReplacement(_T("FriendlyName"), GetObjectFriendlyName());
        return rvm.VMUpdateRegistry(GetOpCodes(), GetOpcodeStringVals(), GetOpcodeDWORDVals(), GetOpcodeBinaryVals(), bRegister);
    }
#injected_line 32 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simplest.h"
    static const TCHAR* GetObjectFriendlyName() 
    {
        return _T("CSimplest Object");
    }
#injected_line 32 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simplest.h"
    static const TCHAR* GetProgID() 
    {
        return _T("SimpleObject.Simplest.1");
    }
#injected_line 32 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simplest.h"
    static const TCHAR* GetVersionIndependentProgID() 
    {
        return _T("SimpleObject.Simplest");
    }
#injected_line 32 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simplest.h"
    BEGIN_COM_MAP(CSimplest)
        COM_INTERFACE_ENTRY(ISimplest)
        COM_INTERFACE_ENTRY(IDispatch)
        COM_INTERFACE_ENTRY(IProvideClassInfo)
    END_COM_MAP()

	//--- End Injected Code For Attribute 'coclass'
};

//+++ Start Injected Code For Attribute 'coclass'
#injected_line 32 "c:\\writing\\ms press\\com book\\6-attributes\\simpleobject\\simplest.h"

				OBJECT_ENTRY_AUTO(__uuidof(CSimplest), CSimplest)
			
//--- End Injected Code For Attribute 'coclass'


