using System;

namespace ForceField.Examples.Services
{
    public interface ILoggingService
    {
        void Log(string message);
        void Log(Exception e);
    }
}