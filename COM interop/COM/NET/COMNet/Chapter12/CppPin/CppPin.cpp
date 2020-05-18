// This is the main project file for VC++ application project 
// generated using an Application Wizard.

#include "stdafx.h"

#using <mscorlib.dll>
using namespace System;
#include <iostream>
using namespace std;

// A managed class
__gc class GcClass
{
public:
   int val;
   GcClass(int n) : val(n) 
   {
      cout << "GcClass::ctor" << endl;
   }
};

#pragma unmanaged

void UnmanagedFunction(int* pn)
{
   cout << "n is " << *pn << endl;
}

#pragma managed
void main()
{
   // Create a managed instance
   GcClass* pgc = new GcClass(3);

   // Create a pinning pointer
   GcClass __pin * ppin = pgc;

   // Pass a member to the function
   UnmanagedFunction(&ppin->val);

   // Zero out the pinning pointer
   ppin = 0;
}