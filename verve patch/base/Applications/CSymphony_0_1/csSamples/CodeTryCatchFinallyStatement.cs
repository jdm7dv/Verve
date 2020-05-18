




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

			tryOn();
				enter (ex (my(), "ThrowApplicationException"));
			catchOn("System.ApplicationException", "ex");
				hi("Handle any SystemApplicationException here");
			catchOn("System.CriticalException", "ex");
				hi("Handle any other exception type here");
			finallyOn();
				hi ("Handle any finally block statements");
			tryOff();

		}
	}
}
