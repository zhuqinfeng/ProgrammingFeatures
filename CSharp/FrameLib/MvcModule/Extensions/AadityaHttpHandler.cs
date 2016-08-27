using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace MvcModule.Extensions
{
    public class AadityaHttpHandler : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public AadityaHttpHandler(RequestContext reqcon)
        {
            RequestContext = reqcon;
        }
        public bool IsReusable
        {
            get 
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write("Hello Aaditya!");
        }
    }
}