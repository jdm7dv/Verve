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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Reflection;
using CSymphony.Lib;

namespace CSymphony.Csy
{
	public class App
	{

	    private static String usage = "usage : csy -c:<ConfigFile> -i:<InputFile> -p:<InputProviderId>";

		private  Hashtable CodeProviders = new Hashtable(5);

		private  string ConfigFile = "csy.Xml";
		
		string InPrvId, InputFile;


		XmlDocument ConfigXmlDoc = new XmlDocument();

		public static void Main(string[] args)
		{
			new App().Run(args);

		}

		private void Run(string[] args)
		{
			InitApp(args);

			CompilerResults cr = GetCompilerResults();

			if (cr.Errors.Count > 0)
			{
				ShowErrors(cr);
			}
			else
			{
				Console.WriteLine("No Errors - Generating Code Now \n");

				CodeCompileUnit unit = GetCompileUnit(cr.CompiledAssembly);
				GenerateCode(unit);
			}
		}

		public void InitApp(string[] args)
		{
			ProcessArgs(args);
			ConfigXmlDoc.Load(ConfigFile);
			GetInputInfo();
			CodeProviders[InPrvId] = new CodeProviderInfo(ConfigXmlDoc, InPrvId);
		}

		public void ProcessArgs(string[] args)
		{
			foreach (string arg in args) 
			{
				if (arg.ToUpper().StartsWith ("-?") || arg.ToUpper().StartsWith ("/?")) 
				{
					Wait(usage);
					return;
				}
				else if (arg.ToUpper().StartsWith ("-C:") || arg.ToUpper().StartsWith ("/C:"))
				{
		              this.ConfigFile = arg.Remove (0, 3);
				}
				else if (arg.ToUpper().StartsWith ("-I:") || arg.ToUpper().StartsWith ("/I:"))
				{
		              this.InputFile = arg.Remove (0, 3);
				}
				else if (arg.ToUpper().StartsWith ("-P:") || arg.ToUpper().StartsWith ("/P:"))
				{
		              this.InPrvId = arg.Remove (0, 3);
				}
			}
		}

		public void AddProvider(string PrvId)
		{
			// if the provider does not exist in the hashtable then add the provider
			//if (CodeProviders[PrvId] == null)
			//{
				
				CodeProviders[PrvId] = new CodeProviderInfo(ConfigXmlDoc, PrvId);
			//}
		}


		public void GetInputInfo()
		{
			XmlNode node = ConfigXmlDoc.SelectSingleNode("/csy/in");

			XmlAttributeCollection attribs = node.Attributes;
		
			if (InPrvId == null)
			{
				InPrvId = InnerText(attribs, "prvId", true);
			}
			if (InputFile == null)
			{
				InputFile = InnerText(attribs, "file", true);
			}
		}

		private CodeCompileUnit GetCompileUnit(Assembly Assm)
		{
			
			Type typ = Assm.GetType("CSymphony.CSy.CSyUnit");

			Object obj = Activator.CreateInstance(typ);
	        
			MethodInfo method = typ.GetMethod("GetUnit");

			return (CodeCompileUnit) method.Invoke(obj, null);
 		}

		private CompilerResults GetCompilerResults()
		{

			CodeProviderInfo ProviderInfo = (CodeProviderInfo) CodeProviders[InPrvId];

			CodeDomProvider provider =  (CodeDomProvider) ProviderInfo.CodeProvider;
	
			// Obtain an ICodeCompiler from a CodeDomProvider class.       
			//ICodeCompiler compiler = provider.CreateCompiler();
			//ICodeGenerator g = provider.CreateGenerator();

			// Configure a CompilerParameters that links System.dll and 
			// produces a file name based on the specified source file name.

			string[] Dlls = {"System.dll", "CSymphonyLib.dll"};
														  
			//CompilerParameters cp = new CompilerParameters(Dlls, .DllFile, false);
			CompilerParameters cp = new CompilerParameters(Dlls);
			cp.GenerateExecutable = false;
			cp.GenerateInMemory = true;

			// Invoke compilation.
			return  provider.CompileAssemblyFromFile(cp, InputFile );//+ "." + ProviderInfo.FileExtension);
			//CompilerResults cr = compiler.CompileAssemblyFromSource(cp, reCode.Text);
		}
			

		private void ShowErrors(CompilerResults cr)
		{
			foreach(CompilerError ce in cr.Errors) 
			{
				Console.WriteLine(ce.ToString()+"\r\n");
			}
		}
	

		private void GenerateCode(CodeCompileUnit CompileUnit)
		{
			// CreateProvidersOut

			XmlNodeList list = ConfigXmlDoc.SelectNodes("/csy/codeGen/*");
			//Console.WriteLine(list);


			// Config each of the datastores 
			foreach (XmlElement e in list)
			{
				string PrvId = e.Attributes["prvId"].InnerText;

				AddProvider(PrvId);

				CodeProviderInfo PrvInfo = (CodeProviderInfo) CodeProviders[PrvId];

				CodeDomProvider CodePrv = PrvInfo.CodeProvider;

				// Obtain an ICodeGenerator from a CodeDomProvider class.
				//ICodeGenerator gen = CodePrv.CreateGenerator();

				// Create a TextWriter to a StreamWriter to an output file.
			
				string OutPath = InnerText(e.Attributes, "path", false)  + "/" + Path.GetFileNameWithoutExtension(InputFile) + "." + PrvInfo.FileExtension;

				IndentedTextWriter tw = new IndentedTextWriter(new StreamWriter(OutPath, false), "    ");
				// Generate source code using the code generator.
				
				CodeGeneratorOptions opts = new CodeGeneratorOptions();
				//opts.BlankLinesBetweenMembers = true;
				opts.BracingStyle = "C";
				try
				{
                    CodePrv.GenerateCodeFromCompileUnit(CompileUnit, tw, opts);
				}
				catch (SystemException ex)
				{
					Wait (" ERROR : " + "\n" + ex.Message + "\n");
				}
				finally
				{
					tw.Close();
				}
			}
		}

		private static string InnerText(XmlAttributeCollection ac, string attrib, bool bThrow)
		{
			try
			{
				return ac[attrib].InnerText;
			}
		
			catch (Exception)
			{
				if (bThrow == true)
				{
					throw;
				}
				return null;
			}
		}

		public static void Wait(string s)
		{
			Console.WriteLine("\n" + s + "\n");
			Console.WriteLine("Press ENTER/RETURN to continue....");
			Console.ReadLine();
		}
	}


	public class CodeProviderInfo
	{
		public string Id;
		public string Info;
		public string FileExtension;
		public bool   BlankLinesBetweenMembers = true;
		public string BracingStyle = "C";

		public Type   ProviderType;
		public CodeDomProvider CodeProvider;

			
		public CodeProviderInfo(XmlNode docNode, string PrvId)
		{
			XmlNode node = (XmlElement) docNode.SelectSingleNode("/csy/codeProviders/codeProvider[@prvId='" + PrvId +  "']");
			XmlAttributeCollection attribs = node.Attributes;

			string typ = String.Format("{0},{1}",attribs["providerType"].InnerText, attribs["assembly"].InnerText);
			
			Id = attribs["prvId"].InnerText;

			ProviderType = Type.GetType(typ);
			Info = attribs["info"].InnerText;
			FileExtension = attribs["fileExt"].InnerText;

			BracingStyle = "C";

			CodeProvider = (CodeDomProvider) Activator.CreateInstance(ProviderType);
		}
	}
}