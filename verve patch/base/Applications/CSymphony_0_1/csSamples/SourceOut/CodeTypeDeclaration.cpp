#pragma once 

#using <mscorlib.dll>

namespace MyNameSpace
{
    
    using namespace System;
    public __gc class TestType;
    public __gc sealed class TestdType2;
    public __gc interface aa;
    
    
    public __gc class TestType : public BaseType
    {
    };
    
    public __gc sealed class TestdType2 : public BaseType, public BaseType2
    {
    };
    
    public __gc interface aa : public bb
    {
    };
    
    
    
}
