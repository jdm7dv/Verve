using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.IO;

namespace ConvertTlb
{
    class Class1
    {
        // Define RegKind enum for use in PInvoke declaration
        private enum RegKind 
        { 
            REGKIND_DEFAULT=0, REGKIND_REGISTER=1, REGKIND_NONE=2 
        }
      
        // PInvoke declaration to access function in oleaut32
        [DllImport("oleaut32.dll", CharSet=CharSet.Unicode, 
             PreserveSig=false)]
        private static extern void LoadTypeLibEx(
            String strTypeLibName, 
            RegKind regKind, 
            [MarshalAs(UnmanagedType.Interface)] out Object typeLib);

        [STAThread]
        static void Main(string[] args)
        {
            // Input and output filenames
            string infile, outfile;

            // Process command line arguments
            if (args.Length == 1)
            {
                if (File.Exists(args[0]))
                {
                    infile = args[0];
                    int extPos = infile.IndexOf('.');
                    if (extPos != -1)
                    {
                        if (infile.ToLower().EndsWith(".dll"))
                        {
                            Console.WriteLine("Error: output file would overwrite input file");
                            return;
                        }
                        outfile = infile.Substring(0, extPos) + ".dll";
                    }
                    else
                        outfile = infile + ".dll";
                }
                else
                {
                    Console.WriteLine("File {0} not found", args[0]);
                    return;
                }
            }
            else if (args.Length == 2)
            {
                if (File.Exists(args[0]))
                {
                    infile = args[0];
                    outfile = args[1];
                    Console.WriteLine("Infile: {0}, outfile: {1}", infile, outfile);
                }
                else
                {
                    Console.WriteLine("File {0} not found", args[0]);
                    return;
                }
            }
            else
            {
                Console.WriteLine("Usage: ConvertTlb <typelib> <outfile>");
                return;
            }

            // Call LoadTypeLibEx to load the type library. RegKind_None 
            // means that the type library will not be registered in 
            // the Windows registry
            Object typeLib;
            LoadTypeLibEx(infile, RegKind.REGKIND_NONE, out typeLib); 
      
            // Check the call worked
            if(typeLib == null)
            {
                Console.WriteLine("LoadTypeLibEx failed.");
                return;
            }
         
            // Create a TypeLibConverter and an event handler for events 
            // raised during the conversion
            TypeLibConverter converter = new TypeLibConverter();
            ConversionEventHandler eventHandler = 
                new ConversionEventHandler();

            // Call ConvertTypeLibToAssembly on the loaded library
            AssemblyBuilder asm = converter.ConvertTypeLibToAssembly(
                typeLib, outfile, 0, eventHandler, 
                null, null, null, null);   

            // Save the interop assembly
            asm.Save(outfile);
        }
    }

    // Define an event handler class for use by the converter
    public class ConversionEventHandler : ITypeLibImporterNotifySink
    {
        public void ReportEvent(ImporterEventKind eventKind, 
            int eventCode, string eventMsg)
        {
            Console.WriteLine("Event msg: " + eventMsg);
        }
   
        public Assembly ResolveRef(object typeLib)
        {
            Console.WriteLine("ResolveRef called");
            return null; 
        }   
    }
}