using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;

[CreateAssetMenu(fileName = "DataSheet", menuName = "ScriptableObj/DataSheet", order = int.MaxValue)]
public class SOMaker : EditorWindow
{
    static string csvFilePath = null;
    static StreamReader reader = null;

    [MenuItem("Tool/KHSOMaker")]
    static void Init()
    {
        SOMaker soMaker = (SOMaker)EditorWindow.GetWindow(typeof(SOMaker));
        soMaker.Show();

        soMaker.titleContent.text = "SOMaker";

        soMaker.minSize = new Vector2(340f, 50f);
        soMaker.maxSize = new Vector2(340f, 50f);
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Choose your scv data sheet");
        if (GUILayout.Button("Find SCV"))
        {
            csvFilePath = EditorUtility.OpenFilePanel("",Directory.GetCurrentDirectory() + "DesignFolder","csv");
            reader = new StreamReader(csvFilePath);

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                Debug.Log(line);
                string[] token = line.Split(',');
                for (int i = 0; i < token.Length; i++)
                {
                    Debug.Log(token[i]);
                }
            }
        }

        if (reader!=null && GUILayout.Button("Generate SO"))
        {
            //scriptable object »ç¿ë¹ý
        }
        
        EditorGUILayout.EndHorizontal();
    }

}
