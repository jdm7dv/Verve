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

			declClass(TypePublic, "Type1");
			FieldType intField = declField(MemPrivate, "System.Int32", "integerField");

			declCtor(MemPublic);
			
			declProperty(MemPrivate, "System.Int32", "integerProperty");
			
			declGet();
			rtn(intField);

			declSet();
			enter (equate(intField, propval()));

		}
	}
}
