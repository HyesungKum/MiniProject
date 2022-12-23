using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MonsterSpawner : MonoBehaviour
{

    [SerializeField] float MonsterSpawnTime;
    [SerializeField] float SpawnRange;
    [SerializeField]
    GameObject[] monsters;

    Playable player = null;

    int mobTypeNum;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<Playable>();

        MonsterSpawnTime = 2f;
        SpawnRange = 30f;
    }


    private void Start()
    {
        // for test! 
        StartCoroutine(GenMonster(2f));
    }


    // Generate monster==============================================================================
    IEnumerator GenMonster(float time)
    {
        yield return new WaitForSeconds(time);

        while (true)
        {
            // to gen random monster type
            mobTypeNum = Random.Range(0, 2);

            Debug.Log(monsters[mobTypeNum].name);

            // to gen multiple mobs
            float count = Mathf.Pow(mobTypeNum + 1, 3);

            // get random point around player 
            Vector3 randomPos = Random.insideUnitSphere.normalized;
            randomPos *= SpawnRange;

            // random pos + target pos
            randomPos += player.transform.position;
            randomPos.y = 0.5f;

            while (count > 0)
            {

                GameObject newMob = null;

                // set monster type
                switch (mobTypeNum)
                {
                    // type 1 mob
                    case 0:
                        newMob = ObjectPool.Inst.BringObject(monsters[mobTypeNum]);
                        newMob.GetComponent<Monster>().SetMonsterType = Monster.MonsterType.temp1;
                        newMob.transform.position = randomPos;
                        newMob.transform.LookAt(player.transform);
                        break;

                    // type 2 mob
                    case 1:
                        {
                            newMob = ObjectPool.Inst.BringObject(monsters[mobTypeNum]);
                            newMob.GetComponent<Monster>().SetMonsterType = Monster.MonsterType.temp2;
                            newMob.transform.position = randomPos;
                            newMob.transform.LookAt(player.transform);
                            newMob.transform.position += newMob.transform.right * (count * 1.5f - 4f) * 2;
                        }
                        break;
                }

                count--;
            }

            // wait for spawn interval 
            yield return new WaitForSeconds(MonsterSpawnTime);
        }

    }
    // Generate monster==============================================================================


}
