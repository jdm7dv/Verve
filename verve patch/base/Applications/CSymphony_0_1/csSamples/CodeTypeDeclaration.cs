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

			declClass("TestType", "BaseType");

			declClass(TypePublic|TypeSealed, "TestdType2","BaseType", "BaseType2");

			declInterface("aa", "bb");

		}
	}
}
