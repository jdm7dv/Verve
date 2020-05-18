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

			declClass(TypePublic, "MyClasss", "ParentClass");
			declMethod(MemPublic, "System.Void", "MyMethod");

			MethodRef selfDispose = new MethodRef (mySuper(), "Dispose");

			enter (ex (selfDispose));

			enter(ex (new MethodRef(mySuper(), "SomeMethod")));

		}
	}
}
