using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Animations;

public class Monster : MonoBehaviour
{

    // ���� Ÿ�� 
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

    // player ��ġȮ���� ����
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
        // HP�� 0�̵Ǹ� �ı� 
        if (MonsterHP <= 0)
        { 
            destroyMyself();
            return;
        }


        // ���� Ÿ�Կ� ���� �̵� �ӵ� �� ���� ����
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

    // ���͸� Ǯ�� �ִ´�
    void destroyMyself()
    {
        MonsterSpawner.Inst.DestroyMonster(this.gameObject);
    }


    // monster damage ����====================================================================
    private void OnCollisionEnter(Collision collision)
    {
        // weapon�� ����� ��� ���� ü�� ����
        if (collision.collider.tag == "Weapon")
        {
            //MonsterHP -= collision.gameObject.GetComponent<Weapon>().damage;
            MonsterHP--;
        }
    }

    // monster damage ����====================================================================

}
