#pragma once 

#using <mscorlib.dll>

namespace MyNameSpace
{
    
    using namespace System;
    public __gc class MyClasss;
    
    
    public __gc class MyClasss
    {
        
        public: virtual System::Void MyMethod();
    };
    
    
    inline System::Void MyClasss::MyMethod()
    {
        this->TestMethod();
    };
}
