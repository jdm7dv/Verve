// CompNameTestDlg.h : header file
//

#pragma once
#include "afxwin.h"

// Import the type library and use the associated namespace.
#import "CompName.TLB" named_guids
using namespace CompName;

// CCompNameTestDlg dialog
class CCompNameTestDlg : public CDialog
{
// Construction
public:
	CCompNameTestDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_COMPNAMETEST_DIALOG };

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
   // User type name selection.
   CComboBox m_Select;
   // Output data received from component.
   CEdit m_Output;
   // Button click event handlers.
   afx_msg void OnBnClickedSingle();
   afx_msg void OnBnClickedAll();
};
