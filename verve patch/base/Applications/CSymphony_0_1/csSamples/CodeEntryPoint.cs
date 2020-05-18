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
			uses("System");

			MethodRef ConsoleWrite = new MethodRef ("System.Console",  "WriteLine");
			Literal  HelloWorld = "Hello World from Mr. ZADCO";


			declClass(TypePublic, "Class1");
			declEntry();
			enter (ex (ConsoleWrite, HelloWorld));

		}
	}
}
