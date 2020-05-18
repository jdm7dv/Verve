// Simplest.cpp : Implementation of CSimplest

#include "stdafx.h"
#include "Simplest.h"


// CSimplest


STDMETHODIMP CSimplest::get_X(short* pVal)
{
	*pVal = n;
	return S_OK;
}

STDMETHODIMP CSimplest::put_X(short newVal)
{
	n = newVal;
	return S_OK;
}

STDMETHODIMP CSimplest::Square(SHORT val, LONG* pResult)
{
	*pResult = val*val;
	return S_OK;
}
