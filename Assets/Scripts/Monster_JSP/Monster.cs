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
            monsterSpeed = 10;
        }

        transform.Translate(monsterSpeed * Time.deltaTime * Vector3.forward);
    }

    // monster 주금 관련====================================================================

    void destroyMyself()
    {
        // 몬스터 사망 위치에 아이템 생성 
        // onDisable 때 아이템 생성 요청을 하면 오류 메세지 나옴 
        ObjectPool.Inst.BringObject("expItem").transform.position = this.transform.position;

        // 몬스터를 풀에 넣는다
        ObjectPool.Inst.DestroyObject(this.gameObject);
    }

    // monster damage 관련====================================================================

    public void Damage(float damage)
    {
        MonsterHP -= damage;
    }

}
