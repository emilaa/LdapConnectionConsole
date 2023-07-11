using Novell.Directory.Ldap;

namespace LdapConnect
{
    class Program
    {
        public void userList()
        {
            string ldapHost = "192.168.56.10";
            int ldapPort = 389;
            string loginDN = "cn=Administrator,cn=Users,dc=cs401,dc=local";
            string password = "Emil123.";
            string searchBase = "dc=cs401,dc=local";
            string searchFilter = "(userPrincipalName={Username})";

            try
            {

                LdapConnection conn = new LdapConnection();
                Console.WriteLine("Connecting to " + ldapHost);
                conn.Connect(ldapHost, ldapPort);
                conn.Bind(loginDN, password);
                string[] requiredAttributes = { "dc", "cn", "ou" };
                LdapSearchResults lsc = (LdapSearchResults)conn.Search(searchBase,
                                    LdapConnection.ScopeSub,
                                    searchFilter,
                                    requiredAttributes,
                                    false);
                while (lsc.HasMore())
                {
                    LdapEntry nextEntry = null;
                    try
                    {
                        nextEntry = lsc.Next();
                    }
                    catch (LdapException e)
                    {
                        Console.WriteLine("Error : " + e.LdapErrorMessage);
                        continue;
                    }
                    Console.WriteLine("\n" + nextEntry.Dn);
                    LdapAttributeSet attributeSet = nextEntry.GetAttributeSet();
                    System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();
                    while (ienum.MoveNext())
                    {
                        LdapAttribute attribute = (LdapAttribute)ienum.Current;
                        string attributeName = attribute.Name;
                        string attributeVal = attribute.StringValue;
                        Console.WriteLine("\t" + attributeName + "\tvalue  = \t" + attributeVal);
                    }
                }
                conn.Disconnect();


            }
            catch (LdapException e)
            {
                Console.WriteLine("Error :" + e.LdapErrorMessage);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error :" + e.Message);
                return;
            }
            Console.WriteLine("Press any key ");
            Console.ReadKey(true);
        }


        static void Main(string[] args)
        {
            Program programObj = new Program();
            programObj.userList();
        }
    }
}