using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DataSheet", menuName = "ScriptableObj/DataSheet", order = int.MaxValue)]
public class DataSheet : ScriptableObject
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
    public int[] aa = new int[3];
    public List<int> aaList = new List<int>();
}
