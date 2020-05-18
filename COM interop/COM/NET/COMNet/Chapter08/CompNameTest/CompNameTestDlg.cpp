// CompNameTestDlg.cpp : implementation file
//

#include "stdafx.h"
#include "CompNameTest.h"
#include "CompNameTestDlg.h"
#include ".\compnametestdlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CCompNameTestDlg dialog



CCompNameTestDlg::CCompNameTestDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CCompNameTestDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CCompNameTestDlg::DoDataExchange(CDataExchange* pDX)
{
   CDialog::DoDataExchange(pDX);
   DDX_Control(pDX, IDC_SELECT, m_Select);
   DDX_Control(pDX, IDC_OUTPUT, m_Output);
}

BEGIN_MESSAGE_MAP(CCompNameTestDlg, CDialog)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
   ON_BN_CLICKED(IDC_SINGLE, OnBnClickedSingle)
   ON_BN_CLICKED(IDC_ALL, OnBnClickedAll)
END_MESSAGE_MAP()


// CCompNameTestDlg message handlers

BOOL CCompNameTestDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// Set the initial name type selection value.
   m_Select.SetCurSel(0);
	
	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CCompNameTestDlg::OnPaint() 
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
HCURSOR CCompNameTestDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CCompNameTestDlg::OnBnClickedSingle()
{
   IComputerName        *pComputerName;   // Component interface pointer.
   _bstr_t              OutputData;       // Output from component.
   COMPUTER_NAME_FORMAT NameType;         // Name Type Enumeration.

   // Initialize the COM environment.
   CoInitialize(NULL);

   // Create the object.
   CoCreateInstance(CLSID_ComputerName, 
                    NULL, 
                    CLSCTX_ALL, 
                    IID_IComputerName,
                    (void**)&pComputerName);

   // Determine which name the user has selected.
   switch (m_Select.GetCurSel())
   {
   case 0:
      NameType = COMPUTER_NAME_FORMAT_ComputerNameNetBIOS;
      break;
   case 1:
      NameType = COMPUTER_NAME_FORMAT_ComputerNameDnsHostname;
      break;
   case 2:
      NameType = COMPUTER_NAME_FORMAT_ComputerNameDnsDomain;
      break;
   case 3:
      NameType = COMPUTER_NAME_FORMAT_ComputerNameDnsFullyQualified;
      break;
   case 4:
      NameType = COMPUTER_NAME_FORMAT_ComputerNamePhysicalNetBIOS;
      break;
   case 5:
      NameType = COMPUTER_NAME_FORMAT_ComputerNamePhysicalDnsHostname;
      break;
   case 6:
      NameType = COMPUTER_NAME_FORMAT_ComputerNamePhysicalDnsDomain;
      break;
   case 7:
      NameType = COMPUTER_NAME_FORMAT_ComputerNamePhysicalDnsFullyQualified;
      break;
   case 8:
      NameType = COMPUTER_NAME_FORMAT_ComputerNameMax;
   }

    // Get the computer names.
   OutputData = pComputerName->GetSingleName(NameType);

   // Display the output on screen.
   m_Output.SetWindowText(OutputData);

   // Uninitialize the COM environment.
   CoUninitialize();
}

void CCompNameTestDlg::OnBnClickedAll()
{
   IComputerName  *pComputerName;   // Component interface pointer.
   _bstr_t        OutputData;       // Output from component.

   // Initialize the COM environment.
   CoInitialize(NULL);

   // Create the object.
   CoCreateInstance(CLSID_ComputerName, 
                    NULL, 
                    CLSCTX_ALL, 
                    IID_IComputerName,
                    (void**)&pComputerName);

   // Get the computer names.
   OutputData = pComputerName->GetAllNames();

   // Display the output on screen.
   m_Output.SetWindowText(OutputData);

   // Uninitialize the COM environment.
   CoUninitialize();
}
