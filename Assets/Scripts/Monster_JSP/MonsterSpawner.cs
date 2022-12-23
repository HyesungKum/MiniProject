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

        // circle�� random�� ��ǥ�� ���� + spawn range 
        Vector3 randomPos = Random.insideUnitSphere.normalized;
        randomPos *= SpawnRange;

        // random�� ��ǥ + ���� ��ġ 
        randomPos += player.transform.position;
        randomPos.y = 0.5f;

        // ���� Ÿ�Կ� ����. �����Ǵ� ���� ��
        while (count > 0)
        {
            // pooling�� �ش� key ���� ���ٸ� �� List ����
            if (!pooling.ContainsKey(key))
            {

                // �� ����Ʈ ����
                List<GameObject> monster = new List<GameObject>();

                // �ش� key�� ���ο� ����Ʈ �߰�
                pooling.Add(key, monster);
            }


            // pool�� ����ִٸ� ���� ����
            if (pooling[key].Count == 0)
            {
                // key ���� prefab �ҷ�����
                GameObject newKeyMonster = Resources.Load($"Prefabs_JSP\\{key}") as GameObject;

                // ���� & list�� �߰�
                newMonster = Instantiate(newKeyMonster);
            }

            else
            {
                foreach (GameObject monster in pooling[key])
                {
                    // ��Ȱ��ȭ�� ������Ʈ�� �ִٸ�
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

            // ������ ������ Ÿ�� ���ϱ�
            switch (newMonster.name)
            {
                case "temp_Monster1":
                    newMonster.GetComponent<Monster>().SetMonsterType = MonsterType.temp1;
                    break;

                case "temp_Monster2":
                    {
                        newMonster.GetComponent<Monster>().SetMonsterType = MonsterType.temp2;

                        // �ϴ�.. ó�� �����Ǵ� ������Ʈ �߽����� �¿쿡 �� 8�� ����.. 
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
