using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanskeWebServer.Common
{
    /// <summary>
    /// This is basic log mechanism that can be use general purpose and integrate any application.
    /// </summary>
    public class Logger
    {
        public static void WriteLog(string logMesage)
        {
            string logPath = AppDomain.CurrentDomain.BaseDirectory;
            string logFileName = GenerateFileName();
            System.IO.StreamWriter file = new System.IO.StreamWriter(logPath +"\\" + logFileName  , true);
            logMesage = string.Format("[{0}]   {1}",DateTime.Now.ToString(),logMesage);
            file.WriteLine(logMesage);
            file.Close();
        }
        public static void WriteLog(Exception ex)
        {
            WriteLog(string.Format("Error Message : {0} {1} StackTrace : {2} ",ex.Message,Environment.NewLine,ex.StackTrace));
        }
        /// <summary>
        /// Automaticly generates a file name for our simple log mechanism.
        /// </summary>
        /// <returns>Returns a file name</returns>
        private static string GenerateFileName()
        {
            StringBuilder fileNameString = new StringBuilder();
            fileNameString.AppendFormat("{0}", DateTime.Now.Year);
            fileNameString.AppendFormat("{0}", DateTime.Now.Month.ToString().PadLeft(2, '0'));
            fileNameString.AppendFormat("{0}.txt", DateTime.Now.Day.ToString().PadLeft(2, '0'));
            return fileNameString.ToString();
        }

    }
}
