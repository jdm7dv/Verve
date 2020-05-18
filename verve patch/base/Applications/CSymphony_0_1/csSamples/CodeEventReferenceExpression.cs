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

			declClass(TypePublic, "MyClasss");
			declMethod(MemPublic, "System.Void", "MyMethod");

			EventRef testEvent = new EventRef (my(), "TestEvent");

			enter (testEvent);

		}
	}
}
