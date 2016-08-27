using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MvcModule.Extensions
{
    public class AadityaHttpModule : IHttpModule
    {
        private StreamWriter sw;
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += (new EventHandler(Application_BeginRequest));
        }
        private void Application_BeginRequest(object source, EventArgs e)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "logger.txt";
            if (!File.Exists(filePath))
            {
                sw = new StreamWriter(@"");
            }
            else
            {
                sw = File.AppendText(filePath);
            }
            sw.WriteLine("User sends request at {0}", DateTime.Now);
            sw.Close();
        }
    }
}