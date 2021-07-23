﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using Utility;

namespace Utility
{
    public class LogWriter : Singleton<LogWriter>
    {
        /// <summary>
        /// アセット下のログフォルダの場所
        /// </summary>
        /// <returns></returns>
        const string dirLocalPath = "Logs/";
        static List<string> logNameList
        {
            get
            {
                if (_logNameList == null)
                {

                    _logNameList = Directory.GetFiles(Application.dataPath + "/" + dirLocalPath, "*.txt").ToList();
                }

                return _logNameList;
            }
        }
        static List<string> _logNameList;

        static string dirPath { get { return Application.dataPath + "/" + dirLocalPath; } }
        string mainLog = null;



        protected override void Awake()
        {
            base.Awake();

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        /// <summary>
        /// 新しいlogを開始する。
        /// </summary>
        /// <param name="fileName">作るログの名前。入力しない場合はmainLogが作成される。</param>
        /// <returns>logのidを返す。すでに同名のファイルが作成されている場合は、新しくは作らず存在するもののidを返す</returns>
        public static int MakeLog(string fileName = null)
        {


            if (fileName == null)
            {
                if (instance.mainLog == null)
                {
                    instance.mainLog = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                }

                fileName = instance.mainLog;
            }
            else
            {
                if (Path.GetExtension(fileName) != ".txt")
                {
                    fileName = fileName + ".txt";
                }
            }

            var fullPath = dirPath + fileName;

            if (!File.Exists(fullPath))
            {
                File.Create(fullPath).Close();
                logNameList.Add(fileName);
            }

            return logNameList.IndexOf(fileName);
        }


        public static void Log(string log, int? id = null)
        {
            string logName;
            if (id == null)
            {
                if (instance.mainLog == null)
                {
                    MakeLog();
                }

                logName = instance.mainLog;
            }
            else
            {
                try
                {
                    logName = logNameList[id.Value];
                }
                catch (System.IndexOutOfRangeException)
                {
                    Debug.LogError("log of id" + id + " doesnt exist!");
                    return;
                }

            }


            Log(log, logName);
        }

        /// <summary>
        /// ファイル名を指定してログをつける
        /// </summary>
        /// <param name="log"></param>
        /// <param name="logName"></param>
        /// <param name="continue">trueの場合、日付をつけず、前の文に続ける</param>
        public static void Log(string log, string logName, bool continuous = false)
        {
            if (Path.GetExtension(logName) != ".txt")
            {
                logName = logName + ".txt";
            }

            if (!logNameList.Contains(logName))
            {
                MakeLog(logName);
            }

            var fullPath = dirPath + logName;

            if (!continuous)
            {
                log = DateTime.Now.ToString("hh:mm:ss") + "  " + log;
            }
            else
            {
                log = "          " + log;
            }

            var sw = new StreamWriter(fullPath, true);
            sw.WriteLine(log);
            sw.Flush();
            sw.Close();
        }

    }

    public class Logger
    {
        string _logName;
        string _senderName;
        public Logger(string logName, string sender)
        {
            _logName = logName;
            _senderName = sender;
        }

        public void Log(string log, [System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
        {
            LogWriter.Log(_senderName + " line:" + line + ": " + log, _logName);
        }

        public void LogLine(string log, [System.Runtime.CompilerServices.CallerLineNumber] int line = 0)
        {
            var format = string.Format("{0," + (_senderName.Count() + 2) + "}:{1}", line, log);
            LogWriter.Log(format, _logName, true);
        }

        public void LogError(string log, bool line = false, [System.Runtime.CompilerServices.CallerLineNumber] int lineNum = 0)
        {
            string formatted = _senderName + "line:" + lineNum + log;

            if (line)
            {
                formatted = string.Format("{0," + (_senderName.Count() + 2) + "}{1}", ">", log);
            }

            LogWriter.Log(formatted, _logName);
            ErrorLogger.Log(log, _senderName);
        }
    }


    public class ErrorLogger
    {
        public static void Log(String log, String name)
        {
            LogWriter.Log(name + ": " + log, "ERROR LOG");
        }
    }
}