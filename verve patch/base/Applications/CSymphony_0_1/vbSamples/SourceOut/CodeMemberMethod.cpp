#pragma once 

#using <mscorlib.dll>

namespace MyNameSpace
{
    
    using namespace System;
    public __gc class MyClass;
    
    
    public __gc class MyClass
    {
        
        public: virtual System::String *  ReturnString(System::String *  text);
    };
    
    
    inline System::String *  MyClass::ReturnString(System::String *  text)
    {
        return text;
    };
}
