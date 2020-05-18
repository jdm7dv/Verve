#region About
// Copyright : Max Virtual, LLC
// License   : GNU General Public License
// Author    : H.G. Sajnani
// Contact   : csy@maxvirtual.com
#endregion

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Reflection;

namespace CSymphony.Lib
{
	#region NestedClasses
	public class VarName : CodeVariableReferenceExpression
	{
		public VarName()
		{
		}

		public VarName(string name) : base(name)
		{
			//base.VariableName = name;
		}

		public static implicit operator VarName(string name)
		{
				return new VarName(name);
		}
	}

	public class TypeRef : CodeTypeReference
	{
		public TypeRef(string name) : base(name)
		{
			//base.VariableName = name;
		}

		public TypeRef(System.Type sysType) : base(sysType)
		{
		}

		public static implicit operator TypeRef(string typeName)
		{
			return new TypeRef(typeName);
		}

		public static implicit operator CodeExpression(TypeRef refType)
		{
			return new CodeTypeReferenceExpression(refType);
		}

		public CodeTypeReferenceExpression Expr()
		{
			return new CodeTypeReferenceExpression(this);
		}

	}

	public class Literal : OperandRef
	{
		public Literal(object val) 
		{
			base.Expression = new CodePrimitiveExpression(val);
		}

		public static implicit operator Literal(string str)
		{
			return new Literal(str);
		}

		public static implicit operator Literal(int i)
		{
			return new Literal(i);
		}
	}

	public class AttribArgType : CodeAttributeArgument
	{
		public AttribArgType(CodeExpression expression)
			: base(expression)
		{
		}

		public AttribArgType(string argName, CodeExpression argValue)
			: base(argName, argValue)
		{
		}
	}

	public class AttribType : CodeAttributeDeclaration
	{
		public AttribType(string name)
			: base(name)
		{
		}

		public AttribType(string name, params AttribArgType[] arguments)
			: base(name, arguments)
		{
		}
	}

	public class FieldType : CodeMemberField
	{
		public FieldType(MemberAttributes memberAttributes, CodeTypeReference fldType, string fieldName) 
			: base(fldType, fieldName)
		{
			this.Attributes = memberAttributes;	
		}

		public FieldType(MemberAttributes memberAttributes, CodeTypeReference fldType, string fieldName, CodeExpression initExpression) 
			: this (memberAttributes, fldType, fieldName)
		{
			this.InitExpression = initExpression;
		}

		public static implicit operator CodeExpression(FieldType fldType)
		{
			CodeFieldReferenceExpression f = new CodeFieldReferenceExpression();
			f.FieldName = fldType.Name;
			f.TargetObject = GraphUnit.my();
			return f;
		}

		public static implicit operator string(FieldType fldType)
		{
			return fldType.Name;
		}
	}

	public class PropertyType : CodeMemberProperty
	{
		public PropertyType(MemberAttributes memberAttributes, object propertyType, string propertyName) 
			: this (memberAttributes, propertyType, propertyName, true, true)
		{
		}

		public PropertyType(MemberAttributes memberAttributes, object propertyType, string propertyName, bool hasGet, bool hasSet) 
		{
			this.Type = GraphUnit.NewType(propertyType);
			this.Name = propertyName;
			this.Attributes = memberAttributes;	
			this.HasGet = hasGet;
			this.HasSet = hasSet;
		}

		public static implicit operator CodeExpression(PropertyType PropertyName)
		{
			CodePropertyReferenceExpression p = new CodePropertyReferenceExpression();
			p.PropertyName = PropertyName.Name;
			//f.TargetObject = GraphUnit.my;
			return p;
		}
	}

	public class MethodType : CodeMemberMethod
	{
		public MethodType()
		{
		}

		public static implicit operator CodeExpression(MethodType method)
		{
			CodeMethodReferenceExpression p = new CodeMethodReferenceExpression();
			p.MethodName = method.Name;
			//f.TargetObject = GraphUnit.my;
			return p;
		}
	}

	public class EntryType : CodeEntryPointMethod
	{
		public EntryType()
		{
		}
	}

	public class EventType : CodeMemberEvent
	{
		public EventType(MemberAttributes memberAttributes, object eventType, string eventName)
		{
			this.Attributes = memberAttributes;
			this.Name = eventName;
			this.Type = GraphUnit.NewType(eventType);
		}

		public static implicit operator CodeExpression(EventType eventType)
		{
			CodeEventReferenceExpression e = new CodeEventReferenceExpression();
			e.EventName = eventType.Name;
			//!maybe this is not necessary - come back after implementing trigger;
			e.TargetObject = GraphUnit.my();
			return e;
		}

		public static implicit operator string(EventType typeEvent)
		{
			return typeEvent.Name;
		}
	}

	public class DelegateType : CodeTypeDelegate
	{
		public DelegateType(TypeAttributes attributes, object returnType, string delegateName)
		{
			this.TypeAttributes = attributes;
			this.Name = delegateName;
			this.ReturnType = GraphUnit.NewType(returnType);
		}
	
		public static implicit operator CodeExpression(DelegateType handler)
		{
			return new CodeTypeReferenceExpression(handler.Name);
		}

		public static implicit operator CodeTypeReference(DelegateType handler)
		{
			return new CodeTypeReference(handler.Name);
		}

		public CodeDelegateCreateExpression Create(CodeExpression targetObject, string methodName)
		{
			CodeDelegateCreateExpression d = new CodeDelegateCreateExpression();
			d.DelegateType = this;
			d.TargetObject = targetObject;
			d.MethodName = methodName;
			return d;
		}

		public CodeDelegateCreateExpression Create(string methodName)
		{
			return this.Create(GraphUnit.my(), methodName);
		}
	}

	public class DataDefType : CodeTypeDeclaration
	{
		public DataDefType()
		{
		}

		public DataDefType(TypeAttributes attributes, string defName, params object[] baseTypes)
			: this(defName, baseTypes)
		{
			this.TypeAttributes = attributes;
		}
		
		public DataDefType(string defName, params object[] baseTypes)
			: base (defName)
		{
			foreach (object obj in baseTypes)
			{
				this.BaseTypes.Add(GraphUnit.NewType(obj));
			}
		}
		
		public static implicit operator CodeExpression(DataDefType defType)
		{
			return new CodeTypeReferenceExpression(defType.Name);
		}

		public static implicit operator CodeTypeReference(DataDefType defType)
		{
			return new CodeTypeReference(defType.Name);
		}
	}

	public class VarType : CodeVariableDeclarationStatement
	{
		public VarType(CodeTypeReference typeVar, string varName, CodeExpression initExpression) 
			: this(typeVar, varName)
		{
			base.InitExpression = initExpression;
		}

		public VarType(CodeTypeReference typeVar, string varName) 
			: base(typeVar, varName)
		{
			
		}

		public static implicit operator CodeExpression(VarType typeVar)
		{
			return new CodeVariableReferenceExpression(typeVar.Name);
		}

		public static implicit operator string(VarType typeVar)
		{
			return typeVar.Name;
		}
	}

	
	public class EventRef : CodeEventReferenceExpression
	{
		public EventRef(CodeExpression targetObject, string eventName)
			: base(targetObject, eventName)
		{
		}
	}

	public class HandlerType : CodeDelegateCreateExpression
	{
		public HandlerType(object handlerType, CodeExpression methodTarget, string methodName)
			: base(GraphUnit.NewType(handlerType), methodTarget, methodName)
		{
		}

		public HandlerType(object handlerType, MethodRef method)
			: base(GraphUnit.NewType(handlerType), ((CodeMethodReferenceExpression)method.Expression).TargetObject, 
												((CodeMethodReferenceExpression)method.Expression).MethodName)
		{
		}
		/*
		public HandlerType(object handlerType, MethodRef methodRef)
			: base(GraphUnit.NewType(handlerType), methodRef.TargetObject, methodRef.MethodName)
		{
		}
		*/
	}

	public class VarRef : OperandRef
	{
        public VarRef(string varName)
		{
			base.Expression = new CodeVariableReferenceExpression(varName);
		}

		public static implicit operator VarRef(string str)
		{
			return new VarRef(str);
		}
	}

	public class MethodRef : OperandRef
	{
		public MethodRef(CodeExpression targetObject, string methodName)
		{
			myExpression = new CodeMethodReferenceExpression(targetObject, methodName);
		}

		public MethodRef(TypeRef typRef, string methodName)
		{
			myExpression = new CodeMethodReferenceExpression(typRef, methodName);
		}

		public MethodRef(string targetType, string methodName) 
		{
			TypeRef t = targetType;
			myExpression = new CodeMethodReferenceExpression(t, methodName);


		}

	}

	public class PropertyRef : OperandRef
	{
		public PropertyRef(CodeExpression targetObject, string propertyName)
		{
			base.myExpression = new CodePropertyReferenceExpression(targetObject, propertyName);
		}
	}

	public class FieldRef : OperandRef
	{
		public FieldRef(CodeExpression targetObject, string fieldName)
		{
			myExpression = new CodeFieldReferenceExpression(targetObject, fieldName);
		}

		public FieldRef(string targetName, string fieldName)
		{

		}
	}

	public class BinOperRef : OperandRef
	{
		public BinOperRef(CodeExpression expression)
		{
			myExpression = expression;
		}
	}

	public class ArgRef : OperandRef
	{
		public ArgRef(string parameterName)
		{
			base.Expression = new CodeArgumentReferenceExpression(parameterName);
		}

		public static implicit operator ArgRef(string name)
		{
			return new ArgRef(name);
		}

		public static implicit operator string (ArgRef refArg)
		{
			CodeArgumentReferenceExpression e = (CodeArgumentReferenceExpression)(refArg.Expression);
			return e.ParameterName;
		}
	}

	public abstract class OperandRef 
	{
		protected CodeExpression myExpression;

		public OperandRef()
		{
		}

		public  static BinOperRef operator + (CodeExpression expr1, OperandRef expr2)
		{
			return new BinOperRef(new  CodeBinaryOperatorExpression(expr1, CodeBinaryOperatorType.Add, expr2.myExpression));
		}

		public  static CodeExpression operator - (CodeExpression expr1, OperandRef expr2)
		{
			return new BinOperRef(new  CodeBinaryOperatorExpression(expr1, CodeBinaryOperatorType.Subtract, expr2.myExpression));
		}

		public  static CodeExpression operator * (CodeExpression expr1, OperandRef expr2)
		{
			return new BinOperRef(new  CodeBinaryOperatorExpression(expr1, CodeBinaryOperatorType.Multiply, expr2.myExpression));
		}

		public  static CodeExpression operator / (CodeExpression expr1, OperandRef expr2)
		{
			return new BinOperRef(new  CodeBinaryOperatorExpression(expr1, CodeBinaryOperatorType.Divide, expr2.myExpression));
		}

		public  static CodeExpression operator % (CodeExpression expr1, OperandRef expr2)
		{
			return new BinOperRef(new  CodeBinaryOperatorExpression(expr1, CodeBinaryOperatorType.Modulus, expr2.myExpression));
		}

		public  static CodeExpression operator > (CodeExpression expr1, OperandRef expr2)
		{
			return new BinOperRef(new  CodeBinaryOperatorExpression(expr1, CodeBinaryOperatorType.GreaterThan, expr2.myExpression));
		}

		public  static CodeExpression operator >= (CodeExpression expr1, OperandRef expr2)
		{
			return new BinOperRef(new  CodeBinaryOperatorExpression(expr1, CodeBinaryOperatorType.GreaterThanOrEqual, expr2.myExpression));
		}

		public  static CodeExpression operator < (CodeExpression expr1, OperandRef expr2)
		{
			return new BinOperRef(new  CodeBinaryOperatorExpression(expr1, CodeBinaryOperatorType.LessThan, expr2.myExpression));
		}

		public  static CodeExpression operator <= (CodeExpression expr1, OperandRef expr2)
		{
			return new BinOperRef(new  CodeBinaryOperatorExpression(expr1, CodeBinaryOperatorType.LessThanOrEqual, expr2.myExpression));
		}

		public  static CodeExpression operator == (CodeExpression expr1, OperandRef expr2)
		{
			return new BinOperRef(new  CodeBinaryOperatorExpression(expr1, CodeBinaryOperatorType.IdentityInequality, expr2.myExpression));
		}

		public  static CodeExpression operator != (CodeExpression expr1, OperandRef expr2)
		{
			return new BinOperRef(new  CodeBinaryOperatorExpression(expr1, CodeBinaryOperatorType.IdentityInequality, expr2.myExpression));
		}

		public static implicit operator CodeExpression(OperandRef expression)
		{
			return expression.Expression;
		}

		public CodeExpression Expression
		{
			get {return myExpression;}
			set {myExpression = value;}
		}

		public CodeExpression Expr()
		{
			return myExpression;
		}

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
	}
	#endregion
}
