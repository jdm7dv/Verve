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
        this->TestEvent -= new System::EventHandler(this, TestMethod);
        this->TestEvent2 -= new System::EventHandler(this, TestMethod);
        this->TestEvent -= new System::EventHandler(this, TestMethod2);
        this->TestEvent3 -= new System::EventHandler(this, TestMethod3);
    };
}
