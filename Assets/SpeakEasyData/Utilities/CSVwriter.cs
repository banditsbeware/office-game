using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class CSVwriter
{
    static StreamWriter sw;
    public static string fileName;
    private static string tablesFolder = "/SpeakEasyData/Tables/";  //appended to Application.dataPath, which when called in the editor is the Assets folder

    public static void OpenCSV(string name)
    {
        fileName = Application.dataPath + tablesFolder + name;
        
        if (!File.Exists(fileName))
        {
            File.Create(fileName).Close();
        }

        sw = new StreamWriter(fileName, false);
        sw.WriteLine("character, cue, inflection, node name, implemented, recorded");
        sw.Close();

        sw = new StreamWriter(fileName, true);
    }

    public static void CloseCSV()
    {
        sw.Close();
    }
    
    public static void WriteCSV(string line)
    {
        sw.WriteLine(line);
    }
}
