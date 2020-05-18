#pragma once 

#using <mscorlib.dll>

namespace MyNameSpace
{
    
    using namespace System;
    public __gc class MyClass;
    
    
    public __gc class MyClass
    {
        
        public:  __property virtual System::String *  get_StringProperty();
        public:  __property virtual  void set_StringProperty(System::String *  value);
    };
    
    
    inline System::String *  MyClass::get_StringProperty()
    {
        // Get Statements come here
        return this->testStringField;
    }
    inline void MyClass::set_StringProperty(System::String *  value)
    {
        // Set statements come here
        this->testStringField = value;
    }
}
