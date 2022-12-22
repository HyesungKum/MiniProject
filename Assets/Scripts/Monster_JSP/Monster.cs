using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Animations;

public class Monster : MonoBehaviour
{

    // 몬스터 타입 
    [SerializeField] MonsterType mobType;

    public MonsterType SetMonsterType { set { mobType = value; } }

    public enum MonsterType
    {
        temp1,
        temp2
    }

    float monsterSpeed;

    Playable player = null;

    string myName;
    Vector3 myDir;



    void Awake()
    {
        // find player 
        player = GameObject.FindObjectOfType<Playable>();
        myName = this.gameObject.name.Replace("(Clone)", "");
    }


    private void OnEnable()
    {
        // for test! disable after 5 sec
        Invoke("destroyMyself", 5f);
        transform.LookAt(player.transform.position);
    }

    void Update()
    {

        if (mobType == MonsterType.temp1)
        {
            this.transform.LookAt(player.transform);
            monsterSpeed = 7f;
        }

        else if (mobType == MonsterType.temp2)
        {
            monsterSpeed = 20f;
        }

        transform.Translate(monsterSpeed * Time.deltaTime * Vector3.forward);
    }



    void destroyMyself()
    {
        MonsterSpawner.Inst.DestroyMonster(this.gameObject);
    }


}
