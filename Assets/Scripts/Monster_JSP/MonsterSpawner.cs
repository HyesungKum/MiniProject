using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;
using UnityEditor;
using UnityEngine;
using static Monster;

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

    // test~!wjwklajslkjd

    [SerializeField] float MonsterSpawnTime;
    [SerializeField] float SpawnRange;

    Playable player = null;

    // for prefab
    GameObject myMonster = null;

    // Dictionary pooling 
    static Dictionary<string, List<GameObject>> pooling = new Dictionary<string, List<GameObject>>();

    int num;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<Playable>();

        MonsterSpawnTime = 2f;
        SpawnRange = 30f;
    }


    // Generate monster==============================================================================
    public void BringObject(string key)
    {
        float count = Mathf.Pow(num, num) * num;

        GameObject newMonster = null;

        // circle의 random한 좌표값 생성 + spawn range 
        Vector3 randomPos = Random.insideUnitSphere.normalized;
        randomPos *= SpawnRange;

        // random한 좌표 + 현재 위치 
        randomPos += player.transform.position;
        randomPos.y = 0.5f;

        // 몬스터 타입에 따라. 생성되는 몬스터 수
        while (count > 0)
        {
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
            newMonster.transform.LookAt(player.transform);

            newMonster.name = newMonster.name.Replace("(Clone)", "");

            // 생성된 몬스터의 타입 정하기
            switch (newMonster.name)
            {
                case "temp_Monster1":
                    newMonster.GetComponent<Monster>().SetMonsterType = MonsterType.temp1;
                    break;

                case "temp_Monster2":
                    {
                        newMonster.GetComponent<Monster>().SetMonsterType = MonsterType.temp2;

                        // 일단.. 처음 생성되는 오브젝트 중심으로 좌우에 총 8개 생성.. 
                        newMonster.transform.position += newMonster.transform.right * (count - 4f) * 2;
                    }
                    break;
            }

            count--;
        }
    }

    // Generate monster==============================================================================



    // Destroy monster==============================================================================
    public void DestroyMonster(GameObject monster)
    {
        // key 값 == 몬스터 이름
        string key = monster.name.ToString();

        // pooling에 해당 key 값이 없다면 새 List 생성
        if (!pooling.ContainsKey(key))
        {
            // 새 리스트 생성
            List<GameObject> newObjectList = new List<GameObject>();

            // 해당 key의 새로운 리스트 추가
            pooling.Add(key, newObjectList);
        }

        monster.SetActive(false);

        pooling[key].Add(monster);
    }
    // Destroy monster==============================================================================



    private void Start()
    {
        // for test! 
        StartCoroutine(GenMonster(2f));
    }


    // for test! 
    IEnumerator GenMonster(float time)
    {
        yield return new WaitForSeconds(time);

        while (true)
        {
            // 랜덤한 몬스터 타입이 나올 수 있도록. 
            num = Random.Range(1, 3);

            BringObject($"temp_Monster{num}");

            yield return new WaitForSeconds(2f);
        }

    }
}
