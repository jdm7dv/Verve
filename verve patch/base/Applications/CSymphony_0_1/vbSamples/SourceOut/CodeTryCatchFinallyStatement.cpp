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
        try
        {
            this->ThrowApplicationException();
        }
        catch (System::ApplicationException * ex)
        {
            // Handle any SystemApplicationException here
        }
        catch (System::CriticalException * ex)
        {
            // Handle any other exception type here
        }
        __finally
        {
            // Handle any finally block statements
        }
    };
}
