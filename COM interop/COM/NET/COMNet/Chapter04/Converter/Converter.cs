using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

// Define a managed equivalent for the ITypeLib interface
[
ComImport,
Guid("00020406-0000-0000-C000-000000000046"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
ComVisible(false)
]
public interface ITypeLibInterface 
{
    void CreateTypeInfo();
    void SetName();
    void SetVersion();
    void SetGuid();
    void SetDocString();
    void SetHelpFileName();
    void SetHelpContext();
    void SetLcid();
    void SetLibFlags();
    void SaveAllChanges();
}

public class Converter 
{
    public static void Main() 
    {
        // Create assembly, converter and callback objects 
        Assembly asm = Assembly.LoadFrom("Account.dll");
        TypeLibConverter converter = new TypeLibConverter();
        ConversionEvents eventHandler = new ConversionEvents();
      
        // Do the conversion
        ITypeLibInterface typeLib = 
            (ITypeLibInterface)converter.ConvertAssemblyToTypeLib(
            asm, "MyTypeLib.tlb", 0, eventHandler);

        // Save the library
        typeLib.SaveAllChanges();
    }
}

public class ConversionEvents : ITypeLibExporterNotifySink 
{
    public void ReportEvent(ExporterEventKind eventKind, 
        int eventCode, string eventMsg) 
    {
        Console.WriteLine("Event: " + eventMsg);
    }
   
    public Object ResolveRef(Assembly asm) 
    {
        // Resolve the reference here and return a correct type library
        return null; 
    }   
}
