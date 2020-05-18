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
			declMethod(MemPublic, "System.Void", "MonMethod");

			VarRef boolVar = "boolVar";
			VarRef otherBoolVar = "otherBoolVar";

			ifOn(boolVar);
				hi ("If Outer If condition is true execute these statements");
				ifOn(otherBoolVar);
					hi("If the Inner If is true execute these statements");
				elseOn();
					hi ("If the Inner If is false execute these statements");
				ifOff();

				hi("Continue with Other statements in the Outer If");

			elseOn();
				hi("Outer Else block. If condition is false, execute these statements.");
			ifOff();
	
		}
	}
}
