//======================================================================
// NoteDemo - Demonstrates the Windows CE Notification API
//
// Written for the book Programming Windows CE
// Copyright (C) 2003 Douglas Boling
//======================================================================
#include <windows.h>				 // For all that Windows stuff
#include <notify.h> 				 // For notification defines
#include "NoteDemo.h"				 // Program-specific stuff

//----------------------------------------------------------------------
// Global data
//
const TCHAR szAppName[] = TEXT ("NoteDemo");
HINSTANCE hInst;					 // Program instance handle
HWND g_hMain;

CE_USER_NOTIFICATION g_ceun;		 // User notification structure
TCHAR szDlgTitle[] = TEXT ("Notification Demo");
TCHAR szDlgText[] = TEXT ("Times Up!");
TCHAR szSound[MAX_PATH] = TEXT ("alarm1.wav");

// Used for timer event notification
TCHAR szEventName[] = TEXT ("Bob");
					   
HANDLE g_hNoteEvent = 0; 
BOOL g_fContinue = TRUE;

// Message dispatch table for MainWindowProc
const struct decodeUINT MainMessages[] = {
	WM_INITDIALOG, DoInitDialogMain,
	WM_COMMAND, DoCommandMain,
	MYMSG_TELLNOTIFY, DoTellNotifyMain,
};
// Command Message dispatch for MainWindowProc
const struct decodeCMD MainCommandItems[] = {
	IDOK, DoMainCommandExit,
	IDCANCEL, DoMainCommandExit,
	IDD_ADDUSERNOT, DoMainCommandAddUserNotification,
	IDD_CFGUSERNOT, DoMainCommandConfigUserNotification,
	IDD_ADDSYSNOT, DoMainCommandAddSysNotification, 
	IDD_ADDTIMENOT, DoMainCommandAddTimerNotification, 
	IDD_CLEARNOT, DoMainCommandClearNotifications, 
};
//======================================================================
// Program entry point
// 
int WINAPI WinMain (HINSTANCE hInstance, HINSTANCE hPrevInstance,
					LPWSTR lpCmdLine, int nCmdShow) {
	INT i;
	TCHAR szText[MAX_PATH];
	WCHAR *pPtr;
	HANDLE hNotify;
	HWND hWnd;
	HANDLE hThread;

	hInst = hInstance;

	if (*lpCmdLine) {
		pPtr = lpCmdLine;
		// Parse the first word of the command line.
		for (i = 0; (i < dim(szText)-1) && (*pPtr > TEXT (' ')); i++)
			szText[i] = *pPtr++;
		szText[i] = TEXT ('\0');

		// Check to see if app started due to notification.
		if (lstrcmp (szText, APP_RUN_TO_HANDLE_NOTIFICATION) == 0) {
			// Acknowledge the notification.
			GetModuleFileName (hInst, szText, sizeof (szText));
			CeHandleAppNotifications (szText);

			// Get handle of command line.
			hNotify = (HANDLE)_wtol (pPtr);

			// Look to see if another instance of the app is running.
			hWnd = FindWindow (NULL, szAppName);
			if (hWnd) {
				SendMessage (hWnd, MYMSG_TELLNOTIFY, 0, 
							 (LPARAM)hNotify);
				// I should terminate this app here, but I don't so you
				// can see what happens.
				return 0;
			}
		} 
	}
	// Do a little initialization of CE_USER_NOTIFICATION.
	memset (&g_ceun, 0, sizeof (g_ceun));
	g_ceun.ActionFlags = PUN_DIALOG;
	g_ceun.pwszDialogTitle = szDlgTitle;
	g_ceun.pwszDialogText = szDlgText;
	g_ceun.pwszSound = szSound;
	g_ceun.nMaxSound = sizeof (szSound);

	// Create secondary thread for timer event notification.
	g_hNoteEvent = CreateEvent (NULL, FALSE, FALSE, szEventName);
	hThread = CreateThread (NULL, 0, MonitorThread, hWnd, 0, (DWORD *)&i);
	if (hThread == 0)
		return -1;

	// Display dialog box as main window.
	DialogBoxParam (hInstance, szAppName, NULL, MainDlgProc, 
					(LPARAM)lpCmdLine);
	// Signal notification thread to terminate
	g_fContinue = FALSE;
	SetEvent (g_hNoteEvent);
	WaitForSingleObject (hThread, 1000);
	CloseHandle (hThread);
	CloseHandle (g_hNoteEvent);
	return 0;

}
//======================================================================
// Message handling procedures for main window
//----------------------------------------------------------------------
// MainDlgProc - Callback function for application window
//
BOOL CALLBACK MainDlgProc (HWND hWnd, UINT wMsg, WPARAM wParam, 
						   LPARAM lParam) {
	INT i;
	//
	// Search message list to see if we need to handle this
	// message. If in list, call procedure.
	//
	for (i = 0; i < dim(MainMessages); i++) {
		if (wMsg == MainMessages[i].Code)
			return (*MainMessages[i].Fxn)(hWnd, wMsg, wParam, lParam);
	}
	return FALSE;
}
//----------------------------------------------------------------------
// DoInitDialogMain - Process WM_INITDIALOG message for window.
//
BOOL DoInitDialogMain (HWND hWnd, UINT wMsg, WPARAM wParam,
					   LPARAM lParam) {

	g_hMain = hWnd;
	if (*(LPTSTR)lParam)
		Add2List (hWnd, (LPTSTR)lParam);
	return FALSE;
}
//----------------------------------------------------------------------
// DoCommandMain - Process WM_COMMAND message for window.
//
BOOL DoCommandMain (HWND hWnd, UINT wMsg, WPARAM wParam, LPARAM lParam){
	WORD idItem, wNotifyCode;
	HWND hwndCtl;
	INT  i;

	// Parse the parameters.
	idItem = (WORD) LOWORD (wParam);
	wNotifyCode = (WORD) HIWORD (wParam);
	hwndCtl = (HWND) lParam;

	// Call routine to handle control message.
	for (i = 0; i < dim(MainCommandItems); i++) {
		if (idItem == MainCommandItems[i].Code) {
			(*MainCommandItems[i].Fxn)(hWnd, idItem, hwndCtl, 
									   wNotifyCode);
			return TRUE;
		}
	}
	return FALSE;
}
//----------------------------------------------------------------------
// DoTellNotifyMain - Process MYMSG_TELLNOTIFY message for window.
//
BOOL DoTellNotifyMain (HWND hWnd, UINT wMsg, WPARAM wParam, 
					   LPARAM lParam) {
	Add2List (hWnd, TEXT ("Notification %d reported"), lParam);
	SetForegroundWindow ((HWND)((DWORD)hWnd | 0x01));
	return 0;
}
//======================================================================
// Command handler routines
//----------------------------------------------------------------------
// DoMainCommandExit - Process Program Exit command.
//
LPARAM DoMainCommandExit (HWND hWnd, WORD idItem, HWND hwndCtl, 
						  WORD wNotifyCode) {
	EndDialog (hWnd, 0);
	return 0;
}
//----------------------------------------------------------------------
// DoMainCommandAddUserNotification - Process Add User Notify button.
//
LPARAM DoMainCommandAddUserNotification (HWND hWnd, WORD idItem, 
										 HWND hwndCtl, WORD wNotifyCode) {
	SYSTEMTIME st, ste;
	TCHAR szExeName[MAX_PATH], szText[128];
	TCHAR szArgs[128] = TEXT("This is my user notification string.");
	CE_NOTIFICATION_TRIGGER nt;
	HANDLE hNotify;

	// Initialize time structure with local time.
	GetLocalTime (&st);
	// Do a trivial amount of error checking.
	st.wMinute++;
	if (st.wMinute > 59) {
		st.wHour++;
		st.wMinute -= 60;
	}

	// Set end time 10 minutes past start.
	ste = st;
	// Do a trivial amount of error checking.
	ste.wMinute += 10;
	if (ste.wMinute > 59) {
		ste.wHour++;
		ste.wMinute -= 60;
	} 

	memset (&nt, 0, sizeof (CE_NOTIFICATION_TRIGGER));
	nt.dwSize = sizeof (CE_NOTIFICATION_TRIGGER);
	nt.dwType = CNT_PERIOD;
	nt.lpszApplication = szExeName;
	nt.lpszArguments = szArgs;
	nt.stStartTime = st;
	nt.stEndTime = ste;
	GetModuleFileName (hInst, szExeName, sizeof (szExeName));

	hNotify = CeSetUserNotificationEx (0, &nt, &g_ceun);
	// Tell the user the notification was set.
	if (hNotify) 
		wsprintf (szText, TEXT ("User notification set for %d:%02d:%02d"),
				  st.wHour, st.wMinute, st.wSecond);
	else
		wsprintf (szText, TEXT ("User notification failed. rc = %d"),
				  GetLastError());

	MessageBox (hWnd, szText, szAppName, MB_OK);
	return 0;
}
//----------------------------------------------------------------------
// DoMainCommandConfigUserNotification - Process Config user
// notification button.
//
LPARAM DoMainCommandConfigUserNotification (HWND hWnd, WORD idItem, 
											HWND hwndCtl, WORD wNotifyCode) {

	// Display the system-provided configuration dialog.
	CeGetUserNotificationPreferences (hWnd, &g_ceun);
	return 0;
}
//----------------------------------------------------------------------
//	DoMainCommandAddSysNotification - Process Add Sys notify button.
//
LPARAM DoMainCommandAddSysNotification (HWND hWnd, WORD idItem, 
										HWND hwndCtl, WORD wNotifyCode) {

	DialogBox (hInst, TEXT ("SysNotifyConfig"), hWnd, 
			   SetEventNotifyDlgProc);
	return 0;
}
//----------------------------------------------------------------------
// DoMainCommandAddTimerNotification - Process add timer notify button.
//
LPARAM DoMainCommandAddTimerNotification (HWND hWnd, WORD idItem, 
										  HWND hwndCtl, WORD wNotifyCode) {
	SYSTEMTIME st;
	HANDLE hNotify;
	CE_NOTIFICATION_TRIGGER nt;
	TCHAR szExeName[MAX_PATH], szText[128];
	TCHAR szArgs[128] = TEXT("This is my timer notification string.");

	// Initialize time structure with local time.
	GetLocalTime (&st);
	// Do a trivial amount of error checking.
	if (st.wMinute == 59) {
		st.wHour++;
		st.wMinute = 0;
	} else
		st.wMinute++;

	memset (&nt, 0, sizeof (CE_NOTIFICATION_TRIGGER));
	nt.dwSize = sizeof (CE_NOTIFICATION_TRIGGER);
	nt.dwType = CNT_TIME;
	nt.lpszApplication = szExeName;
	nt.lpszArguments = szArgs;
	nt.stStartTime = st;

	lstrcpy (szExeName, NAMED_EVENT_PREFIX_TEXT);
	lstrcat (szExeName, szEventName);
	// Set the notification.
	hNotify = CeSetUserNotificationEx (0, &nt, NULL);
	if (hNotify)  
		wsprintf (szText, TEXT ("Timer notification set for %d:%02d:%02d"),
				  st.wHour, st.wMinute, st.wSecond);
	else
		wsprintf (szText, TEXT ("Timer notification failed. rc = %d"),
				  GetLastError());
	MessageBox (hWnd, szText, szAppName, MB_OK);
	return 0;
}
//----------------------------------------------------------------------
// DoMainCommandClearNotifications - Clear all notifications pointing
// to this application.  Note: this is a fairly large stack frame.
//
LPARAM DoMainCommandClearNotifications (HWND hWnd, WORD idItem, 
										HWND hwndCtl, WORD wNotifyCode) {
	PBYTE pBuff = NULL;
	PCE_NOTIFICATION_INFO_HEADER pnih;
	HANDLE hNotHandles[128];  // Assume this is large enough.
	int rc, nCnt = 0;
	TCHAR szExeName[MAX_PATH], szText[128];
	DWORD i, dwSize, nHandCnt = 0;

	// Get our filename.
	GetModuleFileName (hInst, szExeName, sizeof (szExeName));

	pBuff = (PBYTE)LocalAlloc (LPTR, 8192);
	if (!pBuff) {
		MessageBox (hWnd, TEXT ("Out of memory"), szAppName, MB_OK);
		return 0;
	}
	rc = CeGetUserNotificationHandles (hNotHandles, dim (hNotHandles), 
									   &nHandCnt);
	if (rc) {
		for (i = 0; i < nHandCnt; i++) {
			// Query info on a single handle.
			rc = CeGetUserNotification (hNotHandles[i], 8192, 
										&dwSize, pBuff);
			if (rc) {
				pnih = (PCE_NOTIFICATION_INFO_HEADER)pBuff;
				if (!lstrcmp (pnih->pcent->lpszApplication, szExeName)){
					if (CeClearUserNotification (pnih->hNotification))
						nCnt++;
				}
			}
		}
		wsprintf (szText, TEXT ("Cleared %d notifications"), nCnt);
		MessageBox (hWnd, szText, szAppName, MB_OK);
	} else
	   MessageBox (hWnd, TEXT ("Could not query handles"), 
					szAppName, MB_OK);
	LocalFree (pBuff);
	return 0;
}
//----------------------------------------------------------------------
// MySetEventNotification  - Sets event notifications
//
int MySetEventNotification (HWND hWnd, DWORD dwEvent) {
	TCHAR szArgs[] = TEXT("This is my event notification string.");
	CE_NOTIFICATION_TRIGGER nt;
	HANDLE hNotify;
	TCHAR szExeName[MAX_PATH], szText[128];

	memset (&nt, 0, sizeof (CE_NOTIFICATION_TRIGGER));
	nt.dwSize = sizeof (CE_NOTIFICATION_TRIGGER);
	nt.dwType = CNT_EVENT;
	nt.dwEvent = dwEvent;
	nt.lpszApplication = szExeName;
	nt.lpszArguments = szArgs;
	GetModuleFileName (hInst, szExeName, sizeof (szExeName));

	// Set the notification.
	hNotify = CeSetUserNotificationEx (0, &nt, NULL);
	if (hNotify)  
		wsprintf (szText, TEXT ("Event notification set for %08x"), 
				  dwEvent);
	else
		wsprintf (szText, TEXT("Set Event notification failed rc: %d"), 
				  GetLastError());
	MessageBox (hWnd, szText, szAppName, MB_OK);
	return 0;
}
//----------------------------------------------------------------------
// Add2List - Add string to the report list box.
//
void Add2List (HWND hWnd, LPTSTR lpszFormat, ...) {
	int i, nBuf;
	TCHAR szBuffer[512];

	va_list args;
	va_start(args, lpszFormat);

	nBuf = _vstprintf(szBuffer, lpszFormat, args);
	i = SendDlgItemMessage (hWnd, IDD_OUTPUT, WM_SETTEXT, 0, 
							(LPARAM)(LPCTSTR)szBuffer);
	va_end(args);
}

//======================================================================
// SetEventNotifyDlgProc - Callback function for Event dialog box
//
BOOL CALLBACK SetEventNotifyDlgProc (HWND hWnd, UINT wMsg,
									 WPARAM wParam, LPARAM lParam) {
	DWORD dwEvent;

	switch (wMsg) {
	case WM_COMMAND:
		{
			WORD idItem = LOWORD (wParam);
			switch (idItem) {
			case IDOK:
				dwEvent = 0;

				// IsDlgButtonChecked isn't defined in Win CE, so 
				// a macro has been defined.
				if (IsDlgButtonChecked (hWnd, IDC_SYNC_END) == 1)
					dwEvent |= NOTIFICATION_EVENT_SYNC_END;

				if (IsDlgButtonChecked (hWnd, IDC_SERIAL_DETECT) == 1)
					dwEvent |= NOTIFICATION_EVENT_RS232_DETECTED;

				if (IsDlgButtonChecked (hWnd, IDC_DEVICE_CHANGE) == 1)
					dwEvent |= NOTIFICATION_EVENT_DEVICE_CHANGE;

				if (IsDlgButtonChecked (hWnd, IDC_TIME_CHANGE) == 1)
					dwEvent |= NOTIFICATION_EVENT_TIME_CHANGE;

				if (IsDlgButtonChecked (hWnd, IDC_RESTORE_END) == 1)
					dwEvent |= NOTIFICATION_EVENT_RESTORE_END;

				if (IsDlgButtonChecked (hWnd, IDC_POWER_UP) == 1)
					dwEvent |= NOTIFICATION_EVENT_WAKEUP;

				if (IsDlgButtonChecked (hWnd, IDC_TZ_CHANGE) == 1)
					dwEvent |= NOTIFICATION_EVENT_TZ_CHANGE;

				// Call my set event notification function above.
				MySetEventNotification (hWnd, dwEvent);
				EndDialog (hWnd, 1); 
				return TRUE;

			case IDCANCEL:
				EndDialog (hWnd, 0); 
				return TRUE;
			}
		}
		break;
	}
	return FALSE;
}
//======================================================================
// MonitorThread - Monitors event for timer notificaiton 
//
DWORD WINAPI MonitorThread (PVOID pArg) {
	int rc;

	while (g_fContinue) {
		rc = WaitForSingleObject (g_hNoteEvent, INFINITE);
		if (!g_fContinue)
			break;
		if (rc == WAIT_OBJECT_0) 
			SendMessage (g_hMain, MYMSG_TELLNOTIFY, 0, (LPARAM)g_hNoteEvent);
		else 
			break;
	}
	return 0;
}
