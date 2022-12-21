using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DataSheet", menuName = "ScriptableObj/DataSheet", order = int.MaxValue)]
public class DataSheet : ScriptableObject
{
     public int level = 0;
     public float hpf = 0;
     public int atk = 0;
     public int def = 0;
     public int speed = 0;
     public float atkSpeedf = 0;
     public float healf = 0;
     public int drop = 0;
     public int exp = 0;
}
