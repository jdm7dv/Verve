#define _WIN32_DCOM
#include <iostream>
using namespace std;

// Include definitions of basic Windows types
#include <wtypes.h>

// Import the type library
#import "littlestring.tlb" no_namespace named_guids

int main()
{
	CoInitializeEx(0, COINIT_MULTITHREADED);

	//IUnknown* pUnk;
	//HRESULT hr = CoCreateInstance(CLSID_LittleString, NULL, CLSCTX_INPROC_SERVER, 
	//	IID_IUnknown, (void**)&pUnk);
	//if (FAILED(hr))
	//{
	//	cout << "CCI failed: " << hex << hr << dec << endl;
	//	return -1;
	//}

	try
	{
		IStringPtr sp(__uuidof(LittleString));
		cout << "Created object" << endl;

		sp->PutText("hello");
		_bstr_t bs = sp->ToUpper();

		cout << "Text is " << bs << endl;
	}
	catch(_com_error& ce)
	{
		cout << "com error: " << ce.ErrorMessage() << endl;
	}

	CoUninitialize();

	return 0;
}