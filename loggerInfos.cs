using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using System.Diagnostics;
using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System.Reflection;

using System.Net;
using System.Net.Mail;
using System.Configuration;


namespace PROJLogger
{
    
        public sealed class loggerInfo
        {
            public static int countOfError = 0;
            private static AutomationLogging _logger;
            public static AutomationLogging Instance
            {
                get
                {
                    if (_logger == null)
                    {
                        _logger = new AutomationLogging();
                    }

                    return _logger;
                }
            }
        }
        public class AutomationLogging
        {
            public static string currentGuid = string.Empty;
            public static string logfileName = string.Empty;
            public static string resultdirectory = string.Empty;
            public static Logger logger = LogManager.GetCurrentClassLogger();
            public static string _logFolder;
            public static string className = string.Empty;
            public static StringBuilder stringbuilder = null;
            public static List<string> list = new List<string>();
            public static int countOfError = 0;
            public static string newLocationInResultFolder = string.Empty;
            public AutomationLogging()
            {

                string resultFolder = @"C:/WDTF/TestResult";
                string asm = Assembly.GetCallingAssembly().FullName;
                string logFormat = string.Format("{0:yyyy-MM-dd-hh-mm-ss}", DateTime.Now);
                newLocationInResultFolder = resultFolder + "/" + currentGuid + "_" + logFormat;
                DirectoryInfo directoryInfo = new DirectoryInfo(newLocationInResultFolder);
                if (!directoryInfo.Exists)
                {
                    System.IO.Directory.CreateDirectory(newLocationInResultFolder);
                }
                var config = new LoggingConfiguration();


                //===========================================================================================//             
                var consoleTarget = new ColoredConsoleTarget();
                consoleTarget.Layout = "${time} | ${level}  | ${stacktrace::topFrames=2}|${message} ";
                config.AddTarget("console", consoleTarget);
                LoggingRule consoleInfo = new LoggingRule("*", LogLevel.Debug   , consoleTarget);
                config.LoggingRules.Add(consoleInfo);
                //===========================================================================================//
                var fileTarget = new FileTarget();
                fileTarget.Layout = "${time} | ${level}  | ${stacktrace:topFrames=2} | ${message} ";
                
                fileTarget.FileName = newLocationInResultFolder + "/" + className + "_" + logFormat + DateTime.Now.Second + ".log";
                config.AddTarget("file", fileTarget);
                var fileInfo = new LoggingRule("*", LogLevel.Debug, fileTarget);
                config.LoggingRules.Add(fileInfo);
                //===========================================================================================//
                TraceTarget traceTarget = new TraceTarget();
                traceTarget.Layout = "${time} | ${level}  | ${stacktrace:topFrames=2} | ${message} ";

                //===========================================================================================//
               
                //===========================================================================================//
                DatabaseTarget dbTarget = new DatabaseTarget();
                //===========================================================================================//

                // Step 4. Define rules
                LogManager.Configuration = config;
                Console.WriteLine(logger.IsDebugEnabled);
            }
            public void Debug(string Message)
            {
                logger.Debug(Message);
            }
            public void Info(string Message)
            {
                logger.Info(Message);
            }
            public void Warn(string Message)
            {
                logger.Warn(Message);
            }
            public void Error(string Message)
            {
                logger.Error(Message);
            }
            public void ErrorWithException(Exception e, string appLocation, LogLevel logLevel)
            {

                logger.Error("Error Message is: {0}", e.Message);
                countOfError += 1;
            }
            public void Message(string message)
            {
                logger.Info(message);
            }
            public void Email(string message)
            {
            }
        }

    
}
