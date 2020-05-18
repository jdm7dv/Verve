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
			declClass(TypePublic, "FieldTest");
			declField(IsPrivate, "System.String", "TestStringField");
			declCtor(MemPublic);

		}
	}
}
