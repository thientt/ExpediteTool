using System;
using System.IO;
using System.Web;

namespace ExpediteTool.Web
{
    /// <summary>
    /// Summary description for DownloadFile
    /// </summary>
    public class DownloadFile : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string strFileName = context.Request.QueryString.Get("filename");
            string excelPath = System.IO.Path.Combine(context.Server.MapPath("~"), "Reports", strFileName);
            string fileHeader = String.Format("ExpediteLot_{0}.{1}", DateTime.Now.ToString("yyyyMMdd"), "xlsx");
            FileInfo file = new FileInfo(excelPath);
            if (file.Exists)
            {
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.ClearContent();
                context.Response.ClearHeaders();
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileHeader);
                context.Response.AddHeader("Content-Length", file.Length.ToString());
                context.Response.ContentType = "application/excel";
                context.Response.TransmitFile(file.FullName);
                context.Response.Flush();
            }
            File.Delete(excelPath);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}