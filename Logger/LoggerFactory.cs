using System;
using System.Collections.Generic;

namespace LoggerManager
{
    public class LoggerFactory
    {
        private static string callingPath = System.Reflection.Assembly.GetEntryAssembly().Location;
        private static Dictionary<Guid, Logger> cacheLoggers = new Dictionary<Guid, Logger>();
        private static Dictionary<string, Guid> logPaths = new Dictionary<string, Guid>();

        public static Logger GetInstance(LogLevel level = LogLevel.INFO)
        {
            string callingFileName = string.Format("{0}.log", callingPath);
            return GetInstance(callingFileName, level);
        }

        public static Logger GetInstance(string logPath, LogLevel level = LogLevel.INFO)
        {
            Logger log;
            if (CheckLogPath(logPath))
            {
                log = GetInstance(logPaths[logPath]);
            }
            else
            {
                log = new Logger(logPath, level);
                if (!cacheLoggers.ContainsKey(log.Id) && !logPaths.ContainsKey(log.FullName))
                {
                    cacheLoggers.Add(log.Id, log);
                    logPaths.Add(log.FullName, log.Id);
                }
                else
                {
                    throw new InvalidOperationException("New logger has same value in cache.");
                }
            }
            return log;
        }

        public static Logger GetInstance(Guid guid)
        {
            if (cacheLoggers.ContainsKey(guid))
            {
                return cacheLoggers[guid];
            }
            throw new InvalidOperationException("Logger didn't exist.");
        }

        private static bool CheckLogPath(string fullPath)
        {
            return logPaths.ContainsKey(fullPath);
        }
    }
}
