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

    // for prefab
    GameObject myMonster = null;
    
    // Dictionary pooling 
    static Dictionary<string, List<GameObject>> pooling = new Dictionary<string, List<GameObject>>();


    private void Awake()
    {
        player = GameObject.FindObjectOfType<Playable>();

        MonsterSpawnTime = 2f;
        SpawnRange = 20f;
    }


    // Generate monster
    public void BringObject(string key)
    {
        GameObject newMonster = null;

        // circle의 random한 좌표값 생성 + spawn range 
        Vector3 randomPos = Random.insideUnitCircle.normalized * SpawnRange;

        // random한 좌표 + 현재 위치 
        randomPos += player.transform.position;
        randomPos.y = 0.5f;


        // pooling에 해당 key 값이 없다면 새 List 생성
        if (!pooling.ContainsKey(key))
        {

            // 새 리스트 생성
            List<GameObject> monster = new List<GameObject>();

            // 해당 key의 새로운 리스트 추가
            pooling.Add(key, monster);
        }


        // pool이 비어있다면 새로 생성
        if (pooling[key].Count == 0)
        {
            // key 값의 prefab 불러오기
            GameObject newKeyMonster = Resources.Load($"Prefabs_JSP\\{key}") as GameObject;

            // 생성 & list에 추가
            newMonster = Instantiate(newKeyMonster);
            Debug.Log("생성!");
        }

        else
        {
            foreach (GameObject monster in pooling[key])
            {
                // 비활성화된 오브젝트가 있다면
                if (!monster.activeSelf)
                {
                    monster.SetActive(true);
                    newMonster = monster;
                    pooling[key].Remove(monster);
                    break;
                }
            }
        }

        newMonster.transform.position = randomPos;
        newMonster.transform.rotation = Quaternion.identity;
        newMonster.name = newMonster.name.Replace("(Clone)", "");
    }



    // Destroy monster 
    public void DestroyMonster(GameObject monster)
    {
        string key = monster.name.ToString();

        monster.SetActive(false);

        pooling[key].Add(monster);
    }





    private void Start()
    {   
        // for test! 
        StartCoroutine(GenMonster(2f));
    }


    // for test! 
    IEnumerator GenMonster(float time)
    {
        yield return new WaitForSeconds(time);

        int count = 0;
        while (true)
        {

            if (count <= 4)
            {
                BringObject("temp_Monster1");
            }

            else if (count > 4)
            {
                BringObject("temp_Monster2");
            }

            count++;

            yield return new WaitForSeconds(2f);
        }

    }
}
