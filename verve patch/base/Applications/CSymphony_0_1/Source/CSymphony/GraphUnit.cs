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
	/// <summary>
	/// Summary description for GraphUnit.
	/// </summary>
	public abstract class GraphUnit
	{
		protected CodeCompileUnit myUnit = new CodeCompileUnit();
		protected CodeNamespace currNamespace = new CodeNamespace();
		protected CodeIterationStatement currFor;
		protected CodeConditionStatement currIf;
		protected CodeMemberMethod currMethod;
		protected CodeCatchClause currCatch;
		protected PropertyType currProperty;
		protected Stack typeStack = new Stack(10);
		protected Stack statStack = new Stack(10);
		protected Stack tryStack = new Stack(10);
		protected Stack cycleStack = new Stack(10);
		protected Stack ifStack = new Stack(10);

		protected CodeTryCatchFinallyStatement currTry;

		protected CodeParameterDeclarationExpression currParameter;

		protected bool myAutoAdd = true;

		public const MemberAttributes MemConst = MemberAttributes.Const;
		public const MemberAttributes MemFamily = MemberAttributes.Family;
		public const MemberAttributes MemFamilyAndAssembly = MemberAttributes.FamilyAndAssembly;
		public const MemberAttributes MemFamilyOrAssembly = MemberAttributes.FamilyOrAssembly;
		public const MemberAttributes MemFinal = MemberAttributes.Final;
		public const MemberAttributes MemNew = MemberAttributes.New;
		public const MemberAttributes MemOverloaded = MemberAttributes.Overloaded;
		public const MemberAttributes MemOverride = MemberAttributes.Override;
		public const MemberAttributes MemPrivate = MemberAttributes.Private;
		public const MemberAttributes MemPublic = MemberAttributes.Public;
		public const MemberAttributes MemScopeMask = MemberAttributes.ScopeMask;
		public const MemberAttributes MemStatic = MemberAttributes.Static;
		public const MemberAttributes MemVTableMask = MemberAttributes.VTableMask;

		public const TypeAttributes TypeAbstract = TypeAttributes.Abstract;
		public const TypeAttributes TypeAnsiClass  = TypeAttributes.AnsiClass;
		public const TypeAttributes TypeAutoClass  = TypeAttributes.AutoClass;
		public const TypeAttributes TypeAutoLayout  = TypeAttributes.AutoLayout;
		public const TypeAttributes TypeBeforeFieldInit  = TypeAttributes.BeforeFieldInit;
		public const TypeAttributes TypeClass  = TypeAttributes.Class;
		public const TypeAttributes TypeClassSemanticsMask  = TypeAttributes.ClassSemanticsMask;
		public const TypeAttributes TypeExplicitLayout  = TypeAttributes.ExplicitLayout;
		public const TypeAttributes TypeHasSecurity  = TypeAttributes.ExplicitLayout;
		public const TypeAttributes TypeImport  = TypeAttributes.Import;
		public const TypeAttributes TypeInterface  = TypeAttributes.Interface;
		public const TypeAttributes TypeLayoutMask  = TypeAttributes.LayoutMask;
		public const TypeAttributes TypeNestedAssembly  = TypeAttributes.NestedAssembly;
		public const TypeAttributes TypeNestedFamANDAssem  = TypeAttributes.NestedFamANDAssem;
		public const TypeAttributes TypeNestedFamily  = TypeAttributes.NestedFamily;
		public const TypeAttributes TypeNestedFamORAssem  = TypeAttributes.NestedFamORAssem;
		public const TypeAttributes TypeNestedPrivate  = TypeAttributes.NestedPrivate;
		public const TypeAttributes TypeNestedPublic  = TypeAttributes.NestedPublic;
		public const TypeAttributes TypeNotPublic  = TypeAttributes.NotPublic;
		public const TypeAttributes TypePrivate  = TypeAttributes.NotPublic;
		public const TypeAttributes TypePublic  = TypeAttributes.Public;
		public const TypeAttributes TypeReservedMask  = TypeAttributes.ReservedMask;
		public const TypeAttributes TypeRTSpecialName  = TypeAttributes.RTSpecialName;
		public const TypeAttributes TypeSealed  = TypeAttributes.Sealed;
		public const TypeAttributes TypeSequentialLayout  = TypeAttributes.SequentialLayout;
		public const TypeAttributes TypeSerializable  = TypeAttributes.Serializable;
		public const TypeAttributes TypeSpecialName  = TypeAttributes.SpecialName;
		public const TypeAttributes TypeStringFormatMask  = TypeAttributes.StringFormatMask;
		public const TypeAttributes TypeUnicodeClass  = TypeAttributes.UnicodeClass;
		public const TypeAttributes TypeVisibilityMask  = TypeAttributes.VisibilityMask;


		public const CodeBinaryOperatorType add =  CodeBinaryOperatorType.Add ;
		public const CodeBinaryOperatorType assign =  CodeBinaryOperatorType.Assign ;
		public const CodeBinaryOperatorType bitand =  CodeBinaryOperatorType.BitwiseAnd ;
		public const CodeBinaryOperatorType bitor =  CodeBinaryOperatorType.BitwiseOr ;
		public const CodeBinaryOperatorType booland =  CodeBinaryOperatorType.BooleanAnd ;
		public const CodeBinaryOperatorType boolor =  CodeBinaryOperatorType.BooleanOr;
		public const CodeBinaryOperatorType divide =  CodeBinaryOperatorType.Divide ;
		public const CodeBinaryOperatorType gt =  CodeBinaryOperatorType.GreaterThan ;
		public const CodeBinaryOperatorType ge =  CodeBinaryOperatorType.GreaterThanOrEqual ;
		public const CodeBinaryOperatorType eq =  CodeBinaryOperatorType.IdentityEquality ;
		public const CodeBinaryOperatorType ne =  CodeBinaryOperatorType.IdentityInequality ;
		public const CodeBinaryOperatorType lt =  CodeBinaryOperatorType.LessThan ;
		public const CodeBinaryOperatorType le =  CodeBinaryOperatorType.LessThanOrEqual ;
		public const CodeBinaryOperatorType modulus =  CodeBinaryOperatorType.Modulus ;
		public const CodeBinaryOperatorType multiply =  CodeBinaryOperatorType.Multiply ;
		public const CodeBinaryOperatorType sub =  CodeBinaryOperatorType.Subtract ;
		public const CodeBinaryOperatorType valeq =  CodeBinaryOperatorType.ValueEquality ;

		public GraphUnit()
		{
		}

		public GraphUnit(string nameSpace) : this()
		{
			// Add the new namespace to the compile unit.
			currNamespace.Name = nameSpace;
			myUnit.Namespaces.Add(currNamespace);
		}

		/// <summary>
		/// Adds a namespace to the current compile unit.
		/// </summary>
		/// <param name="nameSpace"></param>
		public CodeNamespace ns(string nameSpace)
		{
			currNamespace = new CodeNamespace(nameSpace);
			myUnit.Namespaces.Add(currNamespace);
			return currNamespace;
		}
		
		/// <summary>
		/// Adds a list of the Namespaces to be imported / used to the current namespace.
		/// </summary>
		/// <param name="uses"></param>
		public void uses(params string[] uses)
		{
			foreach (string use in uses)
			{
				currNamespace.Imports.Add(new CodeNamespaceImport(use));
			}
		}

		/// <summary>
		/// Adds a class.
		/// </summary>
		/// <param name="attributes"></param>
		/// <param name="className"></param>
		/// <param name="baseTypes"></param>
		public DataDefType declClass (TypeAttributes attributes, string className, params object[] baseTypes)
		{
			DataDefType def = declClass(className, baseTypes);
			def.TypeAttributes = attributes;
			return def;
		}

		/// <summary>
		/// Adds a class.
		/// </summary>
		/// <param name="className"></param>
		/// <param name="baseTypes"></param>
		public DataDefType declClass (string className, params object[] baseTypes)
		{
			DataDefType def = declDataDefType(className, baseTypes);
			def.IsClass = true;
			return def;
		}

		/// <summary>
		/// Adds a new struct.
		/// </summary>
		/// <param name="attributes"></param>
		/// <param name="structName"></param>
		/// <param name="baseTypes"></param>
		public DataDefType declStruct (TypeAttributes attributes, string structName, params object[] baseTypes)
		{
			DataDefType def = declStruct(structName, baseTypes);
			def.TypeAttributes = attributes;
			return def;
		}

		/// <summary>
		/// Adds a new struct.
		/// </summary>
		/// <param name="structName"></param>
		/// <param name="baseTypes"></param>
		public DataDefType declStruct (string structName, params object[] baseTypes)
		{
			DataDefType def = declDataDefType(structName, baseTypes);
			def.IsStruct = true;
			return def;
		}

		/// <summary>
		/// Adds a new Interface.
		/// </summary>
		/// <param name="attributes"></param>
		/// <param name="interfaceName"></param>
		/// <param name="baseTypes"></param>
		public DataDefType declInterface (TypeAttributes attributes, string interfaceName, params object[] baseTypes)
		{
			DataDefType def = declInterface(interfaceName, baseTypes);
			def.TypeAttributes = attributes;
			return def;
		}

		/// <summary>
		/// Adds a new Interface.
		/// </summary>
		/// <param name="interfaceName"></param>
		/// <param name="baseTypes"></param>
		public DataDefType declInterface (string interfaceName, params object[] baseTypes)
		{
			DataDefType def = declDataDefType(interfaceName, baseTypes);
			def.IsInterface = true;
			return def;
		}

		/// <summary>
		/// Adds a new Enum.
		/// </summary>
		/// <param name="attributes"></param>
		/// <param name="enumName"></param>
		/// <param name="baseTypes"></param>
		public DataDefType declEnum(TypeAttributes attributes, string enumName, params object[] baseTypes)
		{
			DataDefType def = declEnum(enumName, baseTypes);
			def.TypeAttributes = attributes;
			return def;
		}

		/// <summary>
		/// Adds a new Enum.
		/// </summary>
		/// <param name="enunName"></param>
		/// <param name="baseTypes"></param>
		public DataDefType declEnum (string enumName, params object[] baseTypes)
		{
			DataDefType def = declDataDefType(enumName, baseTypes);
			def.IsEnum = true;
			return def;
		}

		private DataDefType declDataDefType (TypeAttributes attributes, string typeName, params object[] baseTypes)
		{
			DataDefType def = declDataDefType(typeName, baseTypes);
			def.TypeAttributes = attributes;
			return def;
		}

		private DataDefType declDataDefType(string className, params object[] baseTypes)
		{
			DataDefType def = new DataDefType(className, baseTypes);
			AddMemberType(def);
			typePush(def);
			return def;
		}

		private DataDefType declNestedClass (TypeAttributes attributes, string className, params object[] baseTypes)
		{
			DataDefType def = new DataDefType(attributes, className, baseTypes);
			//def.Attributes = attributes;
			def.IsClass = true;
			AddMember(def);
			typePush(def);
			return def;
		}

		/// <summary>
		/// Adds a new field to the current Type.
		/// </summary>
		/// <param name="attributes"></param>
		/// <param name="fieldType"></param>
		/// <param name="fieldName"></param>
		public FieldType declField(MemberAttributes attributes, object fieldType, string fieldName)
		{
			FieldType fld =  new FieldType(attributes, NewType(fieldType), fieldName);
			AddMember(fld);
			return fld;
		}

		public FieldType declField(MemberAttributes attributes, object fieldType, string fieldName, CodeExpression initExpression)
		{
			FieldType fld =  new FieldType(attributes, NewType(fieldType), fieldName);
			fld.InitExpression = initExpression;
			AddMember(fld);
			return fld;
		}

		public FieldType declField (MemberAttributes attributes, object fieldType, string fieldName, string propertyName, bool hasGet, bool hasSet) 
		{
			FieldType fld =  new FieldType(attributes, NewType(fieldType), fieldName);
			AddMember(fld);

			if (propertyName == "_") 
			{
				propertyName = fieldName.Substring(1);
			}
			if (propertyName == "my") 
			{
				propertyName = fieldName.Substring(3);
			}

			// if propertyName == "+" then Capitalize the first character
			currProperty = new PropertyType(MemPublic, fieldType, propertyName, hasGet, hasSet);
			if (currProperty.HasGet == true)
			{
				currProperty.GetStatements.Add( new CodeMethodReturnStatement(fld));
			}
			if (currProperty.HasSet == true)
			{
				currProperty.SetStatements.Add( new CodeAssignStatement(fld, propval()));
			}
			AddMember(currProperty);
			return fld;
		}

		public PropertyType declProperty(MemberAttributes attributes, object propertyType, string propertyName, bool hasGet, bool hasSet) 
		{
			currProperty = new PropertyType(MemPublic, propertyType, propertyName, hasGet, hasSet);
			AddMember(currProperty);
			return currProperty ;
		}

		public PropertyType declProperty(MemberAttributes attributes, object propertyType, string propertyName)
		{
			return declProperty(attributes, propertyType, propertyName, true, true);
		}

		public void declGet()
		{

			statStack.Clear();
			statPush(currProperty.GetStatements);
		}

		public void declSet()
		{
			statStack.Clear();
			statPush(currProperty.SetStatements);
		}


		public EventType declEvent(MemberAttributes attributes, object eventType, string eventName)
		{
			EventType evnt =  new EventType(attributes, NewType(eventType), eventName);
			AddMember(evnt);
			return evnt;
		}

		public static CodeDelegateCreateExpression newHandler (object delegateType, CodeExpression targetObject, string methodName)
		{
			return new CodeDelegateCreateExpression(NewType(delegateType), targetObject, methodName);
		}

		public static CodeDelegateCreateExpression newHandler (object delegateType, MethodRef method)
		{
			CodeMethodReferenceExpression m = (CodeMethodReferenceExpression) method.Expression;

			return new CodeDelegateCreateExpression(NewType(delegateType), m.TargetObject, m.MethodName);
		}

		public void nestOn()
		{
			typePush(typePeek());
		}

		public void nestOff()
		{
			typePop();
		}

		public DelegateType declDelegate (TypeAttributes attributes, object returnType, string delegateName, params object[] parameters)
		{
			DelegateType d = new DelegateType(attributes,returnType, delegateName);
			AddParameters(d.Parameters, parameters);
			AddMemberType(d);
			return d;
		}

		public static CodeTypeReference NewType(object type)
		{
			if (type.GetType() == typeof(System.String))
				return new CodeTypeReference((string)type);
			else if (type.GetType() == typeof(System.Type))
				return new CodeTypeReference((System.Type)type);
			else if (type.GetType() == typeof(TypeRef))
				return new CodeTypeReference( ((TypeRef)type).BaseType );
			else
				return new CodeTypeReference(type.ToString());
		}

		public CodeStatement enter(CodeExpression expression)
		{
			return enter(new CodeExpressionStatement(expression));
		}

		public CodeStatement enter(CodeStatement statement)
		{
			statPeek().Add(statement);
			return statement;
		}

		private CodeStatement _enter(CodeStatement statement)
		{
			if ( (myAutoAdd == true) && (statement !=null) )
				enter(statement);
			return statement;
		}

		private CodeStatement _enter(CodeExpression expression)
		{
			return _enter(new CodeExpressionStatement(expression));
		}

		public CodeIterationStatement forOn(CodeStatement initStatement,
			CodeExpression testExpression,CodeStatement incrementStatement)
		{
			currFor = forOn(initStatement);
			forTest(testExpression);
			forInc(incrementStatement);
			return currFor;
		}

		public CodeIterationStatement forOn(CodeStatement initStatement,
			CodeStatement incrementStatement)
		{
			currFor = forOn(initStatement);
			forInc(incrementStatement);
			return currFor;
		}

		public CodeIterationStatement forOn(CodeStatement initStatement)
		{
			currFor = new CodeIterationStatement();
			// Add the statement to the parent Statements;
			enter(currFor);
			// Push the statements collections
			// all the new statements would be written to the For Statements
			cyclePush(currFor);
			statPush(currFor.Statements);
			if (initStatement !=null)
			{
				currFor.InitStatement= initStatement;
			}
			else
			{
				currFor.InitStatement = new CodeSnippetStatement("");
				currFor.IncrementStatement = new CodeSnippetStatement("");
			}
			return currFor;
		}

		public void forInc(CodeStatement statement)
		{
			cyclePeek().IncrementStatement = statement;
		}

		public void forInc(CodeExpression expression)
		{
			cyclePeek().IncrementStatement = new CodeExpressionStatement(expression);
		}

		public void forTest(CodeExpression expression)
		{
			cyclePeek().TestExpression = expression;
		}

		public void forOff()
		{
			statPop();
			cyclePop();
		}

		public CodeParameterDeclarationExpressionCollection AddParameters(CodeParameterDeclarationExpressionCollection collection ,params object[] paramNameValue)
		{
			int j = paramNameValue.GetLength(0);
			if (j % 2 == 1)		
			{
				j--;
			}
			CodeTypeReference name;
			string val;
			for (int i = 0 ;   i < j  ; i+=2)
			{
				name = NewType(paramNameValue[i]);
				if (paramNameValue[i+1].GetType() == typeof(ArgRef))
				{
					//val = ((ArgRef)paramNameValue[i+1]).ParameterName;
					val = ((ArgRef)paramNameValue[i+1]);
				}
				else 
				{
					val = paramNameValue[i+1].ToString();
				}
				collection.Add( new CodeParameterDeclarationExpression(name, val));
			}
			return collection;
		}

		public CodeExpression NewParameter(object fieldName, object fieldType)
		{
			return  new CodeParameterDeclarationExpression(fieldName.ToString(), fieldType.ToString());
		}

		public static CodeMethodInvokeExpression ex(TypeRef typRef, string methodName, params CodeExpression[] parameters)
		{
			return new CodeMethodInvokeExpression(typRef, methodName, parameters);
		}

		public static CodeMethodInvokeExpression ex(CodeExpression targetObject, string methodName, params CodeExpression[] parameters)
		{
			return new CodeMethodInvokeExpression(targetObject, methodName, parameters);
		}

		public static CodeMethodInvokeExpression ex(string typeName, string methodName, params CodeExpression[] parameters)
		{
			return ex(new TypeRef(typeName), methodName, parameters);
		}

		public static CodeMethodInvokeExpression ex(MethodRef refMethod, params CodeExpression[] parameters)
		{
			CodeMethodReferenceExpression e = (CodeMethodReferenceExpression) refMethod.Expression;
			//return (new BinOperRef( new CodeMethodInvokeExpression(e.TargetObject, e.MethodName, parameters)));
			return (new CodeMethodInvokeExpression(e.TargetObject, e.MethodName, parameters));
		}

		public static CodeEventReferenceExpression prop(CodeExpression targetObject, string propertyName)
		{
			return new CodeEventReferenceExpression(targetObject, propertyName);
		}

		public static CodeEventReferenceExpression prop(string propertyName)
		{
			return new CodeEventReferenceExpression(my(), propertyName);
		}

		public static CodeTypeOfExpression kindof(object typeName)
		{
			return new CodeTypeOfExpression(NewType(typeName));
		}

		public static CodeSnippetStatement empty()
		{
			return new CodeSnippetStatement("");
		}

		public CodeCompileUnit CompileUnit
		{
			get {return myUnit;}
		}

		public void begintype(CodeTypeDeclaration type)
		{
			typeStack.Push(type);
		}

		public void loadType()
		{
			// create a copy of the last member upon the last member
			// assuming begintype & endtype would be used consistenly
			// begintype would be of the type last on the stack
			typeStack.Push(typeStack.Pop());
		}

		public void loadMethod(CodeMemberMethod method)
		{
			statStack.Push(method.Statements);
		}


		public void unloadMethod()
		{
			statStack.Pop();
		}

		public void unloadType()
		{
			typeStack.Pop();
		}

		#region Stack Operation

		public CodeStatementCollection statPeek()
		{
			return (CodeStatementCollection)_Peek(statStack);
		}

		public void statPush(CodeStatementCollection collection)
		{
			statStack.Push(collection);
		}

		public CodeStatementCollection statPop()
		{
			return (CodeStatementCollection)statStack.Pop();
		}

		public DataDefType typePeek()
		{
			return (DataDefType)_Peek(typeStack);
		}

		public void typePush(DataDefType dataDef)
		{
			typeStack.Push(dataDef);
		}

		/*
		public DataDefType typeGetCurr()
		{
			CodeTypeMemberCollection c = typePeek();
			if ((c == null) || (c.Count < 1))
			{
				return null;
			}
			else
			{
				return (DataDefType) c[c.Count-1];
			}
		}
		*/

		public CodeTypeMemberCollection typePop()
		{
			return (CodeTypeMemberCollection)typeStack.Pop();
		}

		public CodeTryCatchFinallyStatement tryPop()
		{
			return (CodeTryCatchFinallyStatement)tryStack.Pop();
		}

		public CodeTryCatchFinallyStatement tryPeek()
		{
			return (CodeTryCatchFinallyStatement)_Peek(tryStack);
		}

		public void tryPush(CodeTryCatchFinallyStatement statement)
		{
			tryStack.Push(statement);
		}

		public CodeIterationStatement cyclePop()
		{
			return (CodeIterationStatement)cycleStack.Pop();
		}

		public CodeIterationStatement cyclePeek()
		{
			return (CodeIterationStatement)_Peek(cycleStack);
		}

		public void cyclePush(CodeIterationStatement statement)
		{
			cycleStack.Push(statement);
		}

		public CodeConditionStatement ifPop()
		{
			return (CodeConditionStatement)ifStack.Pop();
		}

		public CodeConditionStatement ifPeek()
		{
			return (CodeConditionStatement)_Peek(ifStack);
		}

		public void ifPush(CodeConditionStatement statement)
		{
			ifStack.Push(statement);
		}

		public void AddMemberType(CodeTypeDeclaration typeDec)
		{
			if (currNamespace != null)
			{
				currNamespace.Types.Add(typeDec);
			}
			else 
			{
				// throw exception;
			}
		/*
			if (typeStack.Count < 1)
			{
				if (currNamespace != null)
				{
					currNamespace.Types.Add(typeDec);
				}
				else 
				{
					// throw exception;
				}
			}
			else 
			{
				AddMember(typeDec);
			}
		*/
		}

		public void AddMember(CodeTypeMember member)
		{
			if (typeStack.Count > 0)
			{
				typePeek().Members.Add(member);
			}
			else 
			{
				// throw exception;
			}
		}

		private object _Peek(Stack s)
		{
			if ((s != null) && (s.Count > 0))
			{
				return s.Peek(); 
			}
			else 
			{
				return null;
			}
		}

		#endregion

		#region Statements
		
		public static CodeAssignStatement equate(CodeExpression leftExpression, CodeExpression rightExpression)
		{	
			return new CodeAssignStatement(leftExpression, rightExpression);
		}
		

		public CodeDelegateInvokeExpression trigger(CodeExpression targetObject, string eventName, params CodeExpression[] parameters)
		{
			EventRef  er = new EventRef(targetObject, eventName);
			CodeDelegateInvokeExpression ie =  new CodeDelegateInvokeExpression(er, parameters);
			_enter(ie);
			return ie;
		}

		public CodeDelegateInvokeExpression trigger(EventRef targetEvent, params CodeExpression[] parameters)
		{
			CodeDelegateInvokeExpression d = new CodeDelegateInvokeExpression(targetEvent, parameters);
			_enter(d);
			return d;
		}
		
		public CodeAttachEventStatement attach(CodeExpression targetObject, string eventName, object handlerType, CodeExpression methodTarget, string methodName)
		{
			EventRef e = new EventRef (targetObject, eventName);
			CodeDelegateCreateExpression dce = newHandler(handlerType, methodTarget, methodName);

			MethodRef m = new MethodRef(methodTarget, methodName);
			return attach (e, handlerType, m);
		}

		public CodeAttachEventStatement attach(CodeExpression targetObject, string eventName, object handlerType, MethodRef methodRef)
		{
			EventRef e = new EventRef (targetObject, eventName);
			return attach (e, handlerType, methodRef);
		}

		public CodeAttachEventStatement attach(EventRef eventRef, object handlerType, MethodRef methodRef)
		{
			CodeAttachEventStatement stat = new CodeAttachEventStatement(eventRef, newHandler(handlerType, methodRef));
			_enter(stat);
			return stat;
		}

		public CodeAttachEventStatement attach(EventRef eventRef, object handlerType, CodeExpression methodTarget, string methodName)
		{
			CodeAttachEventStatement stat = new CodeAttachEventStatement(eventRef, newHandler(handlerType, methodTarget, methodName));
			_enter(stat);
			return stat;
		}

		public CodeAttachEventStatement attach(EventRef eventRef, HandlerType handler)
		{
			CodeAttachEventStatement stat = new CodeAttachEventStatement(eventRef, handler);
			_enter(stat);
			return stat;
		}

		public CodeAttachEventStatement attach(CodeExpression targetObject, string eventName, HandlerType handler)
		{
			EventRef e = new EventRef (targetObject, eventName);
			return attach (e, handler);
		}

		public CodeRemoveEventStatement detach(CodeExpression targetObject, string eventName, object handlerType, CodeExpression methodTarget, string methodName)
		{
			EventRef e = new EventRef (targetObject, eventName);
			CodeDelegateCreateExpression dce = newHandler(handlerType, methodTarget, methodName);

			MethodRef m = new MethodRef(methodTarget, methodName);
			return detach (e, handlerType, m);
		}

		public CodeRemoveEventStatement detach(CodeExpression targetObject, string eventName, object handlerType, MethodRef methodRef)
		{
			EventRef e = new EventRef (targetObject, eventName);
			return detach (e, handlerType, methodRef);
		}

		public CodeRemoveEventStatement detach(EventRef eventRef, object handlerType, MethodRef methodRef)
		{
			CodeRemoveEventStatement stat = new CodeRemoveEventStatement(eventRef, newHandler(handlerType, methodRef));
			_enter(stat);
			return stat;
		}

		public CodeRemoveEventStatement detach(EventRef eventRef, object handlerType, CodeExpression methodTarget, string methodName)
		{
			CodeRemoveEventStatement stat = new CodeRemoveEventStatement(eventRef, newHandler(handlerType, methodTarget, methodName));
			_enter(stat);
			return stat;
		}

		public CodeRemoveEventStatement detach(EventRef eventRef, HandlerType handler)
		{
			CodeRemoveEventStatement stat = new CodeRemoveEventStatement(eventRef, handler);
			_enter(stat);
			return stat;
		}

		public CodeRemoveEventStatement detach(CodeExpression targetObject, string eventName, HandlerType handler)
		{
			EventRef e = new EventRef (targetObject, eventName);
			return detach(e, handler);
		}


		public CodeThrowExceptionStatement throwEx(CodeExpression expression)
		{
			CodeThrowExceptionStatement stat = new CodeThrowExceptionStatement(expression) ;
			_enter(stat);
			return stat;
		}

		public void ifOn(CodeExpression expression)
		{
			currIf = new CodeConditionStatement();
			currIf.Condition = expression;
			_enter(currIf);
			ifPush(currIf);
			statPush(ifPeek().TrueStatements);
		}

		public void elseOn()
		{
			statPop();
			statPush(ifPeek().FalseStatements);
		}

		/* Currently nested if's  not implemented
		 * public void elseiftrue(CodeExpression expression)
		{
			statPop();
			statPush(ifPeek().FalseStatements);
			iftrue(expression);
		}
		*/
		public void ifOff()
		{
			statPop();
			ifPop();
		}

		public CodeStatement tryOn()
		{
			currCatch = null;
			currTry = new CodeTryCatchFinallyStatement();
			_enter(currTry);
			tryPush(currTry);
			statPush(tryPeek().TryStatements);
			return currTry;
		}

		public CodeStatement finallyOn()
		{
			statPop();
			statPush(tryPeek().FinallyStatements);
			return currTry;
		}

		public void catchOn(object exceptionType , string localName)
		{
			CodeCatchClause c = new CodeCatchClause();
			c.CatchExceptionType = NewType(exceptionType);
			c.LocalName = localName;
			tryPeek().CatchClauses.Add(c);
			statPop();
			statPush(c.Statements);
		}

		public void tryOff()
		{
			statPop();
			tryPop();
		}

		public void catchOff()
		{
			statPop();
			tryPop();
		}


		
		public void jumpTo(string label)
		{
			enter( new CodeGotoStatement(label));

		}
		public CodeCommentStatement hi(string text)
		{
			 return hi(text, false);
		}

		public CodeCommentStatement hi(string text, bool docComment)
		{
			CodeCommentStatement comment= new CodeCommentStatement(text, docComment);
			if (statPeek() != null)
			{
				statPeek().Add(comment);
			}
			else if ( typePeek() != null) 
			{
				typePeek().Comments.Add(comment);
			}
			else if (currNamespace != null)
			{
				currNamespace.Comments.Add(comment);
			}
			return comment;
		}

		public CodeCommentStatement hi(CodeTypeMember target, string text)
		{
			return hi (target, text, false);
		}

		public CodeCommentStatement hi(CodeTypeMember target, string text, bool docComment)
		{
			CodeCommentStatement comment = new CodeCommentStatement(text, docComment);
			target.Comments.Add (comment); 
			return comment;
		}

		public CodeMethodReturnStatement rtn(CodeExpression expression)
		{
			CodeMethodReturnStatement statement = new CodeMethodReturnStatement(expression);
			_enter(statement);
			return statement;
		}

		public CodeObjectCreateExpression newObj(object createType, params CodeExpression[] parameters)
		{
			return new CodeObjectCreateExpression(NewType(createType), parameters);
		}

		public CodeArrayCreateExpression newArray(object createType, int size)
		{
			return new CodeArrayCreateExpression(NewType(createType), size);
		}

		public CodeArrayCreateExpression newArray(object createType, CodeExpression sizeExpression)
		{
			return new CodeArrayCreateExpression(NewType(createType),sizeExpression);
		}

		public CodeArrayCreateExpression newArray(object createType, params CodeExpression[] parameters)
		{
			return new CodeArrayCreateExpression(NewType(createType), parameters);
		}



		#endregion
			public 	VarRef declLocal(object varType, object varName)
		{
			VarType v = new VarType(NewType(varType), varName.ToString());
			_enter((CodeStatement) v);
			return new VarRef(varName.ToString());
		}

		public 	VarRef declLocal(object varType, object varName, CodeExpression initExpression)
		{
			VarType v = new VarType(NewType(varType), varName.ToString(), initExpression);
			_enter((CodeStatement) v);
			return new VarRef(varName.ToString());
		}

		public 	VarRef declVar(object varType, object varName)
		{
			VarType v = new VarType(NewType(varType), varName.ToString());
			_enter((CodeStatement) v);
			return new VarRef(varName.ToString());
		}

		public 	VarRef declVar(object varType, object varName, CodeExpression initExpression)
		{
			VarType v = new VarType(NewType(varType), varName.ToString(), initExpression);
			_enter((CodeStatement) v);
			return new VarRef(varName.ToString());
		}


		public CodeLabeledStatement declLabel(string labelName, CodeStatement statement)
		{
			CodeLabeledStatement s =  new CodeLabeledStatement(labelName, statement) ;
			_enter(s);
			return s;
		}

		public CodeLabeledStatement declLabel(string labelName, CodeExpression expression)
		{
			return declLabel(labelName, new CodeExpressionStatement(expression));
		}

		public CodeLabeledStatement declLabel(string labelName)
		{
			CodeLabeledStatement s =  new CodeLabeledStatement(labelName) ;
			return s;
		}

		public bool AutoAdd(bool val)
		{
			return (myAutoAdd = val);
		}
		
		
		public CodeMemberMethod declMethod(MemberAttributes attributes, object returnType, string methodName, params object[] parameters)
		{
			statStack.Clear();
			MethodType method = new MethodType();
			statPush(method.Statements);
			method.Attributes = attributes;
			method.Name = methodName;
			method.ReturnType = NewType(returnType);
			AddParameters(method.Parameters, parameters);
			AddMember(method);
			return method;
		}

		public CodeEntryPointMethod declEntry()
		{
			statStack.Clear();
			CodeEntryPointMethod method = new CodeEntryPointMethod();
			statPush(method.Statements);
			AddMember(method);
			return method;
		}


		public CodeMemberMethod declCtor(MemberAttributes attributes, params object[] parameters)
		{
			statStack.Clear();
			CodeMemberMethod method = new CodeConstructor();
			statPush(method.Statements);
			method.Attributes = attributes;
			AddParameters(method.Parameters,parameters);
			AddMember(method);
			currMethod = method;
			return method;
		}

		public CodeMemberMethod declTypeCtor(MemberAttributes attributes, params object[] parameters)
		{
			statStack.Clear();
			CodeMemberMethod method = new CodeTypeConstructor();
			statPush(method.Statements);
			method.Attributes = attributes;
			AddParameters(method.Parameters,parameters);
			AddMember(method);
			currMethod = method;
			return method;
		}


		public void ctorOff()
		{
			// or statStack.Clear;
			statPop();
		}
		
		public void parent(params CodeExpression[] expressions)
		{
			if (expressions.Length == 0)
			{
				((CodeConstructor)currMethod).BaseConstructorArgs.Add(new CodeSnippetExpression(""));
			}
			else
			{
				foreach (CodeExpression expression in expressions)
				{
					((CodeConstructor)currMethod).BaseConstructorArgs.Add(expression);
				}			}
		}

		public AttribType declClassAttrib(string attribName, params AttribArgType[] arguments)
		{
			AttribType at = new AttribType(attribName, arguments);
			typePeek().CustomAttributes.Add(at);
			return at;
		}

		public AttribType declMethodAttrib(string attribName, params AttribArgType[] arguments)
		{
			AttribType at = new AttribType(attribName, arguments);
			currMethod.CustomAttributes.Add(at);
			return at;
		}

		public void peer(params CodeExpression[] expressions)
		{
			if (expressions.Length == 0  )
			{
				((CodeConstructor)currMethod).ChainedConstructorArgs.Add(new CodeSnippetExpression(""));
			}
			else
			{
				foreach (CodeExpression expression in expressions)
				{
					((CodeConstructor)currMethod).ChainedConstructorArgs.Add(expression);
				}			}
		}
		
		#region expressions
		public static CodeArrayIndexerExpression cell(CodeExpression expression, params CodeExpression[] indices)
		{
			return new CodeArrayIndexerExpression(expression, indices);
		}

		public static CodeIndexerExpression indx(CodeExpression expression, params CodeExpression[] indices)
		{
			return new CodeIndexerExpression(expression, indices);
		}

		public static CodeDirectionExpression inp(CodeExpression expression)
		{
			return new CodeDirectionExpression(FieldDirection.In, expression);
		}

		public static CodeDirectionExpression outp(CodeExpression expression)
		{
			return new CodeDirectionExpression(FieldDirection.Out, expression);
		}

		public static CodeDirectionExpression refp (CodeExpression expression)
		{
			return new CodeDirectionExpression(FieldDirection.Ref, expression);
		}

		public static CodeParameterDeclarationExpression inp(object fieldType, string fieldName)
		{
			CodeParameterDeclarationExpression param =	 new CodeParameterDeclarationExpression(NewType(fieldType), fieldName);
			param.Direction = FieldDirection.In;
			return param;
		}

		public static CodeParameterDeclarationExpression outp(object fieldType, string fieldName)
		{
			CodeParameterDeclarationExpression param =	 new CodeParameterDeclarationExpression(NewType(fieldType), fieldName);
			param.Direction = FieldDirection.Out;
			return param;
		}

		public static CodeParameterDeclarationExpression refp(object fieldType, string fieldName)
		{
			CodeParameterDeclarationExpression param =	 new CodeParameterDeclarationExpression(NewType(fieldType), fieldName);
			param.Direction = FieldDirection.Ref;
			return param;
		}

		public static CodeFieldReferenceExpression member(CodeExpression targetObject, string fieldName)
		{
			return new CodeFieldReferenceExpression(targetObject, fieldName);
		}

		public static CodeFieldReferenceExpression member(object targetType, string fieldName)
		{
			CodeTypeReferenceExpression target = new CodeTypeReferenceExpression(NewType(targetType));
			return new CodeFieldReferenceExpression(target, fieldName);
		}

		public static CodeFieldReferenceExpression member(CodeExpression targetObject, FieldType dataField)
		{
			return new CodeFieldReferenceExpression(targetObject, dataField.Name);
		}

		public static CodePropertyReferenceExpression myself(PropertyType aliasField)
		{
			return new CodePropertyReferenceExpression(my(), aliasField.Name);
		}

		public static CodeEventReferenceExpression myself(EventType msgHandler)
		{
			return new CodeEventReferenceExpression(my(), msgHandler.Name);
		}

		public static CodePrimitiveExpression newLit(object val)
		{
			return new CodePrimitiveExpression(val);
		}

		public static CodeVariableReferenceExpression newVar(string variableName)
		{
			return new CodeVariableReferenceExpression(variableName);
		}

		public static CodeBinaryOperatorExpression oper(CodeExpression left, CodeBinaryOperatorType operatorType, CodeExpression right)
		{
			return new CodeBinaryOperatorExpression(left, operatorType, right);
		}

		public static CodeThisReferenceExpression my()
		{
			return new CodeThisReferenceExpression();
		}

		public static CodeBaseReferenceExpression mySuper()
		{
			return new CodeBaseReferenceExpression();
		}


		public static CodePropertySetValueReferenceExpression propval()
		{
			return new CodePropertySetValueReferenceExpression();
		}

		public static CodeObjectCreateExpression make(object classType)
		{
			return new CodeObjectCreateExpression(NewType(classType));
		}

		public static CodeObjectCreateExpression make (object classType, params CodeExpression[] parameters)
		{
			return new CodeObjectCreateExpression(NewType(classType), parameters);
		}

		public static CodeCastExpression cast(object newType, CodeExpression expression)
		{
			return new CodeCastExpression(NewType(newType), expression);
		}

		#endregion

		public CodeCompileUnit GetUnit()
		{
			Build();
			return myUnit;
		}

		public abstract void Build();
	}
}
