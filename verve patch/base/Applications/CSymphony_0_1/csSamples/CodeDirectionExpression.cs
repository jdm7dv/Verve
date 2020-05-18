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

			FieldRef TestParam = new FieldRef (my(), "TestParameter");

			MethodRef ThisMethod = new MethodRef (my(), "ThisMethod" );
			declClass(TypePublic, "Class1");
			declMethod(MemPublic, "System.Void", "ClassMethod");

			enter (ex (ThisMethod, refp(TestParam) ));

		}
	}
}
