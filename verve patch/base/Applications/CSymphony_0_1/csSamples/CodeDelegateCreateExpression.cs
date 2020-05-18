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

			HandlerType handler = new HandlerType("System.EventHandler", mthd);
			HandlerType handler2 = new HandlerType("System.EventHandler", my(), "Method2");


			attach(evnt, handler);

			attach(evnt, handler2);

		}
	}
}
