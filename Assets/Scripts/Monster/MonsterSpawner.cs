using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{

    #region Singleton
    static MonsterSpawner inst = null;

    static public MonsterSpawner Inst
    {
        get
        {
            if (inst == null)
            {
                inst = GameObject.FindObjectOfType<MonsterSpawner>();

                if (inst == null)
                {
                    inst = new GameObject("MonsterSpawner").AddComponent<MonsterSpawner>();
                }
            }
            return inst;
        }
    }
    #endregion


    [SerializeField] float MonsterSpawnTime;
    [SerializeField] float SpawnRange;

    Playable player = null;


    GameObject myMonster = null;
    Queue<GameObject> monsterPool = new Queue<GameObject>();


    // Dictionary pooling 
    Dictionary<string, List<GameObject>> pooling = new Dictionary<string, List<GameObject>>();






    private void Awake()
    {
        player = GameObject.FindObjectOfType<Playable>();

        MonsterSpawnTime = 2f;
        SpawnRange = 20f;

        myMonster = Resources.Load("temp_monster", typeof(GameObject)) as GameObject;
    }


    // Generate monster
    public void BringObject(string key)
    {
        GameObject newMonster = null;

        // circle의 random한 좌표값 생성 + spawn range 
        Vector3 randomPos = Random.insideUnitCircle * SpawnRange;

        // random한 좌표 + 현재 위치 
        randomPos += player.transform.position;
        randomPos.y = 0.5f;


        // pooling에 해당 key 값이 없다면 새 List 생성
        if (pooling.ContainsKey(key.ToString()))
        {

            // 새 리스트 생성
            List<GameObject> monster = new List<GameObject>();

            // 해당 key의 새로운 리스트 추가
            pooling.Add(key.ToString(), monster);
        }


        // pool이 비어있다면 새로 생성
        if (pooling[key].Count == 0)
        {
            // key 값의 prefab 불러오기
            GameObject newKeyMonster = Resources.Load($"Prefabs/{key.ToString()}") as GameObject;

            // 생성 & list에 추가
            newMonster = Instantiate(myMonster);
            pooling[key].Add(newKeyMonster);
        }

        else
        {
            foreach (GameObject monster in pooling[key])
            {
                // 비활성화되었다면
                if (!monster.activeSelf)
                {
                    monster.SetActive(true);
                    newMonster = monster;
                }
            }
        }


        newMonster.transform.position = randomPos;
        newMonster.transform.rotation = Quaternion.identity;
        newMonster.name = newMonster.name.Replace("(Clone)", "");
    }



    // Destroy monster 












    // === for test ===

/*
    IEnumerator GenerateMonster(float delayTime)
    {

        yield return new WaitForSeconds(delayTime);

        while (true)
        {
            GameObject newMonster = null;

            // circle의 random한 좌표값 생성 + spawn range 
            Vector3 randomPos = Random.insideUnitCircle * SpawnRange;

            // random한 좌표 + 현재 위치 
            randomPos += player.transform.position;
            randomPos.y = 0.5f;


            if (monsterPool.Count == 0)
            {
                newMonster = Instantiate(myMonster);

                newMonster.name = newMonster.name.Replace("(Clone)", "");
            }

            else
            {
                // pool에서 하나 꺼내 온다 
                newMonster = monsterPool.Dequeue();
                newMonster.SetActive(true);
            }

            newMonster.transform.position = randomPos;
            newMonster.transform.rotation = Quaternion.identity;
            yield return new WaitForSeconds(MonsterSpawnTime);
        }
    }


    public void DestroyMonster(GameObject monster)
    {
        monster.SetActive(false);
        monsterPool.Enqueue(monster);
    }

    */
}
