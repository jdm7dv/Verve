#pragma once 

#using <mscorlib.dll>

namespace MyNameSpace
{
    
    using namespace System;
    public __gc class BaseType;
    
    
    public __gc class BaseType
    {
        
        public: virtual System::Void MyMethod();
    };
    
    
    inline System::Void BaseType::MyMethod()
    {
        // This comment was generated from a System.CodeDom.CodeComment
    };
}
