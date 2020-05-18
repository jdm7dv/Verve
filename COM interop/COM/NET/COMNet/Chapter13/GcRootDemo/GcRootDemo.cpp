// This is the main project file for VC++ application project 
// generated using an Application Wizard.

#include "stdafx.h"

// Include the gcroot header file
#include <gcroot.h>

#using <mscorlib.dll>
using namespace System;
using namespace System::Runtime::InteropServices;

// A managed class
public __gc class Managed
{
   String* name;
public:
   void SetName(String* nm) { name = nm; }
   void SayHello() { Console::WriteLine(S"Hello, {0}", name); }
};

// An unmanaged class that contains a pointer to a managed object
class Unmanaged
{
   gcroot<Managed*> pManaged;
public:

   Unmanaged(Managed* pm, const char* nm) : pManaged(pm) 
   {
      pManaged->SetName(nm);
   }

   void Hello() { pManaged->SayHello(); }
};

void main()
{
   // Create a managed object
   Managed* pman = new Managed();

   // Create and initialize an unmanaged object
   Unmanaged unman(pman, "Fred");

   unman.Hello();
}