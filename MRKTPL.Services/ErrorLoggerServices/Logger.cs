using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MRKTPL.Services.ErrorLoggerServices
{
    public class Logger : ILogger
    {
        #region Fields
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string Exceptions { get; set; }
        public string Information { get; set; }
        public string LogType { get; set; }
        public string Message { get; set; }
        public string StracTrace { get; set; }
        public int StatusCode { get; set; }
        private log4net.ILog Log { get; set; }
        #endregion
        #region cTor
        public Logger()
        {
            Log = log4net.LogManager.GetLogger(typeof(Logger));
        }
        #endregion
        public void WriteLog()
        {
            //log4net.Config.XmlConfigurator.Configure();  
            if (LogType == "Info")
            {
                Info();
            }
            else if (LogType == "Error")
            {
                Error();
            }
        }
        private void Error()
        {
            JObject jobj = new JObject
            {
                { "LogDate", DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss") },
                { "MethodName", MethodName },
                { "Exceptions", Exceptions }
            };
            Log.InfoFormat(Convert.ToString(jobj).Replace("{", "{{").Replace("}", "}}"));
        }
        private void Info()
        {
            JObject jobj = new JObject
            {
                { "LogDate", DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss") },
                { "MethodName", MethodName },
                { "Exceptions", Exceptions }
            };
            Log.InfoFormat(Convert.ToString(jobj).Replace("{", "{{").Replace("}", "}}"));
        }
        public void ErrorLog(Exception ex)
        {
            System.Reflection.MethodBase mth = new StackTrace().GetFrame(1).GetMethod();
            ClassName = mth.ReflectedType.Name;
            MethodName = mth.Name;
            Information = ex.Message;
            LogType = "Error";
            WriteLog();
        }
        public void InfoLog(string Info)
        {
            System.Reflection.MethodBase mth = new StackTrace().GetFrame(1).GetMethod();
            ClassName = mth.ReflectedType.Name;
            MethodName = mth.Name;
            Information = Info;
            LogType = "Info";
            WriteLog();
        }
    }
}
