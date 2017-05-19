using System;
using System.IO;

namespace LoggerManager
{
    public class Logger
    {
        public enum LogLevel
        {
            DEBUG,
            INFO,
            WARNING,
            ERROR
        }


        private string callingPath = System.Reflection.Assembly.GetCallingAssembly().Location;
        public Guid Id { get; private set; }
        public LogLevel OutputLevel { get; private set; }
        public string LogPath { get; private set; }
        public string LogFileName { get; private set; }
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
                FullName = callingPath + fileName;
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

        private void GenerateFullName(string fileName)
        {
            this.FullName = LogPath + Path.DirectorySeparatorChar + fileName;
        }

        private string GenerateFullName()
        {
            string fileName;
            FileInfo info = new FileInfo(callingPath);
            fileName = string.Format("{0}.log", callingPath);
            return fileName;
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
