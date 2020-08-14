using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Log : MonoBehaviour
{
    public enum LevelsOfLogs
    {
        INFO,
        WARNING,
        ERROR,
        FATAL
    }

    //static private string pathForLog = @"/storage/emulated/0/Swipe Game" + @"/Logs/";
    static private string pathForLog = Application.dataPath + @"/Logs/";
    static private string fileName;
    static private string path;

    static public void Setup()
    {
        fileName = DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss");
        if (!Directory.Exists(pathForLog))
            Directory.CreateDirectory(pathForLog);
        path = pathForLog + fileName + ".txt";
    }

    static public void WriteLog(string message, LevelsOfLogs tag, string place)
    {
        if (!File.Exists(path))
            CreateFile(path);
        using (StreamWriter sw = File.AppendText(path))
        {
            int sec = 0, min = 0, hours = 0;
            GetTime(ref sec, ref min, ref hours);
            string log = String.Format("{0,-8} [{1,-9} ", String.Format("[{0}.{1}.{2}]", hours, min, sec), tag + "]") + String.Format("{0,-35}", message) + String.Format(" [{0}]", place);
            sw.WriteLine(log);
        }
    }

    static private void CreateFile(string path)
    {
        // Create a file to write to.
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.WriteLine(String.Format("Program execution log {0}:", fileName));
            sw.WriteLine(String.Format("{0,-8}{1, 1}{2,-10}{3,-38}{4,-32}", " TIME", "", " TAG", " MESSAGE", "PLACE"));
        }
    }

    static private void GetTime(ref int sec, ref int min, ref int hours)
    {
        float t = Time.time;
        sec = (int)(t % 60);
        min = (int)((t / 60) % 60);
        hours = (int)((t / 3600) % 24);
    }
}