using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.NetworkInformation;
using DanskeWebServer.Business;
using System.Diagnostics;
using System.Net;

namespace DanskeWebServer.UnitTest
{
    [TestClass]
    public class WebServerTest
    {

        [TestMethod]
        public void CheckWebServerRunningProperly()
        {
            ProcessStartInfo info = new ProcessStartInfo(AppDomain.CurrentDomain.BaseDirectory + "\\DanskeWebServer.exe");
            info.UseShellExecute = true;
            info.Verb = "runas";
            Process.Start(info);

            WebRequest wr = WebRequest.Create("http://localhost:3000/index.html");
            wr.Timeout = 3500;
            HttpWebResponse response = (HttpWebResponse)wr.GetResponse();
            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }
    }
}
