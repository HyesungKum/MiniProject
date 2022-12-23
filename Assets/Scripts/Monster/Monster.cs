using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Animations;

public class Monster : MonoBehaviour
{

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

        if (MonsterHP <= 0)
        {
            destroyMyself();
            return;
        }



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

    // monster destroy ====================================================================

    void destroyMyself()
    {

        ObjectPool.Inst.BringObject(expItem).transform.position = this.transform.position;
        ObjectPool.Inst.DestroyObject(this.gameObject);
    }

    // monster damage ====================================================================

    public void Damage(float damage)
    {
        MonsterHP -= damage;
    }

}
