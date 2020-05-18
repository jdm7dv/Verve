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
			declMethod(MemPublic, "System.Void", "MonMethod");

			VarRef testInt = declVar("System.Int32", "testInt", new Literal (0));
			forOn(equate (testInt, new Literal(1)));
			forTest(testInt < new Literal (10));
			forInc(testInt + new Literal(1));
			hi ("for statements come here");
			MethodRef writeLine = new MethodRef ("System.Console", "WriteLine");
			MethodRef testIntToStr = new MethodRef (testInt, "ToString");

			enter (ex (writeLine, ex(testIntToStr)));
			forOff();
	
		}
	}
}
