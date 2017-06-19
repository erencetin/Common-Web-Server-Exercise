using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using DanskeWebServer.Common;
using System.IO;

namespace DanskeWebServer.UnitTest
{
    [TestClass]
    public class LoggingTest
    {
        [TestMethod]
        public void CreateCorrectPathAndFile()
        {

            StringBuilder fileNameString = new StringBuilder();
            fileNameString.AppendFormat("{0}", DateTime.Now.Year);
            fileNameString.AppendFormat("{0}", DateTime.Now.Month.ToString().PadLeft(2, '0'));
            fileNameString.AppendFormat("{0}.txt", DateTime.Now.Day.ToString().PadLeft(2, '0'));
            string logPath = AppDomain.CurrentDomain.BaseDirectory;
            Logger.WriteLog("test");
            Assert.IsTrue(File.Exists(logPath + "\\"+fileNameString.ToString()));

        }
        [TestMethod]
        public void WriteErrorLog()
        {

            try
            {
                throw new Exception("this is a test error");
            }
            catch (Exception ex)
            {

                Logger.WriteLog(ex);
            }

        }
    }
}
