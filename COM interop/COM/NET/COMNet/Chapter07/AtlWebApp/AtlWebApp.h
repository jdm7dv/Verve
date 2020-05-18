// AtlWebApp.h : Defines the ATL Server request handler class
//
#pragma once
#include <ctype.h>     // for isdigit
#include <stdlib.h>    // for atoi

// Structure to hold an array of shorts and the size of the array
typedef struct tagSumArgs
{
   size_t nVals;
   short* pVals;
} SumArgs;


[ request_handler("Default") ]
class CAtlWebAppHandler
{
private:
   // Call this function to return a simple error response to the user.
   // The HTTP status code defaults to 500 (a generic server error).
   HTTP_CODE SendError(const CStringA& strError, WORD wHttpStatus = 500)
   {
      // Clear any buffered headers (including cookies) and content.
      m_HttpResponse.ClearResponse();

      // Suggest that clients and proxies do not cache this response.
      m_HttpResponse.SetCacheControl("no-cache");
      m_HttpResponse.SetExpires(0);

      // Set the status code in the response object.
      m_HttpResponse.SetStatusCode(wHttpStatus);

      // Build the body of the response.
      m_HttpResponse << 
         "<html><head><title>ATL Server Tutorial</title></head><body><p>"
         << strError << "</p></body></html>";

      // Return a HTTP_CODE that tells the ATL Server code to
      // discontinue processing of the SRF file.
      return AtlsHttpError(wHttpStatus, SUBERR_NO_PROCESS);
   }

protected:
	// Put protected members here

public:
	// Put public members here

	HTTP_CODE ValidateAndExchange()
	{
		// TODO: Put all initialization and validation code here
		
		// Set the content-type
		m_HttpResponse.SetContentType("text/html");
		
		return HTTP_SUCCESS;
	}
 
protected:
	// Here is an example of how to use a replacement tag with the stencil processor
	[ tag_name(name="Hello") ]
	HTTP_CODE OnHello(void)
	{
		m_HttpResponse << "Hello World!";
		return HTTP_SUCCESS;
	}

   // A method that uses built-in parsing
	[ tag_name(name="Square") ]
	HTTP_CODE OnSquare(short* pVal)
	{
      long result = (*pVal) * (*pVal);
		m_HttpResponse << "Square of " << (*pVal) << " is " << result;
		return HTTP_SUCCESS;
	}

   //--------------------------------------------------------------------------------------

	HTTP_CODE ParseEchoData(IAtlMemMgr *pMemMgr, LPCSTR szParams, char **ppDest)
	{
      // If any of the parameters are null, fail
		if (pMemMgr == 0 || szParams == 0 || ppDest == 0)
		{
			return HTTP_FAIL;
		}
		
		// Allocate memory in the memoy manager, so that it may be automatically freed later
		*ppDest	=	(char*)pMemMgr->Allocate(strlen(szParams)+1);

		if (*ppDest == 0)
			return HTTP_FAIL;

      // Copy the string over...
		strcpy(*ppDest, szParams);

      return HTTP_SUCCESS;
   }

	[tag_name( name = "Echo", parse_func = "ParseEchoData")]
	HTTP_CODE OnEcho(char *pStr)
	{
		if (pStr == 0)
			return HTTP_FAIL;

		m_HttpResponse	<<	"Arg string is '"	<<	pStr << "'";

		return HTTP_SUCCESS;
	}
	
   //--------------------------------------------------------------------------------------

   // Parsing function to parse one short value
	HTTP_CODE ParseSquareData(IAtlMemMgr *pMemMgr, LPCSTR szParams, short **ppDest)
	{
      // If any of the parameters are null, fail
		if (pMemMgr == 0 || szParams == 0 || ppDest == 0)
		{
			return HTTP_FAIL;
		}

      // Very simpleminded check...
      for (int i=0; i<strlen(szParams); i++)
      {
         bool bOK = false;
         switch(szParams[i])
         {
         case '-':
            if (i==0) bOK = true;
            else bOK = false;
            break;
         case '+':
            if (i==0) bOK = true;
            else bOK = false;
            break;
         case '0': case '1': case '2': case '3':
         case '4': case '5': case '6': case '7':
         case '8': case '9':
            bOK = true;
            break;
         default:
            bOK = false;
            break;
         }

         if (bOK == false)
         {
            // Causes 500 - server error
            //return AtlsHttpError(500, ISE_SUBERR_STENCIL_PARSE_FAIL);
            return SendError("Invalid parameter", 400);            
         }
      }

      // Allocate memory for a short
      *ppDest = (short*)pMemMgr->Allocate(sizeof(short));

      // Convert using atoi, because we know the parameter is OK
      **ppDest = atoi(szParams);

      return HTTP_SUCCESS;
   }

   // A method that uses custom parsing
	[ tag_name(name="PSquare", parse_func="ParseSquareData") ]
	HTTP_CODE OnSquare2(short* pVal)
	{
      long result = (*pVal) * (*pVal);
		m_HttpResponse << "(P)Square of " << (*pVal) << " is " << result;
		return HTTP_SUCCESS;
	}


   // Parsing function to parse an array
	HTTP_CODE ParseSumData(IAtlMemMgr *pMemMgr, LPCSTR szParams, SumArgs **ppDest)
	{
      // If any of the parameters are null, fail
		if (pMemMgr == 0 || szParams == 0 || ppDest == 0)
		{
			return HTTP_FAIL;
		}

      // Allocate memory for the struct
      *ppDest = (SumArgs*)pMemMgr->Allocate(sizeof(SumArgs));

      return HTTP_SUCCESS;
   }

   // A method that uses custom parsing to sum data
	[ tag_name(name="Sum", parse_func="ParseSumData") ]
	HTTP_CODE OnSum(SumArgs* pArgs)
	{
		return HTTP_SUCCESS;
	}
}; // class CAtlWebAppHandler
