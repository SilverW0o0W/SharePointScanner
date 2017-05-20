using System;
using System.IO;

namespace LoggerManager
{
    public enum LogLevel
    {
        DEBUG,
        INFO,
        WARNING,
        ERROR
    }
    public class Logger
    {
        private string callingPath = System.Reflection.Assembly.GetEntryAssembly().Location;
        public Guid Id { get; private set; }
        public LogLevel OutputLevel { get; private set; }
        public string FullName { get; private set; }

        public Logger(LogLevel level = LogLevel.INFO)
        {
            Id = Guid.NewGuid();
            OutputLevel = level;
            FullName = GenerateFullName();
        }

        public Logger(string fullName, LogLevel level = LogLevel.INFO)
        {
            Id = Guid.NewGuid();
            OutputLevel = level;
            if (string.IsNullOrEmpty(fullName))
            {
                fullName = GenerateFullName();
            }
            FullName = fullName;
        }

        public Logger(string fileName, bool isFullName, LogLevel level = LogLevel.INFO)
        {
            Id = Guid.NewGuid();
            OutputLevel = level;
            if (isFullName)
            {
                FullName = fileName;
            }
            else
            {
                FullName = GenerateFullName(fileName);
            }
        }

        //public bool SetLogPath(string path)
        //{
        //    bool result = false;
        //    if (!CheckPath(path))
        //    {
        //        return false;
        //    }
        //    if (string.IsNullOrEmpty(LogPath))
        //    {
        //        LogPath = path;
        //        //GenerateFullName();
        //        result = true;
        //    }
        //    else
        //    {
        //        lock (LogPath)
        //        {
        //            ResetLogPath(path);
        //        }
        //        //GenerateFullName();
        //        result = true;
        //    }
        //    return result;
        //}

        private bool CheckPath(string path)
        {
            bool result = false;
            try
            {
                result = string.IsNullOrEmpty(path);
                if (!result)
                {
                    DirectoryInfo info = new DirectoryInfo(path);
                    if (info.Exists)
                    {
                        result = true;
                    }
                    else
                    {
                        info.Create();
                        result = true;
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        //private void ResetLogPath(string path)
        //{
        //    this.Log(LogLevel.WARNING, "The log path is changed.New path: {0}.", path);
        //    LogPath = path;
        //}

        private string GetDirectoryPath(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            return info.Parent.FullName;
        }

        public string GenerateFullName(string fileName = null)
        {
            string fullName;
            FileInfo info = new FileInfo(callingPath);
            if (string.IsNullOrEmpty(fileName))
            {
                fullName = string.Format("{0}.log", callingPath);
            }
            else
            {
                fullName = string.Format("{0}{1}{2}", info.DirectoryName, Path.DirectorySeparatorChar, fileName);
            }
            return fullName;
        }

        public void ChangeOutputLevel(LogLevel level)
        {
            if (this.OutputLevel != level)
            {
                this.Warning("Change log level. Current level: {0}. New level: {1}.", this.OutputLevel, level);
                this.OutputLevel = level;
            }
        }

        #region output log
        public void Log(LogLevel level, string format, params object[] args)
        {
            if (level < this.OutputLevel)
            {
                return;
            }
            string message = string.Format("{0} {1} :{2}", level.ToString(), DateTime.Now.ToString("MM-dd HH:mm:ss,fff"), string.Format(format, args));
            using (StreamWriter writer = new StreamWriter(FullName, true))
            {
                writer.WriteLine(message);
            }
        }

        public void Debug(string format, params object[] args)
        {
            this.Log(LogLevel.DEBUG, format, args);
        }

        public void Info(string format, params object[] args)
        {
            this.Log(LogLevel.INFO, format, args);
        }

        public void Warning(string format, params object[] args)
        {
            this.Log(LogLevel.WARNING, format, args);
        }

        public void Error(string format, params object[] args)
        {
            this.Log(LogLevel.ERROR, format, args);
        }
        #endregion output log
    }
}