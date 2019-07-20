using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp7
{
    class Program
    {
        public static object Assert { get; private set; }

        static void Main(string[] args)
        {
            // Locals
            Boolean RESULT = true;
            String url = "";
            Uri uri = null;
            String fullFileName = @"D:\_TEMP\Test-01.txt"; // @"D:\TestResources\FTP\OUT\Документооборот.zip"
            byte[] data = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream requestStream = null;
            String exceptionDetail = String.Empty;

            url = @"https://dapi-doclink.cislink.moscow/RoamingService.svc/SendPacket";
            //url = @"https://papi-doclink.cislink.com/RoamingService.svc/SendPacket";
            uri = new Uri(url);

            //fullFileName = @"D:\(s) TestResource\18852\83ce4656af68474d917e8e348094cbf7.cms";
            fullFileName = @"C:\Users\Andersen\Downloads\Microsoft.SkypeApp_kzf8qxf38zg5c!App\85f6a1022d563c0f9e0540208204b474.cms";

            data = File.ReadAllBytes(fullFileName);

            request = (HttpWebRequest)WebRequest.Create(uri);
            request.ContentType = "text/plain"; // "text/xml; charset=utf-8";
            request.Headers.Add("Content-Disposition", $"attachment; filename=\"{Path.GetFileName(fullFileName)}\"");
            request.Headers.Add("Send-Receipt-To", @"http://prosto.chtobi.bilo.ru");
            request.Method = "POST";

            requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            try
            {
                response = request.GetResponse() as HttpWebResponse;
            }
           /* catch (FaultException ex)
            {
                RESULT = false;
                MessageFault msgFault = ex.CreateMessageFault();
                var msg = msgFault.GetReaderAtDetailContents().Value;
            }*/
            catch (WebException ex)
            {
                RESULT = false;
                response = ex.Response as HttpWebResponse;

                Stream st = ex.Response.GetResponseStream();
                st.Position = 0;
                using (StreamReader reader = new StreamReader(st, Encoding.UTF8)) { exceptionDetail = reader.ReadToEnd(); }
            }
            catch (Exception e)
            {
                RESULT = false;
                throw;
            }

            //Assert.IsTrue(RESULT);
        }
    }
}
