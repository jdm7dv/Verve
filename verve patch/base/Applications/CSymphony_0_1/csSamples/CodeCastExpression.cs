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
			declClassAttrib("xxxx", new AttribArgType("aaaa", new Literal ("yyy")));
			declMethod(MemPublic, "System.Void", "MyMethod");
			enter ( cast ("System.Int64", new Literal (1000)));

		}
	}
}
