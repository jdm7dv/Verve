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
        SomeVar = S"TestString";
        SomeVar = 10;
        SomeVar = 10.2;
        SomeVar = 0;
    };
}
