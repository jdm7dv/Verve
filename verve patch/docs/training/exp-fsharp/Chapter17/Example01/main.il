
//  Microsoft (R) .NET Framework IL Disassembler.  Version 2.0.50727.42
//  Copyright (c) Microsoft Corporation.  All rights reserved.



// Metadata version: v2.0.50727
.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 2:0:0:0
}
.assembly extern fslib
{
  .publickeytoken = (A1 90 89 B1 C7 4D 08 09 )                         // .....M..
  .ver 1:9:2:9
}
.assembly 'ch17-pp424-fs2il'
{
  .hash algorithm 0x00008004
  .ver 0:0:0:0
}
.module 'F#-Module-ch17-pp424-fs2il'
// MVID: {072C9F60-4E50-027A-A745-0383609F2C47}
.imagebase 0x00400000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x009C0000


// =============== CLASS MEMBERS DECLARATION ===================

.class private auto ansi beforefieldinit '<PrivateImplementationDetails$ch17-pp424-fs2il>'
       extends [mscorlib]System.Object
{
} // end of class '<PrivateImplementationDetails$ch17-pp424-fs2il>'

.class private auto ansi beforefieldinit '<StartupCode>'.'Ch17-pp424-fs2il'
       extends [mscorlib]System.Object
{
  .field static assembly int32 i@4
  .field static assembly class [fslib]Microsoft.FSharp.Core.Unit _doval@5@5
  .field static assembly int32 v@6
  .field static assembly class [fslib]Microsoft.FSharp.Core.Unit _doval@7@7
  .method public static void  _main() cil managed
  {
    .entrypoint
    // Code size       56 (0x38)
    .maxstack  4
    IL_0000:  ldc.i4.2
    IL_0001:  stsfld     int32 '<StartupCode>'.'Ch17-pp424-fs2il'::i@4
    IL_0006:  ldstr      "Input a number:"
    IL_000b:  call       void [mscorlib]System.Console::WriteLine(string)
    IL_0010:  ldnull
    IL_0011:  stsfld     class [fslib]Microsoft.FSharp.Core.Unit '<StartupCode>'.'Ch17-pp424-fs2il'::_doval@5@5
    IL_0016:  call       string [mscorlib]System.Console::ReadLine()
    IL_001b:  call       int32 [mscorlib]System.Int32::Parse(string)
    IL_0020:  stsfld     int32 '<StartupCode>'.'Ch17-pp424-fs2il'::v@6
    IL_0025:  ldc.i4.2
    IL_0026:  call       int32 'Ch17-pp424-fs2il'::get_v()
    IL_002b:  mul
    IL_002c:  call       void [mscorlib]System.Console::WriteLine(int32)
    IL_0031:  ldnull
    IL_0032:  stsfld     class [fslib]Microsoft.FSharp.Core.Unit '<StartupCode>'.'Ch17-pp424-fs2il'::_doval@7@7
    IL_0037:  ret
  } // end of method 'Ch17-pp424-fs2il'::_main

} // end of class '<StartupCode>'.'Ch17-pp424-fs2il'

.class public auto ansi beforefieldinit 'Ch17-pp424-fs2il'
       extends [mscorlib]System.Object
{
  .custom instance void [fslib]Microsoft.FSharp.Core.CompilationMappingAttribute::.ctor(valuetype [fslib]Microsoft.FSharp.Core.SourceLevelConstruct) = ( 01 00 07 00 00 00 00 00 ) 
  .method public static int32  get_i() cil managed
  {
    // Code size       6 (0x6)
    .maxstack  4
    IL_0000:  ldsfld     int32 '<StartupCode>'.'Ch17-pp424-fs2il'::i@4
    IL_0005:  ret
  } // end of method 'Ch17-pp424-fs2il'::get_i

  .method public static class [fslib]Microsoft.FSharp.Core.Unit 
          get__doval@5() cil managed
  {
    // Code size       6 (0x6)
    .maxstack  4
    IL_0000:  ldsfld     class [fslib]Microsoft.FSharp.Core.Unit '<StartupCode>'.'Ch17-pp424-fs2il'::_doval@5@5
    IL_0005:  ret
  } // end of method 'Ch17-pp424-fs2il'::get__doval@5

  .method public static int32  get_v() cil managed
  {
    // Code size       6 (0x6)
    .maxstack  4
    IL_0000:  ldsfld     int32 '<StartupCode>'.'Ch17-pp424-fs2il'::v@6
    IL_0005:  ret
  } // end of method 'Ch17-pp424-fs2il'::get_v

  .method public static class [fslib]Microsoft.FSharp.Core.Unit 
          get__doval@7() cil managed
  {
    // Code size       6 (0x6)
    .maxstack  4
    IL_0000:  ldsfld     class [fslib]Microsoft.FSharp.Core.Unit '<StartupCode>'.'Ch17-pp424-fs2il'::_doval@7@7
    IL_0005:  ret
  } // end of method 'Ch17-pp424-fs2il'::get__doval@7

  .property class [fslib]Microsoft.FSharp.Core.Unit
          _doval@7()
  {
    .custom instance void [fslib]Microsoft.FSharp.Core.CompilationMappingAttribute::.ctor(valuetype [fslib]Microsoft.FSharp.Core.SourceLevelConstruct) = ( 01 00 09 00 00 00 00 00 ) 
    .get class [fslib]Microsoft.FSharp.Core.Unit 'Ch17-pp424-fs2il'::get__doval@7()
  } // end of property 'Ch17-pp424-fs2il'::_doval@7
  .property int32 v()
  {
    .custom instance void [fslib]Microsoft.FSharp.Core.CompilationMappingAttribute::.ctor(valuetype [fslib]Microsoft.FSharp.Core.SourceLevelConstruct) = ( 01 00 09 00 00 00 00 00 ) 
    .get int32 'Ch17-pp424-fs2il'::get_v()
  } // end of property 'Ch17-pp424-fs2il'::v
  .property int32 i()
  {
    .custom instance void [fslib]Microsoft.FSharp.Core.CompilationMappingAttribute::.ctor(valuetype [fslib]Microsoft.FSharp.Core.SourceLevelConstruct) = ( 01 00 09 00 00 00 00 00 ) 
    .get int32 'Ch17-pp424-fs2il'::get_i()
  } // end of property 'Ch17-pp424-fs2il'::i
  .property class [fslib]Microsoft.FSharp.Core.Unit
          _doval@5()
  {
    .custom instance void [fslib]Microsoft.FSharp.Core.CompilationMappingAttribute::.ctor(valuetype [fslib]Microsoft.FSharp.Core.SourceLevelConstruct) = ( 01 00 09 00 00 00 00 00 ) 
    .get class [fslib]Microsoft.FSharp.Core.Unit 'Ch17-pp424-fs2il'::get__doval@5()
  } // end of property 'Ch17-pp424-fs2il'::_doval@5
} // end of class 'Ch17-pp424-fs2il'


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
