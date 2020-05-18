#pragma once 

#using <mscorlib.dll>

namespace MyNameSpace
{
    
    using namespace System;
    public __gc class Type1;
    
    
    public __gc class Type1
    {
        
        public: virtual System::Void MyMethod();
    };
    
    
    inline System::Void Type1::MyMethod()
    {
        System::DateTime();
    };
}
