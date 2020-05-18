using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using CSymphony.Lib;

namespace CSymphony.CSy
{
	public class CSyUnit : GraphUnit
	{
		public override void Build()
		{
			ns("MyNameSpace");

			declClass(TypePublic, "MyClass");
			declMethod(MemPublic, "System.Void", "MyMethod");

			Literal stringPrimitive = "Test-String";
			Literal intPrimitive = 10 ;
			Literal floatPrimitive = new Literal(10.2);
			Literal nullPrimitive = new Literal(null);

			VarRef  SomeVar = "SomeVar";

			enter (equate (SomeVar, stringPrimitive));
			enter (equate (SomeVar, intPrimitive));
			enter (equate (SomeVar, floatPrimitive));
			enter (equate (SomeVar, nullPrimitive));

		}
	}
}
