using System.Diagnostics;

namespace ForceField.Examples.Services
{
    public class LoggingService : ILoggingService
    {
        public void Log(string message)
        {
            Debug.WriteLine(message);
        }

        public void Log(System.Exception e)
        {
            Debug.WriteLine(e.GetType().Name + " : " + e.Message);
        }
    }
}