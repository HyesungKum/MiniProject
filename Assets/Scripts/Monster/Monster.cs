using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] float monsterSpeed;
    
    Playable player = null;

    string myName;


    void Awake()
    {
        // find player 
        player = GameObject.FindObjectOfType<Playable>();
        monsterSpeed = 5f;
        myName = this.gameObject.name.Replace("(Clone)", "");
    }

    private void Start()
    {
        // for test, destroy myself after 5 sec
        Invoke("DestroyMyself", 5f);

        transform.LookAt(player.transform.position);

    }

    void Update()
    {
        // chasing player 
        Vector3 dir = (player.transform.position - gameObject.transform.position).normalized;
        transform.Translate(monsterSpeed * Time.deltaTime * dir);


        transform.Translate(monsterSpeed * Time.deltaTime * Vector3.forward);

    }


    void DestroyMyself()
    {
        MonsterSpawner.Inst.DestroyMonster(this.gameObject);
    }


}
