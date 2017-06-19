using DanskeWebServer.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DanskeWebServer.Business
{
    public class CustomHttpServer
    {
        #region Variables
        private Thread serverThread;
        private string rootDirectory;
        private HttpListener listener;
        private int port;
        public int Port
        {
            get { return port; }
            private set { }
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Start to listen request on given port and directory
        /// </summary>
        /// <param name="dir">The directory that consists our web pages</param>
        /// <param name="port">Our web port number</param>
        public void Start(string dir, int port)
        {
            if (!Directory.Exists(dir))
            {
                Logger.WriteLog("Web server directory doesn't exist.");
                throw new Exception("Please check if your web server directory exists.");
            }
            this.rootDirectory = dir;
            this.port = port;
            serverThread = new Thread(this.Listen);
            Logger.WriteLog("Server is started.");
            serverThread.Start();
        }
        /// <summary>
        /// Stop server and dispose all functions.
        /// </summary>
        public void Stop()
        {
            serverThread.Abort();
            listener.Stop();
        }
        #endregion
        #region Private Methods
        private void Listen()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://*:" + port.ToString() + "/");
            listener.Start();
            while (true)
            {
                try
                {
                    HttpListenerContext context = listener.GetContext();
                    Process(context);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex);
                }
            }
        }
        private void Process(HttpListenerContext context)
        {
            string filename = context.Request.Url.AbsolutePath;
            filename = filename.Substring(1);
            if (string.IsNullOrEmpty(filename))
            {
                foreach (string initialFile in ServerResources.InitialFiles)
                {
                    if (File.Exists(Path.Combine(rootDirectory, initialFile)))
                    {
                        filename = initialFile;
                        break;
                    }
                }
            }
            filename = Path.Combine(rootDirectory, filename);
            if (File.Exists(filename))
            {
                try
                {
                    Stream input = new FileStream(filename, FileMode.Open);
                    //Adding permanent http response headers
                    string mime;
                    context.Response.ContentType = ServerResources.MimeTypes.TryGetValue(Path.GetExtension(filename), out mime) ? mime : "application/octet-stream";
                    context.Response.ContentLength64 = input.Length;
                    context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                    context.Response.AddHeader("Last-Modified", System.IO.File.GetLastWriteTime(filename).ToString("r"));

                    byte[] buffer = new byte[1024 * 16];
                    int nbytes;
                    while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                        context.Response.OutputStream.Write(buffer, 0, nbytes);
                    input.Close();

                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.OutputStream.Flush();
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex);
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                Logger.WriteLog("Page not found:" + filename);
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            context.Response.OutputStream.Close();
        }
       
        #endregion
    }
}
