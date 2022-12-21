using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Monster_Increase", menuName = "ScriptableObj/Monster_Increase", order = int.MaxValue)]
public class Monster_Increase : ScriptableObject
{
     public int monster_ID = 0;
     public int level = 0;
     public float hp = 0f;
     public int atk = 0;
     public float speed = 0f;
     public float atkspeed = 0f;
}
