using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;
using UnityEditor;
using UnityEngine;
using static Monster;

public class MonsterSpawner : MonoBehaviour
{

    [SerializeField] float MonsterSpawnTime;
    [SerializeField] float SpawnRange;

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
            // ������ ���� Ÿ���� ���� �� �ֵ���. 
            mobTypeNum = Random.Range(1, 3);

            // temp code, ���� ���� 8���� ��ȯ�� ���� �ڵ� 
            float count = Mathf.Pow(mobTypeNum, mobTypeNum) * mobTypeNum;

            // circle�� random�� ��ǥ�� ���� + spawn range 
            Vector3 randomPos = Random.insideUnitSphere.normalized;
            randomPos *= SpawnRange;

            // random�� ��ǥ + Ÿ�� ��ġ 
            randomPos += player.transform.position;
            randomPos.y = 0.5f;

            // ���� Ÿ�Կ� ����. �����Ǵ� ���� ��
            while (count > 0)
            {
                // object pool�� ���� ���� ��û
                GameObject newMob = ObjectPool.Inst.BringObject(null);

                newMob.transform.position = randomPos;
                newMob.transform.LookAt(player.transform);

                // ������ ������ Ÿ�� ���ϱ�
                switch (mobTypeNum)
                {
                    // type 1 mob
                    case 1:
                        newMob.GetComponent<Monster>().SetMonsterType = MonsterType.temp1;
                        break;

                    // type 2 mob
                    case 2:
                        {
                            newMob.GetComponent<Monster>().SetMonsterType = MonsterType.temp2;

                            // �ϴ�.. ó�� �����Ǵ� ������Ʈ �߽����� �¿쿡 �� 8�� ����.. 
                            newMob.transform.position += newMob.transform.right * (count - 4f) * 2;
                        }
                        break;
                }

                count--;
            }

            // ���� ���� �ð� ��ŭ ���
            yield return new WaitForSeconds(MonsterSpawnTime);
        }

    }
    // Generate monster==============================================================================


}
