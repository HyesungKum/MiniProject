using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Animations;

public class Monster : MonoBehaviour
{

    [SerializeField] MonsterType monsterType;
    float monsterSpeed;

    Playable player = null;

    string myName;
    Vector3 myDir;


    // enum type으로 한번 해봅시다! 
    enum MonsterType
    { 
        temp1,
        temp2
    }
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

        if (myName == "temp_Monster1")
        {
            this.transform.LookAt(player.transform);
            monsterSpeed = 7f;
        }

        else if (myName == "temp_Monster2")
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
