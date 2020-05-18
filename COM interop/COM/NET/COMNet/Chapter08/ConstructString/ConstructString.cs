using System;
using System.EnterpriseServices;
using System.Runtime.InteropServices;

// This attribute turns off access control checks for this
// example. Normally you would turn the access control checks
// on and define various security checks.
[assembly: ApplicationAccessControl(false)]

// This attribute givens the COM+ application a specific name.
[assembly: ApplicationName("ConstStringApp")]

namespace ConstructString
{
   /// <summary>
   /// An interface used to access the CheckString functions.
   /// </summary>
   [Guid("BB924921-68D6-4b14-8771-878EDCFEC8B2"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
   public interface ICheckString
   {
      string GetConstructorString();
   }

	/// <summary>
	/// This class shows how to work with COM+ object constructor
	/// strings.
	/// </summary>
   [Guid("93707E2D-7672-4d3f-A8F3-4FB934DB9BE1"),
    ClassInterface(ClassInterfaceType.None),
    ConstructionEnabled(Default="Hello world")]
   public class CheckString : ServicedComponent, ICheckString
	{
      private string _ConstructorValue;
		public CheckString()
		{
			//
			// TODO: Add constructor logic here
			//
      }

      /// <summary>
      /// This method is called after the object constructor. It
      /// enables the developer to work with the constructor string
      /// in creating object values or performing setups.
      /// </summary>
      /// <param name="constructString">The value of the constructor string.</param>
      protected override void Construct(string constructString)
      {
         // Save the value of the constructor string in a
         // private variable.
         _ConstructorValue = constructString;
      }

      /// <summary>
      /// This method allows the caller to retrieve the constructor
      /// string value.
      /// </summary>
      /// <returns>The value of the constructor string.</returns>
      public string GetConstructorString()
      {
         // Return the current value of the constructor string.
         return _ConstructorValue;
      }
   }
}
