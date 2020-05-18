using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ScriptableControl
{
   [ComImport, 
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown), 
    Guid("CB5BDC81-93C1-11cf-8F20-00805F2CD064")]
   public interface IObjectSafety
   {
      [PreserveSig()]
      UInt32 GetInterfaceSafetyOptions( 
         ref Guid riid,
         out Int32 pdwSupportedOptions,
         out Int32 pdwEnabledOptions);
           
      [PreserveSig()]
      UInt32 SetInterfaceSafetyOptions(
         ref Guid riid,
         Int32 dwOptionSetMask,
         Int32 dwEnabledOptions);
   }


	/// <summary>
	/// This is a simple button.
	/// </summary>
	public class MyButton : Button, IObjectSafety
	{
		public MyButton()
		{
		}

      // Standard return values.
      public const UInt32 S_OK            = 0x00000000;
      public const UInt32 E_FAIL          = 0x00000001;
      public const UInt32 E_NOINTERFACE   = 0x80004002;

      // Options for IObjectSafety
      public const Int32 INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
      public const Int32 INTERFACESAFE_FOR_UNTRUSTED_DATA   = 0x00000002;

      #region IObjectSafety Members

      public UInt32 GetInterfaceSafetyOptions(ref Guid riid, out Int32 pdwSupportedOptions, out Int32 pdwEnabledOptions)
      {
         // TODO:  Add MyButton.GetInterfaceSafetyOptions implementation
         pdwSupportedOptions = new Int32 ();
         pdwEnabledOptions = new Int32 ();
         return E_NOINTERFACE;
      }

      public UInt32 SetInterfaceSafetyOptions(ref Guid riid, Int32 dwOptionSetMask, Int32 dwEnabledOptions)
      {
         // TODO:  Add MyButton.SetInterfaceSafetyOptions implementation
         return E_NOINTERFACE;
      }

      #endregion
   }
}
