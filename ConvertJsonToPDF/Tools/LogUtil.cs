﻿using NLog;

namespace ConvertJsonToPDF.Tools
{
    public static class LogUtil
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void Trace(string message)
        {
            _logger.Trace(message);
        }

        public static void Info(string message)
        {
            _logger.Info(message);
        }

        public static void Debug(string message)
        {
            _logger.Debug(message);
        }

        public static void Warn(string message)
        {
            _logger.Warn(message);
        }

        public static void Error(string message)
        {
            _logger.Error(message);
        }
    }
}
