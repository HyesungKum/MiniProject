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

    // for who call me? 
    GameObject caller = null;

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

        GameObject newObject = null;


        // pooling�� �ش� key ���� ���ٸ� �� List ����
        if (!pooling.ContainsKey(key))
        {
            // �� ����Ʈ ����
            List<GameObject> objectList = new List<GameObject>();

            // �ش� key�� ���ο� ����Ʈ �߰�
            pooling.Add(key, objectList);
        }

        // ���� ���� ��û�� ���! 
        if (key.Contains("Monster"))
        {
            // circle�� random�� ��ǥ�� ���� + spawn range 
            Vector3 randomPos = Random.insideUnitSphere.normalized;
            randomPos *= SpawnRange;

            // random�� ��ǥ + ���� ��ġ 
            randomPos += player.transform.position;
            randomPos.y = 0.5f;

            // ���� Ÿ�Կ� ����. �����Ǵ� ���� ��
            while (count > 0)
            {
                // pool�� ����ִٸ� ���� ����
                if (pooling[key].Count == 0)
                {
                    // key ���� prefab �ҷ�����
                    GameObject addObject = Resources.Load($"Prefabs_JSP\\{key}") as GameObject;

                    // ���� & list�� �߰�
                    newObject = Instantiate(addObject);
                }

                else
                {
                    foreach (GameObject objecyInPool in pooling[key])
                    {
                        // ��Ȱ��ȭ�� ������Ʈ�� �ִٸ�
                        if (!objecyInPool.activeSelf)
                        {
                            objecyInPool.SetActive(true);
                            newObject = objecyInPool;
                            pooling[key].Remove(objecyInPool);
                            break;
                        }
                    }
                }

                newObject.transform.position = randomPos;
                newObject.transform.LookAt(player.transform);

                newObject.name = newObject.name.Replace("(Clone)", "");

                // ������ ������ Ÿ�� ���ϱ�
                switch (newObject.name)
                {
                    case "temp_Monster1":
                        newObject.GetComponent<Monster>().SetMonsterType = MonsterType.temp1;
                        break;

                    case "temp_Monster2":
                        {
                            newObject.GetComponent<Monster>().SetMonsterType = MonsterType.temp2;

                            // �ϴ�.. ó�� �����Ǵ� ������Ʈ �߽����� �¿쿡 �� 8�� ����.. 
                            newObject.transform.position += newObject.transform.right * (count - 4f) * 2;
                        }
                        break;
                }

                count--;
            }
        }


        else if (key.Contains("Item"))
        {

            // pool�� ����ִٸ� ���� ����
            if (pooling[key].Count == 0)
            {
                // key ���� prefab �ҷ�����
                GameObject objectPrefab = Resources.Load($"Prefabs_JSP\\{key}") as GameObject;

                // ���� & list�� �߰�
                newObject = Instantiate(objectPrefab);
            }

            else
            {
                foreach (GameObject obejctInPool in pooling[key])
                {
                    // ��Ȱ��ȭ�� ������Ʈ�� �ִٸ�
                    if (!obejctInPool.activeSelf)
                    {
                        obejctInPool.SetActive(true);
                        newObject = obejctInPool;
                        pooling[key].Remove(obejctInPool);
                        break;
                    }
                }
            }

            newObject.transform.position = caller.transform.position;
        }



    }





    // Generate monster==============================================================================



    // Destroy monster==============================================================================
    public void DestroyMonster(GameObject monster)
    {
        // who call me? 
        caller = monster;

        // key �� == ���� �̸�
        string key = monster.name.ToString();

        // pooling�� �ش� key ���� ���ٸ� �� List ����
        if (!pooling.ContainsKey(key))
        {
            // �� ����Ʈ ����
            List<GameObject> newObjectList = new List<GameObject>();

            // �ش� key�� ���ο� ����Ʈ �߰�
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
            // ������ ���� Ÿ���� ���� �� �ֵ���. 
            num = Random.Range(1, 3);

            BringObject($"temp_Monster{num}");

            yield return new WaitForSeconds(2f);
        }

    }
}
