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
			FieldRef testStr = new FieldRef (my(), "testStringField");
			declProperty(MemPublic, "System.String", "StringProperty");
			declGet();
			hi ("Get Statements come here");
			rtn(testStr);

			declSet();
			hi("Set statements come here");
			enter( equate (testStr, propval()));

		}
	}
}
