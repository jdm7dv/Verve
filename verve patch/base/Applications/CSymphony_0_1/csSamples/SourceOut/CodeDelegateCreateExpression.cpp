#pragma once 

#using <mscorlib.dll>

namespace MyNameSpace
{
    
    using namespace System;
    public __gc class MyClass;
    
    
    public __gc class MyClass
    {
        
        public: virtual System::Void MonMethod();
    };
    
    
    inline System::Void MyClass::MonMethod()
    {
        this->TestEvent += new System::EventHandler(this, TestMethod);
        this->TestEvent += new System::EventHandler(this, Method2);
    };
}
