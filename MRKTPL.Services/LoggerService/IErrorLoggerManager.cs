using MRKTPL.Data.ViewModel;

namespace MRKTPL.LoggerService
{
    public interface IErrorLoggerManager
    {
        //void LogInfo(string message);
        //void LogWarn(string message);
        //void LogDebug(string message);
        string LogError();
    }
}
