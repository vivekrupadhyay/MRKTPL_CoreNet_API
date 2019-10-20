using System;
using System.Collections.Generic;
using System.Text;

namespace MRKTPL.Data.ViewModel
{
    public class Logging
    {
        public LogSetting LogLevel { get; set; }
    }
    public class LogSetting
    {
        public string Error { get; set; }
        public string Information { get; set; }
        public string Default { get; set; }
    }
}
