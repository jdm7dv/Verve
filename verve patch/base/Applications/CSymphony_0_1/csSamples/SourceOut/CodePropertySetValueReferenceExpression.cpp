#pragma once 

#using <mscorlib.dll>

namespace MyNameSpace
{
    
    using namespace System;
    public __gc class Type1;
    
    
    public __gc class Type1
    {
        
        private: System::Int32 integerField;
        
        public: Type1();
        public:  __property virtual System::Int32 get_integerProperty();
        public:  __property virtual  void set_integerProperty(System::Int32 value);
    };
    
    
    
    inline Type1::Type1()
    {
    }
    
    inline System::Int32 Type1::get_integerProperty()
    {
        return this->integerField;
    }
    inline void Type1::set_integerProperty(System::Int32 value)
    {
        this->integerField = value;
    }
}
