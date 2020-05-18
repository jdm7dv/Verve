#pragma once 

#using <mscorlib.dll>

namespace MyNameSpace
{
    
    using namespace System;
    public __gc class BaseType;
    public __gc class DerivedType;
    
    
    public __gc class BaseType
    {
        
        public: BaseType();
        public: BaseType(System::String *  TestStringParameter);};
    
    public __gc class DerivedType : public MyNameSpace::BaseType
    {
        
        public: DerivedType(System::String *  System.CodeDom.CodeArgumentReferenceExpression);
        public: DerivedType();
        public: DerivedType(System::Int32 TestIntParameter);};
    
    
    inline BaseType::BaseType()
    {
    }
    
    inline BaseType::BaseType(System::String *  TestStringParameter)
    {
    }
    
    
    inline DerivedType::DerivedType(System::String *  System.CodeDom.CodeArgumentReferenceExpression) : 
            BaseType(TestStringParameter)
    {
    }
    
    inline DerivedType::DerivedType() : 
            BaseType(S"test")
    {
    }
    
    inline DerivedType::DerivedType(System::Int32 TestIntParameter)
    {
        DerivedType();
    }
}
