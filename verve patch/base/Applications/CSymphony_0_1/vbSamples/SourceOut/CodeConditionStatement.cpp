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
        if (boolVar)
        {
            // If Outer If condition is true execute these statements
            if (otherBoolVar)
            {
                // If the Inner If is true execute these statements
            }
            else
            {
                // If the Inner If is false execute these statements
            }
            // Continue with Other statements in the Outer If
        }
        else
        {
            // Outer Else block. If condition is false, execute these statements.
        }
    };
}
