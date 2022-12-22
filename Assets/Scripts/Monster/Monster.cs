using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Playable player = null;

    [SerializeField] float monsterSpeed;

    void Awake()
    {
        // find player 
        player = GameObject.FindObjectOfType<Playable>();
        monsterSpeed = 5f;
    }

    private void Start()
    {
        // Destroy after 5 sec 
        Invoke("DestroyMyself", 10f);
    }

    void Update()
    {
        // chasing player 
        Vector3 dir = (player.transform.position - gameObject.transform.position).normalized;

        transform.Translate(monsterSpeed * Time.deltaTime * dir);

    }


    //void DestroyMyself()
    //{
    //    MonsterSpawner.Inst.DestroyMonster(this.gameObject);
    //}


}
