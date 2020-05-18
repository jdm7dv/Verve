// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the TESTINTEROP_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// TESTINTEROP_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef TESTINTEROP_EXPORTS
#define TESTINTEROP_API __declspec(dllexport)
#else
#define TESTINTEROP_API __declspec(dllimport)
#endif


typedef struct tagPerson
{
   char* firstName;
   char* lastName;
   int age;
} PERSON, *LPPERSON;

typedef struct tagAccount
{
   PERSON* accountHolder;
   char* accountName;
   long accountNumber;
   double balance;
} ACCOUNT, *LPACCOUNT;

TESTINTEROP_API char* GetAccountDetails(LPACCOUNT acct);

typedef struct tagPoint
{
  int x,y;
} PNT, *LPPNT;

typedef struct tagRect
{
  PNT p1,p2;
} RCT, *LPRCT;

TESTINTEROP_API int GetArea(LPRCT pRect);
