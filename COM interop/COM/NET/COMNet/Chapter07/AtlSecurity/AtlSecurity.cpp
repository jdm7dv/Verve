#include <iostream>
using namespace std;

// Needed because some features are only available on 2000 and above
#define _WIN32_WINNT 0x0500

// Access rights - defined in <Winnt.h>
#define STANDARD_RIGHTS_ALL              (0x001F0000L)

#include <atlbase.h>
#include <atlsecurity.h>
#include <atlstr.h>
using namespace ATL;

int main()
{
   // Get the SID for a file object, and print details
   // AtlGetOwnerSid function takes a name and an object type,
   // and fills in the fields of a CSid object
   CSid sid;
   if (!AtlGetOwnerSid("c:\\temp\\output.cpp", SE_FILE_OBJECT, &sid))
      cout << "Error getting SID" << endl;
   else
   {
      cout << "Account: " << sid.AccountName() << endl;
      cout << "Domain: " << sid.Domain() << endl;
      cout << "SID: " << sid.Sid() << endl;
   }

   // Get the security descriptor
   CSecurityDesc sd;
   if (!AtlGetSecurityDescriptor("c:\\temp\\output.cpp", SE_FILE_OBJECT, &sd))
      cout << "Error getting security descriptor" << endl;
   else
   {
      cout << "Security Descriptor:" << endl;
      SECURITY_INFORMATION si = OWNER_SECURITY_INFORMATION | GROUP_SECURITY_INFORMATION |
         DACL_SECURITY_INFORMATION | SACL_SECURITY_INFORMATION;

      // The ToString function produces a string representation of a security descriptor.
      // This can be converted back to a security descriptor using the FromString function.
      // Note that this function is only available on Windows 2000 and later.
      CString cs;
      sd.ToString(&cs, si);
      cout << cs << endl;

      // Get the DACL
      CDacl dcl;
      bool bPresent, bDefault;
      sd.GetDacl(&dcl, &bPresent, &bDefault);
      cout << "DACL: present=" << bPresent << ", defaulted=" << bDefault << endl;

      if (dcl.IsNull()) cout << "DACL is null" << endl;
      if (dcl.IsEmpty()) cout << "DACL is empty" << endl;

      // Add an ACE. First, create a SID for the administrator account
      CSid sid2("Administrator");
      // Add it to the DACL
      if (dcl.AddAllowedAce(sid2, STANDARD_RIGHTS_ALL))
      {
         cout << "ACE added OK" << endl;
         // Set the DACL back into the security descriptor
         try
         {
            sd.SetDacl(dcl);
            cout << "DACL added OK" << endl;
         }
         catch(CAtlException& ae)
         {
            cout << "Error setting DACL (" << hex << ae.m_hr << dec << ")" << endl;
         }
      }
      else
         cout << "Error adding ACE" << endl;
   }
   return 0;
}
