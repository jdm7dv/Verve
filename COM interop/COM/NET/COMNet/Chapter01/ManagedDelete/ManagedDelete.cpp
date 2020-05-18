#include "stdafx.h"

#using <mscorlib.dll>

using namespace System;

// Unmanaged class
class UnmanagedType
{
public:
   ~UnmanagedType() {
      Console::WriteLine("~UnmanagedType called");
   }
};

// Managed class
__gc class ManagedType
{
public:
   ~ManagedType() {
      Console::WriteLine("~ManagedType called");
   }
};

// Managed container class
__gc class Container
{
   UnmanagedType* pUman;
   ManagedType* pMan;
public:
   Container() {
      pUman = new UnmanagedType();
      pMan = new ManagedType();
   }

   ~Container() {
      delete pUman; 
      //delete pMan;
   }
};

void _tmain()
{
   Container* pc = new Container();
   delete pc;
   Console::WriteLine("after delete");
}
