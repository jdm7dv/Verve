#pragma once 

#using <mscorlib.dll>

namespace MyNameSpace
{
    using namespace System;
    
    using namespace System;
    public __gc class Class1;
    
    
    public __gc class Class1
    {
        
        public: virtual System::Void ClassMethod();
    };
    
    
    inline System::Void Class1::ClassMethod()
    {
        this->ThisMethod(this->TestParameter);
    };
}
