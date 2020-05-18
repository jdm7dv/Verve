// MyMathTestDlg.h : header file
//

#pragma once
#include "afxwin.h"

// Import the type library and use the associated namespace.
#import "MyMath.TLB" named_guids
using namespace MyMath;

// CMyMathTestDlg dialog
class CMyMathTestDlg : public CDialog
{
// Construction
public:
	CMyMathTestDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_MYMATHTEST_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
    // The four button event handlers.
	afx_msg void OnBnClickedAdd();
    afx_msg void OnBnClickedSubtract();
    afx_msg void OnBnClickedMultiply();
    afx_msg void OnBnClickedDivide();
    // Contains the first input value object.
    CEdit m_Input1;
    // Contains the second input value object.
    CEdit m_Input2;
    // Contains the output of the math operation.
    CEdit m_Output;
};
