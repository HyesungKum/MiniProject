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
    [SerializeField] int MonsterHP;
    [SerializeField] int monsterSpeed;


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
        Invoke("destroyMyself", 5f);
        transform.LookAt(player.transform.position);
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
            monsterSpeed = 7;
        }

        else if (mobType == MonsterType.temp2)
        {
            monsterSpeed = 20;
        }

        transform.Translate(monsterSpeed * Time.deltaTime * Vector3.forward);
    }

    // 몬스터를 풀에 넣는다
    void destroyMyself()
    {
        MonsterSpawner.Inst.DestroyMonster(this.gameObject);
    }


    // monster damage 관련====================================================================
    private void OnCollisionEnter(Collision collision)
    {
        // weapon에 닿았을 경우 몬스터 체력 감소
        if (collision.collider.tag == "Weapon")
        {
            //MonsterHP -= collision.gameObject.GetComponent<Weapon>().damage;
            MonsterHP--;
        }
    }

    // monster damage 관련====================================================================

}
