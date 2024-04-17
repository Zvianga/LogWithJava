using System.Diagnostics;

namespace LogWithJava
{
    public static class Logger
    {
        public static HashSet<Sinks> LoggedSinks = new HashSet<Sinks>();
        public static Dictionary<Severity, string> FilePaths = new Dictionary<Severity, string>();

        static Logger()
        {
            FilePaths[Severity.Info] = "..//InfoLog.txt";
            FilePaths[Severity.Warning] = "..//WarningLog.txt";
        }
        public static void LogInformation(string message)
        {
            Log(Severity.Info, message);
        }

        public static void LogWarning(string message)
        {
            Log(Severity.Warning, message);
        }
        public static void LogDebug(string message, Severity severity = Severity.Info)
        {
#if DEBUG
            Log(severity, message);
#endif
        }
        private static void Log(Severity severity, string message)
        {
            if (message == null || message.Length == 0) throw new Exception("Invalid logging message. Log cannot be empty.");
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
