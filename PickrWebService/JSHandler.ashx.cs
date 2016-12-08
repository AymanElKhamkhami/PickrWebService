using JSONWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace PickrWebService
{
    /// <summary>
    /// Summary description for JSHandler
    /// </summary>
    public class JSHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            ServiceAPI pickrWebService = new ServiceAPI();
            var response = new Result();

            string method = context.Request.QueryString["method"].ToString();

            switch(method)
            {
                
                case "CheckUserExistence":
                    response.Check = pickrWebService.CheckUserExistence(context.Request.QueryString["Email"].ToString());
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write(jsonSerializer.Serialize(response));
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public class Result
        {
            public bool Check { get; set; }
        }  
    }
}