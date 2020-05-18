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

			// Class : BaseType
			declClass(TypePublic, "BaseType");

			declCtor(MemPublic);
			declCtor(MemPublic, "System.String", "TestStringParameter");

			// Class : DerviedType
			declClass(TypePublic, "DerivedType", "BaseType");
			ArgRef argTestParam = "TestStringParameter";

			declCtor(MemPublic, "System.String" , argTestParam);
			parent(argTestParam);

			declCtor(MemPublic);
			parent(new Literal("test"));

			declCtor(MemPublic, "System.Int32", "TestIntParameter");
			peer();

		}
	}
}
