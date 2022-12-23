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
            // 랜덤한 몬스터 타입이 나올 수 있도록. 
            mobTypeNum = Random.Range(1, 3);

            // temp code, 군집 몬스터 8마리 소환을 위한 코드 
            float count = Mathf.Pow(mobTypeNum, mobTypeNum) * mobTypeNum;

            // circle의 random한 좌표값 생성 + spawn range 
            Vector3 randomPos = Random.insideUnitSphere.normalized;
            randomPos *= SpawnRange;

            // random한 좌표 + 타겟 위치 
            randomPos += player.transform.position;
            randomPos.y = 0.5f;

            // 몬스터 타입에 따라. 생성되는 몬스터 수
            while (count > 0)
            {
                // object pool에 몬스터 생성 요청
                GameObject newMob = ObjectPool.Inst.BringObject(null);

                newMob.transform.position = randomPos;
                newMob.transform.LookAt(player.transform);

                // 생성된 몬스터의 타입 정하기
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

                            // 일단.. 처음 생성되는 오브젝트 중심으로 좌우에 총 8개 생성.. 
                            newMob.transform.position += newMob.transform.right * (count - 4f) * 2;
                        }
                        break;
                }

                count--;
            }

            // 몬스터 스폰 시간 만큼 대기
            yield return new WaitForSeconds(MonsterSpawnTime);
        }

    }
    // Generate monster==============================================================================


}
