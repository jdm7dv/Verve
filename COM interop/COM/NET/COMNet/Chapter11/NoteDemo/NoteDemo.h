//======================================================================
// Header file
//
// Written for the book Programming Windows CE
// Copyright (C) 2003 Douglas Boling
//======================================================================
// Returns number of elements
#define dim(x) (sizeof(x) / sizeof(x[0])) 

//----------------------------------------------------------------------
// Generic defines and data types
//
struct decodeUINT {                            // Structure associates
    UINT Code;                                 // messages 
                                               // with a function. 
    BOOL (*Fxn)(HWND, UINT, WPARAM, LPARAM);
}; 
struct decodeCMD {                             // Structure associates
    UINT Code;                                 // menu IDs with a 
    LRESULT (*Fxn)(HWND, WORD, HWND, WORD);    // function.
};

// Define function not supported under Windows CE.
#ifndef IsDlgButtonChecked
#define IsDlgButtonChecked(a, b)\
                           SendDlgItemMessage (a, b, BM_GETCHECK, 0, 0)
#endif
//----------------------------------------------------------------------
// Generic defines used by application

#define  ID_ICON             1   

#define  IDD_ADDUSERNOT      10                // Control IDs
#define  IDD_CFGUSERNOT      11
#define  IDD_ADDSYSNOT       12
#define  IDD_ADDTIMENOT      13
#define  IDD_OUTPUT          14
#define  IDD_CLEARNOT        15

#define  IDC_SYNC_END        20
#define  IDC_DEVICE_CHANGE   21
#define  IDC_SERIAL_DETECT   22
#define  IDC_TIME_CHANGE     23
#define  IDC_RESTORE_END     24
#define  IDC_POWER_UP        25 
#define  IDC_TZ_CHANGE       26

#define MYMSG_TELLNOTIFY     (WM_USER + 100)

//----------------------------------------------------------------------
// Function prototypes
//
void Add2List (HWND hWnd, LPTSTR lpszFormat, ...);
// Window procedures
BOOL CALLBACK MainDlgProc (HWND, UINT, WPARAM, LPARAM);
BOOL CALLBACK SetEventNotifyDlgProc (HWND, UINT, WPARAM, LPARAM);

// Message handlers
BOOL DoInitDialogMain (HWND, UINT, WPARAM, LPARAM);
BOOL DoCommandMain (HWND, UINT, WPARAM, LPARAM);
BOOL DoTellNotifyMain (HWND, UINT, WPARAM, LPARAM);

// Command functions
LPARAM DoMainCommandExit (HWND, WORD, HWND, WORD);
LPARAM DoMainCommandAddUserNotification (HWND, WORD, HWND, WORD);
LPARAM DoMainCommandConfigUserNotification (HWND, WORD, HWND, WORD);
LPARAM DoMainCommandAddSysNotification (HWND, WORD, HWND, WORD); 
LPARAM DoMainCommandAddTimerNotification (HWND, WORD, HWND, WORD); 
LPARAM DoMainCommandClearNotifications (HWND, WORD, HWND, WORD);
// Thread prototype
DWORD WINAPI MonitorThread (PVOID pArg);
