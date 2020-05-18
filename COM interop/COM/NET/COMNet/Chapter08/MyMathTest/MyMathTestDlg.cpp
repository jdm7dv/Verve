// MyMathTestDlg.cpp : implementation file
//

#include "stdafx.h"
#include "MyMathTest.h"
#include "MyMathTestDlg.h"
#include ".\mymathtestdlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CMyMathTestDlg dialog



CMyMathTestDlg::CMyMathTestDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CMyMathTestDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CMyMathTestDlg::DoDataExchange(CDataExchange* pDX)
{
    CDialog::DoDataExchange(pDX);
    DDX_Control(pDX, IDC_INPUT1, m_Input1);
    DDX_Control(pDX, IDC_INPUT2, m_Input2);
    DDX_Control(pDX, IDC_OUTPUT, m_Output);
}

BEGIN_MESSAGE_MAP(CMyMathTestDlg, CDialog)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	ON_BN_CLICKED(IDC_ADD, OnBnClickedAdd)
    ON_BN_CLICKED(IDC_SUBTRACT, OnBnClickedSubtract)
    ON_BN_CLICKED(IDC_MULTIPLY, OnBnClickedMultiply)
    ON_BN_CLICKED(IDC_DIVIDE, OnBnClickedDivide)
END_MESSAGE_MAP()


// CMyMathTestDlg message handlers

BOOL CMyMathTestDlg::OnInitDialog()
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

void CMyMathTestDlg::OnPaint() 
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
HCURSOR CMyMathTestDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CMyMathTestDlg::OnBnClickedAdd()
{
   IMathFunctions   *pMath;     // Pointer to math object.
   CString          TempString; // Temporary data string.
   long             Input1;     // Input 1 value.
   long             Input2;     // Input 2 value.
   long             Output;     // Output value.

   // Initialize the COM environment.
   CoInitialize(NULL);

   // Create the object.
   CoCreateInstance(CLSID_MathFunctions, 
                    NULL, 
                    CLSCTX_ALL, 
                    IID_IMathFunctions,
                    (void**)&pMath);

   // Get the input values.
   m_Input1.GetWindowText(TempString);
   Input1 = atoi(TempString.GetBuffer());
   TempString.ReleaseBuffer(-1);
   m_Input2.GetWindowText(TempString);
   Input2 = atoi(TempString.GetBuffer());
   TempString.ReleaseBuffer(-1);

   // Perform the operation.
   Output = pMath->DoAdd(Input1, Input2);

   // Display the result.
   itoa(Output, TempString.GetBuffer(10), 10);
   TempString.ReleaseBuffer(-1);
   m_Output.SetWindowText(TempString);

   // Uninitialize the COM environment.
   CoUninitialize();
}

void CMyMathTestDlg::OnBnClickedSubtract()
{
   IMathFunctions   *pMath;     // Pointer to math object.
   CString          TempString; // Temporary data string.
   long             Input1;     // Input 1 value.
   long             Input2;     // Input 2 value.
   long             Output;     // Output value.

   // Initialize the COM environment.
   CoInitialize(NULL);

   // Create the object.
   CoCreateInstance(CLSID_MathFunctions, 
                    NULL, 
                    CLSCTX_ALL, 
                    IID_IMathFunctions,
                    (void**)&pMath);

   // Get the input values.
   m_Input1.GetWindowText(TempString);
   Input1 = atoi(TempString.GetBuffer());
   TempString.ReleaseBuffer(-1);
   m_Input2.GetWindowText(TempString);
   Input2 = atoi(TempString.GetBuffer());
   TempString.ReleaseBuffer(-1);

   // Perform the operation.
   Output = pMath->DoSubtract(Input1, Input2);

   // Display the result.
   itoa(Output, TempString.GetBuffer(10), 10);
   TempString.ReleaseBuffer(-1);
   m_Output.SetWindowText(TempString);

   // Uninitialize the COM environment.
   CoUninitialize();
}

void CMyMathTestDlg::OnBnClickedMultiply()
{
   IMathFunctions   *pMath;     // Pointer to math object.
   CString          TempString; // Temporary data string.
   long             Input1;     // Input 1 value.
   long             Input2;     // Input 2 value.
   long             Output;     // Output value.

   // Initialize the COM environment.
   CoInitialize(NULL);

   // Create the object.
   CoCreateInstance(CLSID_MathFunctions, 
                    NULL, 
                    CLSCTX_ALL, 
                    IID_IMathFunctions,
                    (void**)&pMath);

   // Get the input values.
   m_Input1.GetWindowText(TempString);
   Input1 = atoi(TempString.GetBuffer());
   TempString.ReleaseBuffer(-1);
   m_Input2.GetWindowText(TempString);
   Input2 = atoi(TempString.GetBuffer());
   TempString.ReleaseBuffer(-1);

   // Perform the operation.
   Output = pMath->DoMultiply(Input1, Input2);

   // Display the result.
   itoa(Output, TempString.GetBuffer(10), 10);
   TempString.ReleaseBuffer(-1);
   m_Output.SetWindowText(TempString);

   // Uninitialize the COM environment.
   CoUninitialize();
}

void CMyMathTestDlg::OnBnClickedDivide()
{
   IMathFunctions   *pMath;     // Pointer to math object.
   CString          TempString; // Temporary data string.
   long             Input1;     // Input 1 value.
   long             Input2;     // Input 2 value.
   long             Output;     // Output value.

   // Initialize the COM environment.
   CoInitialize(NULL);

   // Create the object.
   CoCreateInstance(CLSID_MathFunctions, 
                    NULL, 
                    CLSCTX_ALL, 
                    IID_IMathFunctions,
                    (void**)&pMath);

   // Get the input values.
   m_Input1.GetWindowText(TempString);
   Input1 = atoi(TempString.GetBuffer());
   TempString.ReleaseBuffer(-1);
   m_Input2.GetWindowText(TempString);
   Input2 = atoi(TempString.GetBuffer());
   TempString.ReleaseBuffer(-1);

   // Perform the operation.
   Output = pMath->DoDivide(Input1, Input2);

   // Display the result.
   itoa(Output, TempString.GetBuffer(10), 10);
   TempString.ReleaseBuffer(-1);
   m_Output.SetWindowText(TempString);

   // Uninitialize the COM environment.
   CoUninitialize();
}
