// ConstructStringTestDlg.h : header file
//

#pragma once
#include "afxwin.h"

// Import the type library and use the associated namespace.
#import "ConstructString.TLB" named_guids
using namespace ConstructString;


// CConstructStringTestDlg dialog
class CConstructStringTestDlg : public CDialog
{
// Construction
public:
	CConstructStringTestDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_CONSTRUCTSTRINGTEST_DIALOG };

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
   // Constructor String Output Value
   CEdit m_Output;
   // Test button event handler.
   afx_msg void OnBnClickedTest();
};
