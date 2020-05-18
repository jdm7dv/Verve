// PublisherDlg.cpp : implementation file
//

#include "stdafx.h"
#include "Publisher.h"
#include "PublisherDlg.h"
#include ".\publisherdlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CPublisherDlg dialog



CPublisherDlg::CPublisherDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CPublisherDlg::IDD, pParent)
   , strMsg(_T(""))
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CPublisherDlg::DoDataExchange(CDataExchange* pDX)
{
   CDialog::DoDataExchange(pDX);
   DDX_Text(pDX, IDC_MSG, strMsg);
   DDX_Control(pDX, IDC_MSG, MsgText);
}

BEGIN_MESSAGE_MAP(CPublisherDlg, CDialog)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
   ON_BN_CLICKED(IDC_PUBLISH, OnBnClickedPublish)
END_MESSAGE_MAP()


// CPublisherDlg message handlers

BOOL CPublisherDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here
	
	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CPublisherDlg::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CPublisherDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CPublisherDlg::OnBnClickedPublish()
{
   ISendMsg *pSendMsg;  // Pointer to ConstString object.
   _bstr_t  Output;     // Temporary data string.
   CString  Data;       // Data from the message box.

   // Initialize the COM environment.
   CoInitialize(NULL);

   // Create the object.
   CoCreateInstance(CLSID_SendMsg, 
                    NULL, 
                    CLSCTX_ALL, 
                    IID_ISendMsg,
                    (void**)&pSendMsg);

   // Send the message.
   MsgText.GetWindowText(Data);
   Output = Data;
   pSendMsg->FireBroadcastMsg(Output);

   // Uninitialize the COM environment.
   CoUninitialize();
}
