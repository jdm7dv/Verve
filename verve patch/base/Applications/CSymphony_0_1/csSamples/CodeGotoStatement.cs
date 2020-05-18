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
			declMethod(MemPublic, "System.Void", "MonMethod");

			jumpTo("JumpToLabel");

			MethodRef w = new MethodRef ("System.Console", "WriteLine");

			enter (ex (w , new Literal ("Test Output")));

			declLabel("JumpToLabel", ex (w, new Literal ("Output from Labeled statement")));

		}
	}
}
