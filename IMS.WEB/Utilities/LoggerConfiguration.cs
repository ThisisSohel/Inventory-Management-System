using log4net;
using log4net.Config;

public static class LoggerConfiguration
{
    public static ILog GetLogger()
    {
        XmlConfigurator.Configure();
        return LogManager.GetLogger(typeof(LoggerConfiguration));
    }
}
