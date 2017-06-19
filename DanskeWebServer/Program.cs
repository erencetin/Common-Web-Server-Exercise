using DanskeWebServer.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace DanskeWebServer
{
    class Program
    {
        /// <summary>
        /// ATTENTION!
        /// This console application must be run as Administrator. 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string path = ConfigurationManager.AppSettings["webPath"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
            CustomHttpServer server = new CustomHttpServer();            
            server.Start(path, port);
            //http://localhost:3000/index.html      
        }
    }
}
