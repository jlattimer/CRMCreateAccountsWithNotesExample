using System.IO;
using System.Threading.Tasks;
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

        private const int StartingNumber = 0;
        private const int NumberToCreate = 100;

        static void Main()
        {
            CrmConnection connection = CrmConnection.Parse(
                ConfigurationManager.ConnectionStrings["CRMOnlineO365"].ConnectionString);

            using (_orgService = new OrganizationService(connection))
            {
                Parallel.For(StartingNumber, (StartingNumber + NumberToCreate), i =>
                {
                    Entity account = new Entity("account");
                    account["name"] = i.ToString(CultureInfo.InvariantCulture);

                    Guid id = _orgService.Create(account);

                    Entity annotation = new Entity("annotation");
                    annotation["objectid"] = new EntityReference("account", id);

                    //Creates a text file with the specified text
                    annotation["subject"] = "Test Text";
                    annotation["filename"] = "Test.txt";
                    annotation["documentbody"] = Convert.ToBase64String(
                        new UnicodeEncoding().GetBytes("Sample Annotation Text"));
                    annotation["mimetype"] = "text/plain";

                    ////Creates a jpg file from the included sample
                    //FileStream stream = File.OpenRead(@"Test Files\1.jpg");
                    //byte[] data = new byte[stream.Length];
                    //stream.Read(data, 0, data.Length);
                    //stream.Close();
                    //string encodedData = Convert.ToBase64String(data);
                    //annotation["subject"] = "Test JPG";
                    //annotation["filename"] = "1.jpg";
                    //annotation["documentbody"] = encodedData;
                    //annotation["mimetype"] = "image/jpg";

                    ////Creates a png file from the included sample
                    //FileStream stream = File.OpenRead(@"Test Files\2.png");
                    //byte[] data = new byte[stream.Length];
                    //stream.Read(data, 0, data.Length);
                    //stream.Close();
                    //string encodedData = Convert.ToBase64String(data);
                    //annotation["subject"] = "Test PNG";
                    //annotation["filename"] = "2.png";
                    //annotation["documentbody"] = encodedData;
                    //annotation["mimetype"] = "image/png";

                    _orgService.Create(annotation);

                    Console.WriteLine("Created: " + i);
                });
            }
        }
    }
}
