using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static LoggerManager.Logger;

namespace LoggerManager
{
    public class LoggerFactory
    {
        private static Dictionary<Guid, Logger> cacheLoggers = new Dictionary<Guid, Logger>();
        private static Dictionary<string, Guid> logPaths = new Dictionary<string, Guid>();

        public static Logger GetInstance(LogLevel level = LogLevel.INFO)
        {
            return GetInstance(string.Empty, level);
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
                cacheLoggers.Add(log.Id, log);
                logPaths.Add(log.FullName, log.Id);
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
