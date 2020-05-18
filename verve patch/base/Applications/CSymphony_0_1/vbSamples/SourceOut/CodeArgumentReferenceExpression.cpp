#pragma once 

#using <mscorlib.dll>

namespace MyNameSpace
{
    
    using namespace System;
    public __gc class MyClass;
    
    
    public __gc class MyClass
    {
        
        public: virtual System::Void WriteString(System::String *  System.CodeDom.CodeArgumentReferenceExpression);
    };
    
    
    inline System::Void MyClass::WriteString(System::String *  System.CodeDom.CodeArgumentReferenceExpression)
    {
        Console::WriteLine(text);
    };
}
