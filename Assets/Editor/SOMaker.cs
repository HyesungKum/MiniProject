using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Text;

public class SOMaker : EditorWindow
{
    /// <summary>
    /// so class file name
    /// </summary>
    static string FileName = null;

    /// <summary>
    /// parsing each data type
    /// </summary>
    static List<string> dataType = new ();
    /// <summary>
    /// parsing each data name
    /// </summary>
    static List<string> dataName = new ();
    /// <summary>
    /// parsing each combine data
    /// </summary>
    static List<StringBuilder> data = new();

    /// <summary>
    /// path used save data
    /// </summary>
    static string savePath = null;

    /// <summary>
    /// uniqe guid container
    /// </summary>
    static string CustomGUID = null;

    [MenuItem("Tool/KHSOMaker")]
    static void Init()
    {
        SOMaker soMaker = (SOMaker)EditorWindow.GetWindow(typeof(SOMaker));
        soMaker.Show();

        soMaker.titleContent.text = "SOMaker";

        soMaker.minSize = new Vector2(500f, 100f);
        soMaker.maxSize = new Vector2(500f, 100f);
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Choose your scv data sheet");

        //read csv
        if (GUILayout.Button("Find CSV"))
        {
            string csvFilePath = EditorUtility.OpenFilePanel("", Directory.GetCurrentDirectory() + "/DesignFolder/", "csv");

            if (!string.IsNullOrWhiteSpace(csvFilePath))
            {
                StreamReader reader = new(csvFilePath);

                FileName = csvFilePath.Split("/")[^1].Replace(".csv", "");

                DataParsing(reader, ref dataType, ref dataName, ref data);
                MakeSOClass(FileName, Directory.GetCurrentDirectory()+"/Assets/Scripts/DataTable/");
                AssetDatabase.Refresh();
            }
        }

        //read meta for guid
        if (GUILayout.Button("Find Meta"))
        {
            string metaFilePath = EditorUtility.OpenFilePanel("", Directory.GetCurrentDirectory() + "/Assets/Scripts/DataTable/", "meta");

            if (!string.IsNullOrWhiteSpace(metaFilePath))
            {
                StreamReader reader = new(metaFilePath);

                FileName = metaFilePath.Split("/")[^1].Replace(".cs.meta", "");

                string csvFilePath = Directory.GetCurrentDirectory() + "/DesignFolder/" + $"{FileName}.csv";
                StreamReader reReader = new(csvFilePath);

                DataParsing(reReader, ref dataType, ref dataName, ref data);
                CustomGUID = GetGUID(reader);
            }
        }

        if (CustomGUID !=null && GUILayout.Button("Create SO"))
        {
            MakeSOFile(CustomGUID, Directory.GetCurrentDirectory() + $"/Assets/Resources/DataTable/{FileName}/");

            //initialize
            dataName = new();
            dataType = new();
            data = new();

            FileName = null;
            savePath = null;
            CustomGUID = null;

            AssetDatabase.Refresh();
        }

        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// data parsing for editable string Builder list format
    /// </summary>
    /// <param name="readData"> need stream reader made by csv </param>
    /// <param name="dataType"> string list for record data type </param>
    /// <param name="dataName"> string list for record data name </param>
    /// <param name="data"> referance string builder data list </param>
    public void DataParsing(StreamReader readData, ref List<string> dataType, ref List<string> dataName, ref List<StringBuilder> data)
    {
        int j = 0;
        while (!readData.EndOfStream)
        {
            string line = readData.ReadLine();
            string[] token = line.Split(',');

            //data type
            if (j == 0)
            {
                for (int i = 0; i < token.Length; i++)
                {
                    dataType.Add(token[i]);
                }
            }
            //data name
            else if (j == 1)
            {
                for (int i = 0; i < token.Length; i++)
                {
                    dataName.Add(token[i]);
                }
            }
            //value parsing
            else
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < token.Length; i++)
                {
                    switch (dataType[i])
                    {
                        case "int":
                            sb.Append(dataType[i] + " " + dataName[i] + " " + token[i] + " ");
                            break;
                        case "float":
                            sb.Append(dataType[i] + " " + dataName[i] + " " + token[i] + " ");
                            break;
                        case "string":
                            sb.Append(dataType[i] + " " + dataName[i] + " " + token[i] + " ");
                            break;
                    }
                }

                data.Add(sb);
            }
            j++;
        }
    }

    /// <summary>
    /// Create Inheritance ScriptableObject Class
    /// </summary>
    /// <param name="name"> Class name </param>
    /// <param name="path"> Cs file path, if value = null -> default path Assets/Scripts </param>
    public void MakeSOClass(string name, string path)
    {
        StringBuilder sb = new();

        sb.AppendLine("using System.Collections;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine($"[CreateAssetMenu(fileName = \"{name}\", menuName = \"ScriptableObj/{name}\", order = int.MaxValue)]");
        sb.AppendLine($"public class {name} : ScriptableObject");

        sb.AppendLine("{");

        for (int i = 0; i < dataType.Count; i++)
        {
            switch (dataType[i])
            {
                case "int":
                    sb.AppendLine("     " + "public " + $"{dataType[i]}" + $" {dataName[i]}" + " = 0;");
                    break;
                case "float":
                    sb.AppendLine("     " + "public " + $"{dataType[i]}" + $" {dataName[i]}" + " = 0f;");
                    break;
                case "string":
                    sb.AppendLine("     " + "public " + $"{dataType[i]}" + $" \"{dataName[i]}\"" + " = \"\";");
                    break;
            }
        }
        sb.AppendLine("}");

        if (string.IsNullOrWhiteSpace(path))
        {
            savePath = Directory.GetCurrentDirectory() + "\\Assets\\Scripts\\";
        }
        else
        {
            if (Directory.Exists(path))
            {
                savePath = path;
            }
            else
            {
                Directory.CreateDirectory(path);
                savePath = path;
            }
        }


        StreamWriter outStream = File.CreateText(savePath + $"{name}.cs");

        outStream.Write(sb);
        outStream.Close();
    }

    /// <summary>
    /// get guid read meta file
    /// </summary>
    /// <param name="readMeta"> need streamReaded meta file </param>
    /// <returns> unique guid </returns>
    public string GetGUID(StreamReader readMeta)
    {
        string guid = null;
        int i = 0;

        while (i != 2)
        {
            if (i == 1)
            {
                guid = readMeta.ReadLine().Replace("guid: ", "");
            }
            else
            {
                readMeta.ReadLine();
            }

            i++;
        }

        return guid;
    }

    /// <summary>
    /// make scriptable object read csv and guid
    /// </summary>
    /// <param name="guid"> uniqe guid </param>
    /// <param name="path"> path for scriptable object file </param>
    public void MakeSOFile(string guid, string path)
    {
        for (int i = 0; i < data.Count; i++)
        {
            string[] args = data[i].ToString().Split(" ");

            StringBuilder sb = new();

            sb.AppendLine("%YAML 1.1");
            sb.AppendLine("%TAG !u! tag:unity3d.com,2011:");
            sb.AppendLine("--- !u!114 &11400000");
            sb.AppendLine("MonoBehaviour:");
            sb.AppendLine("  m_ObjectHideFlags: 0");
            sb.AppendLine("  m_CorrespondingSourceObject: {fileID: 0}");
            sb.AppendLine("  m_PrefabInstance: {fileID: 0}");
            sb.AppendLine("  m_PrefabAsset: {fileID: 0}");
            sb.AppendLine("  m_GameObject: {fileID: 0}");
            sb.AppendLine("  m_Enabled: 1");
            sb.AppendLine("  m_EditorHideFlags: 0");
            sb.AppendLine("  m_Script: { fileID: 11500000, guid: " + guid + ", type: 3}");
            sb.AppendLine($"  m_Name: {FileName}_{args[1]}_{args[2]}");
            sb.AppendLine("  m_EditorClassIdentifier:");
            for (int j = 0; j < args.Length - 1; j += 3)
            {
                switch (args[j])
                {
                    case "float"://float 예외처리
                        sb.AppendLine($"  {args[j + 1]}: {args[j + 2].Replace("f","")}");
                        break;
                    default:
                        sb.AppendLine($"  {args[j + 1]}: {args[j + 2]}");
                        break;
                }
            }


            if (string.IsNullOrWhiteSpace(path))
            {
                savePath = Directory.GetCurrentDirectory() + "\\Assets\\Scripts\\";
            }
            else
            {
                if (Directory.Exists(path))
                {
                    savePath = path;
                }
                else
                {
                    Directory.CreateDirectory(path);
                    savePath = path;
                }
            }

            StreamWriter outStream = File.CreateText(savePath + $"{FileName}_{args[1]}_{args[2]}.asset");
            outStream.Write(sb);
            outStream.Close();
        }
    }
}
