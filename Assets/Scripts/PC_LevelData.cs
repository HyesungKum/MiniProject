using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PC_LevelData", menuName = "ScriptableObj/PC_LevelData", order = int.MaxValue)]
public class PC_LevelData : ScriptableObject
{
     public int level = 0;
     public float hp = 0f;
     public int atk = 0;
     public int def = 0;
     public int speed = 0;
     public float atkSpeed = 0f;
     public float heal = 0f;
     public int drop = 0;
     public int exp = 0;
}
