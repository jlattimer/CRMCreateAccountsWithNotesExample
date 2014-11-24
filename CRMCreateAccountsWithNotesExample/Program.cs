using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using System;
using System.Configuration;
using System.Globalization;
using System.Text;

namespace CRMCreateAccountsWithNotesExample
{
    class Program
    {
        private static OrganizationService _orgService;

        private const int NumberToCreate = 100;

        static void Main()
        {
            CrmConnection connection = CrmConnection.Parse(
                ConfigurationManager.ConnectionStrings["CRMOnlineO365"].ConnectionString);

            using (_orgService = new OrganizationService(connection))
            {
                for (int i = 0; i < NumberToCreate; i++)
                {
                    Entity account = new Entity("account");
                    account["name"] = i.ToString(CultureInfo.InvariantCulture);

                    Guid id = _orgService.Create(account);

                    Entity annotation = new Entity("annotation");
                    annotation["objectid"] = new EntityReference("account", id);
                    annotation["subject"] = "test";
                    annotation["filename"] = "test.txt";
                    annotation["documentbody"] = Convert.ToBase64String(
                        new UnicodeEncoding().GetBytes("Sample Annotation Text"));
                    annotation["mimetype"] = "text/plain";

                    _orgService.Create(annotation);

                    Console.WriteLine("Created: " + i);
                }
            }
        }
    }
}
