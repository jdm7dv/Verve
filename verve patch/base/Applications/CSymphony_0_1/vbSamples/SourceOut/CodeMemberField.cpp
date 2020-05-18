#pragma once 

#using <mscorlib.dll>

namespace MyNameSpace
{
    
    using namespace System;
    public __gc class FieldTest;
    
    
    public __gc class FieldTest
    {
        
        private: System::String *  TestStringField;
        
        public: FieldTest();};
    
    
    
    inline FieldTest::FieldTest()
    {
    }
}
