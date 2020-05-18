#pragma once 

#using <mscorlib.dll>

namespace MyNameSpace
{
    
    using namespace System;
    public __gc class Type1;
    
    
    public __gc class Type1
    {
        
        public: virtual System::Void MonMethod();
    };
    
    
    inline System::Void Type1::MonMethod()
    {
        goto JumpToLabel;
        System::Console::WriteLine(S"Test Output");
    JumpToLabel:
        System::Console::WriteLine(S"Output from Labeled statement");
    };
}
