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

			MethodRef selfDispose = new MethodRef (my(), "Dispose");

			enter (ex (selfDispose, new Literal(true)));

			enter(ex (new MethodRef(my(), "SomeMethod"), new Literal (false)));

		}
	}
}
