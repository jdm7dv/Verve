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
        System::Int32 testInt = 0;
        for (        testInt = 1; (testInt < 10);         (testInt + 1))
        {
            // for statements come here
            System::Console::WriteLine(testInt.ToString());
        }
    };
}
