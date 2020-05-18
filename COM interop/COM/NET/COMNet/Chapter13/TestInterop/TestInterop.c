// TestInterop.cpp : Defines the entry point for the DLL application.
//
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <stdlib.h>

#include "TestInterop.h"
BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
    return TRUE;
}


// Return details for an account, in the form "AccountName (Holder)"
TESTINTEROP_API char* GetAccountDetails(LPACCOUNT acct)
{
   char* buff = (char*) malloc(strlen(acct->accountName) + 
      strlen(acct->accountHolder->firstName) + 
      strlen(acct->accountHolder->lastName) + 
      4);   // terminating null plus two brackets and two spaces

   strcpy(buff, acct->accountName);
   strcat(buff, " (");
   strcat(buff, acct->accountHolder->firstName);
   strcat(buff, " ");
   strcat(buff, acct->accountHolder->lastName);
   strcat(buff,")");

	return buff;
}

TESTINTEROP_API int GetArea(LPRCT pRect)
{
   int width = (pRect->p2.x > pRect->p1.x) ? pRect->p2.x - pRect->p1.x : pRect->p1.x - pRect->p2.x;
   int height = (pRect->p2.y > pRect->p1.y) ? pRect->p2.y - pRect->p1.y : pRect->p1.y - pRect->p2.y;

   return height * width;
}
