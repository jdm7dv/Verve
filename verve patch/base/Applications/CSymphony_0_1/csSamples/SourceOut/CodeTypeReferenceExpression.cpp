#pragma once 

#using <mscorlib.dll>

namespace MyNameSpace
{
    
    using namespace System;
    public __gc class MyClass;
    
    
    public __gc class MyClass
    {
        
        public: virtual System::Void MyMethod();
    };
    
    
    inline System::Void MyClass::MyMethod()
    {
        System::DateTime;
    };
}
