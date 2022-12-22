#define PC
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playable : MonoBehaviour
{
    [SerializeField] PC_LevelData[] LevelData = null;
    //======================current player data scope==========================
    [field: SerializeField] int curLevel { get; set; }
    [field : SerializeField] float curHp { get; set; }
    [field: SerializeField] int curAtk { get; set; }
    [field: SerializeField] int curDef { get; set; }
    [field: SerializeField] int curSpeed { get; set; }
    [field: SerializeField] float curAtkSpeed { get;set; }
    [field: SerializeField] float curHeal { get; set; }
    [field: SerializeField] int curDrop { get; set; }
    [field: SerializeField] int curExp { get; set; }
    //=========================================================================

    [SerializeField] private AbsorbRange AbsorbRange = null;

    private Vector3 moveDir = Vector3.forward;

    private void Awake()
    {
        SetPCParameter(1);
        curExp      = 0 ;
    }

    void Update()
    {
        MoveControll();
        ExpControll();

        #region debug
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (curLevel < LevelData.Length-1)
                curLevel++;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if(curLevel>1)
                curLevel--;
        }

        Debug.Log($" level    : {LevelData[curLevel - 1].level} "          );
        Debug.Log($" hp       : {curHp}/{LevelData[curLevel - 1].hp}"      );
        Debug.Log($" atk      : {LevelData[curLevel - 1].atk}"             );
        Debug.Log($" def      : {LevelData[curLevel - 1].def}"             );
        Debug.Log($" speed    : {LevelData[curLevel - 1].speed}"           );
        Debug.Log($" atkSpeed : {LevelData[curLevel - 1].atkSpeed}"        );
        Debug.Log($" heal     : {LevelData[curLevel - 1].heal}"            );
        Debug.Log($" drop     : {LevelData[curLevel - 1].drop}"            );
        Debug.Log($" Exp      : {curExp}/{LevelData[curLevel - 1].maxExp}" );
        Debug.Log($"================================================="     );
        #endregion
    }
    //========================parameter controll====================================
    /// <summary>
    /// setting pc parameter used level
    /// setting upper level out of index, will be return this function
    /// </summary>
    /// <param name="level"> used level 1 ~ max</param>
    public void SetPCParameter(int level)
    {
        level--;

        if (LevelData[level] == null) return;

        curLevel    = LevelData[level].level;
        curHp       = LevelData[level].hp;
        curAtk      = LevelData[level].atk;
        curDef      = LevelData[level].def;
        curSpeed    = LevelData[level].speed;
        curAtkSpeed = LevelData[level].atkSpeed;
        curHeal     = LevelData[level].heal;
        curDrop     = LevelData[level].drop;
    }
    public void MoveControll()
    {
        Vector3 moveSpeed = Vector3.zero;

        #region PC Controll
        #if PC
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.name == "RayBlock") return;

                moveDir = new Vector3((hit.point.x - this.transform.position.x),0f, (hit.point.z - this.transform.position.z)).normalized;
                moveSpeed = LevelData[curLevel - 1].speed * this.transform.forward;
            }
        }
        #endif
        #endregion
        this.transform.forward = moveDir;
        this.transform.localPosition += moveSpeed * Time.deltaTime;

        #region Mobile Controll
#if Moblie
        if (Input.GetMouseButton(0))
        {
        }
#endif
        #endregion
    }
    public void ExpControll() //need call event 
    {
        if (LevelData[curLevel].maxExp == 0) return;

        if (curExp >= LevelData[curLevel-1].maxExp)
        {
            curLevel++;

            SetPCParameter(curLevel);

            curExp = 0;
        }
    }
    public void DropRangeControll()
    {
        AbsorbRange.Collider.radius = curDrop;
    }

    //========================Message==================================
    public void GetExp(int value)
    {
        curExp += value;
    }
}
