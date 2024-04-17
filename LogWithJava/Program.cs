namespace LogWithJava
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logger.LoggedSinks = new HashSet<Sinks> { Sinks.Console, Sinks.File, Sinks.Debug };
            Logger.LogInformation("Started logging");
            Logger.LogWarning("Got warning that next is exception");
            Logger.LogDebug("This will appear only if it's debugging");
            Logger.LogDebug(null);
        }
    }
}
