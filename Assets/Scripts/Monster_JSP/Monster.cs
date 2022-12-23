using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Animations;

public class Monster : MonoBehaviour
{

    // 몬스터 타입 
    [Header("Monster Info")]
    [SerializeField] MonsterType mobType;
    [SerializeField] float MonsterHP;
    [SerializeField] int monsterSpeed;

    [SerializeField] GameObject expItem;

    public MonsterType SetMonsterType { set { mobType = value; } }

    public enum MonsterType
    {
        temp1,
        temp2 = 8
    }

    // player 위치확인을 위해
    Playable player = null;


    void Awake()
    {
        // find player 
        player = GameObject.FindObjectOfType<Playable>();
    }

    private void OnEnable()
    {
        // for test! disable after 5 sec
        //Invoke("destroyMyself", 5f);
        transform.LookAt(player.transform.position);

        MonsterHP = 3;
    }

    void Update()
    {
        // HP가 0이되면 파괘 
        if (MonsterHP <= 0)
        {
            destroyMyself();
            return;
        }


        // 몬스터 타입에 따른 이동 속도 및 방향 설정
        if (mobType == MonsterType.temp1)
        {
            this.transform.LookAt(player.transform);
            monsterSpeed = 2;
        }

        else if (mobType == MonsterType.temp2)
        {
            monsterSpeed = 5;
        }

        transform.Translate(monsterSpeed * Time.deltaTime * Vector3.forward);
    }

    // 몬스터를 풀에 넣는다
    void destroyMyself()
    {
        MonsterSpawner.Inst.DestroyMonster(this.gameObject);
    }

    private void OnDisable()
    {
        MonsterSpawner.Inst.BringObject("expItem");
    }


    // monster damage 관련====================================================================

    public void Damage(float damage)
    {
        MonsterHP -= damage;
    }

    // monster damage 관련====================================================================

}
