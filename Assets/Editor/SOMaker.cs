using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using System.Text;

public class SOMaker : EditorWindow
{
    static string FileName = null;

    static List<string> dataType = new ();
    static List<string> dataName = new ();
    static List<StringBuilder> data = new();

    static string savePath = null;

    static string GetGUID = null;

    [MenuItem("Tool/KHSOMaker")]
    static void Init()
    {
        SOMaker soMaker = (SOMaker)EditorWindow.GetWindow(typeof(SOMaker));
        soMaker.Show();

        soMaker.titleContent.text = "SOMaker";

        soMaker.minSize = new Vector2(340f, 100f);
        soMaker.maxSize = new Vector2(340f, 100f);
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Choose your scv data sheet");

        //read csv
        if (GUILayout.Button("Find CSV"))
        {
            string csvFilePath = EditorUtility.OpenFilePanel("", Directory.GetCurrentDirectory() + "DesignFolder", "csv");
            StreamReader reader = new(csvFilePath);

            FileName = csvFilePath.Split("/")[^1].Replace(".csv", "");

            DataParsing(reader);
        }

        if ( FileName != null && GUILayout.Button("Create SO"))
        {
            MakeSOClass(FileName);
        }
        
        if ( File.Exists(savePath + $"/{FileName}") && GUILayout.Button("Generate SO"))
        {
            MakeFile(GUID.Generate().ToString());
        }
        
        EditorGUILayout.EndHorizontal();
    }

    public void DataParsing(StreamReader readData)
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
                            sb.Append(dataType[i] + " " + dataName[i] + " " + token[i] + "f ");
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
    public void MakeSOClass(string name)
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
                    sb.AppendLine("     " + "public " + $"{dataType[i]}" + $" {dataName[i]}f" + " = 0;");
                    break;
                case "string":
                    sb.AppendLine("     " + "public " + $"{dataType[i]}" + $" \"{dataName[i]}\"" + " = \"\";");
                    break;
            }
        }
        sb.AppendLine("}");

        savePath = Directory.GetCurrentDirectory() + "\\Assets\\Scripts\\";

        StreamWriter outStream = File.CreateText(savePath + $"{name}.cs");

        outStream.Write(sb);
        outStream.Close();

        AssetDatabase.ImportAsset(savePath, ImportAssetOptions.ImportRecursive);
    }
    public void MakeFile(string guid)
    {
        for (int i = 0; i < data.Count; i++)
        {
            string[] args = data[i].ToString().Split(" ");
            
            StringBuilder sb = new StringBuilder();

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
            sb.AppendLine("  m_Script: { fileID: 11500000, guid: " + "7da5b8933b23aec49a2d1cf5ad3b0c4a" + ", type: 3}");
            sb.AppendLine($"  m_Name: {"name"}");
            sb.AppendLine("  m_EditorClassIdentifier:");
            for (int j = 3; j < args.Length - 3; j += 3)
            {
                switch (args[j])
                {
                    case "float"://float 예외처리
                        sb.AppendLine($"  {args[j + 1]}: {args[j + 2]}");
                        break;
                    default:
                        sb.AppendLine($"  {args[j + 1]}: {args[j + 2].Replace("f", "")}");
                        break;
                }
            }

            savePath = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\";

            Debug.Log(savePath);

            StreamWriter outStream = File.CreateText(savePath + $"{FileName}_{args[1]}_{args[2]}.asset");
            outStream.Write(sb);
            outStream.Close();
        }
    }
}
