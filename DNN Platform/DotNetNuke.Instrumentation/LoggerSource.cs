
namespace DotNetNuke.Instrumentation
{
    public static class LoggerSource
    {
        private static ILoggerSource s_instance = new LoggerSourceImpl();

        public static ILoggerSource Instance
        {
            get { return s_instance; }
        }

        public static void SetTestableInstance(ILoggerSource loggerSource)
        {
            s_instance = loggerSource;
        }
    }
}