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

			declMethod(MemPublic, "System.Void", "MyMethod");

			hi("This comment was generated from a System.CodeDom.CodeComment");

		}
	}
}
