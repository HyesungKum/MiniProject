#define PC
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KHS_Axis;
using Unity.VisualScripting;

public class Playable : MonoBehaviour
{
    //======================debug controll===================================
#if UNITY_EDITOR
    [field : SerializeField] bool DebugConsole { get; set; }
#endif
    //======================input mode ========================================
    enum PlayMode
    {
        Touch,
        Slide
    }
    [field: SerializeField] PlayMode playMode { get; set; }

    //==========================level data sheet==============================
    [Header("Parameter")]
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
    [Header("Weapon")]
    [SerializeField] GameObject weaponPrefabs = null;

    [Header("Getting Range")]
    [SerializeField] private AbsorbRange AbsorbRange = null;

    //==============================inner variables===========================
    private Vector3 moveDir = Vector3.forward;
    private float AttackTimer = 0f;
    private float HitTimer = 0f;


    private void Awake()
    {
        SetPCParameter(1);
        curExp      = 0 ;
    }

    void Update()
    {
        MoveCtl();
        ExpCtl();
        AttackCtl();
        HealthCtl();

        #region debug
#if UNITY_EDITOR
        if (DebugConsole)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                if (curLevel < LevelData.Length - 1)
                    curLevel++;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (curLevel > 1)
                    curLevel--;
            }

            Debug.Log($" level    : {LevelData[curLevel - 1].level} ");
            Debug.Log($" hp       : {curHp}/{LevelData[curLevel - 1].hp}");
            Debug.Log($" atk      : {LevelData[curLevel - 1].atk}");
            Debug.Log($" def      : {LevelData[curLevel - 1].def}");
            Debug.Log($" speed    : {LevelData[curLevel - 1].speed}");
            Debug.Log($" atkSpeed : {LevelData[curLevel - 1].atkSpeed}");
            Debug.Log($" heal     : {LevelData[curLevel - 1].heal}");
            Debug.Log($" drop     : {LevelData[curLevel - 1].drop}");
            Debug.Log($" Exp      : {curExp}/{LevelData[curLevel - 1].maxExp}");
            Debug.Log($"=================================================");
        }
        #endif
        #endregion
    }
    //======================== controll====================================
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
    public void MoveCtl()
    {
        Vector3 moveSpeed = Vector3.zero;

        #region PC Controll
        #if PC
        if (Input.GetMouseButton(0))
        {
            if (playMode == PlayMode.Touch)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.Log(hit.collider.name);
                    if (hit.collider.name == "RayBlock") return;

                    moveDir = new Vector3((hit.point.x - this.transform.position.x), 0f, (hit.point.z - this.transform.position.z)).normalized;
                    moveSpeed = LevelData[curLevel - 1].speed * this.transform.forward;
                }
            }
            else if (playMode == PlayMode.Slide)
            {
                if (KHS_Axis.Axis.GetHorizontal != 0 || KHS_Axis.Axis.GetVertical != 0)
                {
                    float x = KHS_Axis.Axis.GetHorizontal;
                    float z = KHS_Axis.Axis.GetVertical;

                    moveDir = new Vector3(-x, 0f, -z).normalized;
                    moveSpeed = LevelData[curLevel - 1].speed * this.transform.forward;
                }
            }
        }
        #endif
        #endregion
        this.transform.right = moveDir;
        this.transform.localPosition += moveSpeed * Time.deltaTime;

        #region Mobile Controll
#if Moblie
        if (Input.GetMouseButton(0))
        {
        }
#endif
        #endregion
    }
    public void ExpCtl() //need call event 
    {
        if (LevelData[curLevel].maxExp == 0) return;

        if (curExp >= LevelData[curLevel-1].maxExp)
        {
            curLevel++;

            SetPCParameter(curLevel);

            curExp = 0;
        }
    }
    public void DropRangeCtl()
    {
        AbsorbRange.Collider.radius = curDrop;
    }
    public void AttackCtl()
    {
        if (Timer(ref AttackTimer, 1f))
        {
            Instantiate(weaponPrefabs, this.transform.position, Quaternion.LookRotation(moveDir));
            //오브젝트 풀링 반환 형태 필요
        }
    }
    public void HealthCtl()
    {
        if (curHp <= 0) MonsterSpawner.Inst.DestroyMonster(this.gameObject);
    }

    //=======================Inner function===========================
    /// <summary>
    /// timer for interval true return
    /// </summary>
    /// <param name="timeSource"> using timer </param>
    /// <param name="refreshTime"> refresh time limit </param>
    /// <returns>if timer value has higher than refresh time will be ture</returns>
    public bool Timer(ref float timeSource, float refreshTime)
    {
        timeSource += Time.deltaTime;
        if (timeSource >= refreshTime)
        {
            timeSource = 0f;
            return true;
        }
        else
        {
            return false;
        }
    }

    //========================Message==================================
    public void GetExp(int value)
    {
        curExp += value;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.name.Split("Monster").Length == 2)
        {
            if (Timer(ref HitTimer, 1f))
                curHp -= 10;
        }
    }

}
