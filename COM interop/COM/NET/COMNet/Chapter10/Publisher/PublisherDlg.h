// PublisherDlg.h : header file
//
// Import the type library and use the associated namespace.
#include "afxwin.h"
#import "SimpleEventObject.TLB" named_guids
using namespace SimpleEventObject;

#pragma once


// CPublisherDlg dialog
class CPublisherDlg : public CDialog
{
// Construction
public:
	CPublisherDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_PUBLISHER_DIALOG };

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
   // Holds the message to transmit.
   CString strMsg;
   afx_msg void OnBnClickedPublish();
   // The data we want to send to the subscriber.
   CEdit MsgText;
};
