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

        // circle�� random�� ��ǥ�� ���� + spawn range 
        Vector3 randomPos = Random.insideUnitCircle * SpawnRange;

        // random�� ��ǥ + ���� ��ġ 
        randomPos += player.transform.position;
        randomPos.y = 0.5f;


        // pooling�� �ش� key ���� ���ٸ� �� List ����
        if (pooling.ContainsKey(key.ToString()))
        {

            // �� ����Ʈ ����
            List<GameObject> monster = new List<GameObject>();

            // �ش� key�� ���ο� ����Ʈ �߰�
            pooling.Add(key.ToString(), monster);
        }


        // pool�� ����ִٸ� ���� ����
        if (pooling[key].Count == 0)
        {
            // key ���� prefab �ҷ�����
            GameObject newKeyMonster = Resources.Load($"Prefabs/{key.ToString()}") as GameObject;

            // ���� & list�� �߰�
            newMonster = Instantiate(myMonster);
            pooling[key].Add(newKeyMonster);
        }

        else
        {
            foreach (GameObject monster in pooling[key])
            {
                // ��Ȱ��ȭ�Ǿ��ٸ�
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

            // circle�� random�� ��ǥ�� ���� + spawn range 
            Vector3 randomPos = Random.insideUnitCircle * SpawnRange;

            // random�� ��ǥ + ���� ��ġ 
            randomPos += player.transform.position;
            randomPos.y = 0.5f;


            if (monsterPool.Count == 0)
            {
                newMonster = Instantiate(myMonster);

                newMonster.name = newMonster.name.Replace("(Clone)", "");
            }

            else
            {
                // pool���� �ϳ� ���� �´� 
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
