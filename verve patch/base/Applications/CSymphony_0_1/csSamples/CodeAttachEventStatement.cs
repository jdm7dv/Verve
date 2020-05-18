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

			EventRef evnt = new EventRef(my(), "TestEvent");
			MethodRef mthd = new MethodRef(my(), "TestMethod");

			attach(evnt, "System.EventHandler", mthd);

			attach(my(), "TestEvent2", "System.EventHandler", mthd);

			attach(evnt, "System.EventHandler", my(), "TestMethod2");

			attach(my(), "TestEvent3", "System.EventHandler", my(), "TestMethod3");

		}
	}
}
