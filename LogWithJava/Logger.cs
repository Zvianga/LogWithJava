using System.Diagnostics;

namespace LogWithJava
{

    /// <summary>
    /// Class <c>Logger</c> logs messages passed to it.
    /// </summary>
    public static class Logger
    {
        public static HashSet<Sinks> LoggedSinks = new HashSet<Sinks>();
        public static Dictionary<Severity, string> FilePaths = new Dictionary<Severity, string>();

        static Logger()
        {
            FilePaths[Severity.Info] = "..//InfoLog.txt";
            FilePaths[Severity.Warning] = "..//WarningLog.txt";
        }

        /// <summary>
        /// Logs information
        /// </summary>
        /// <param name="message"></param>
        public static void LogInformation(string message)
        {
            Log(Severity.Info, message);
        }

        /// <summary>
        /// Logs warning
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarning(string message)
        {
            Log(Severity.Warning, message);
        }

        /// <summary>
        /// Logs during debugging only
        /// </summary>
        /// <param name="message"></param>
        public static void LogDebug(string message, Severity severity = Severity.Info)
        {
#if DEBUG
            Log(severity, message);
#endif
        }

        /// <summary>
        /// DEPRECATED
        /// </summary>
        /// <param name="message"></param>
        public static void DeprecatedLog(string message)
        {
            Console.WriteLine("I'm not here to log your message " + message);
        }
        private static void Log(Severity severity, string message)
        {
            try
            {
                if (message == null || message.Length == 0)
                    throw new Exception("Invalid logging message. Log cannot be empty.");
                foreach (var sink in LoggedSinks)
                {
                    switch (sink)
                    {
                        case Sinks.Console:
                            Console.WriteLine($"{DateTime.UtcNow} [{severity.ToString().ToUpper()}] {message}");
                            break;
                        case Sinks.File:
                            string path = FilePaths[severity];
                            if (!File.Exists(path))
                                File.Create(path).Close();
                            var file = File.AppendText(path);
                            file.WriteLine($"{DateTime.UtcNow} [{severity.ToString().ToUpper()}] {message}");
                            file.Close();

                            break;
                        case Sinks.Debug:
                            Debug.WriteLine($"{DateTime.UtcNow} [{severity.ToString().ToUpper()}] {message}");
                            break;
                    }
                }
            }
            catch(Exception ex) 
            {
                LogWarning(ex.Message);
            }
        }
    }

    public enum Sinks
    {
        Console,
        File,
        Debug
    }

    public enum Severity
    {
        Info,
        Warning
    }
}
