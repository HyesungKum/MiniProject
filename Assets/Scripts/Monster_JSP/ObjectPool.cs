using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Monster;

public class ObjectPool : MonoBehaviour
{
    #region Singleton
    static ObjectPool inst = null;

    static public ObjectPool Inst
    {
        get
        {
            if (inst == null)
            {
                inst = GameObject.FindObjectOfType<ObjectPool>();

                if (inst == null)
                {
                    inst = new GameObject("ObjectPool").AddComponent<ObjectPool>();
                }
            }
            return inst;
        }
    }
    #endregion


    // Dictionary pooling 
    static Dictionary<string, List<GameObject>> pooling = new Dictionary<string, List<GameObject>>();


    public GameObject BringObject(GameObject targetObject)
    {
        // key 값 == 오프젝트 이름
        string key = targetObject.name.ToString();

        GameObject newObject = null;

        // pooling에 해당 key 값이 없다면 새 List 생성
        if (!pooling.ContainsKey(key))
        {
            // 새 리스트 생성
            List<GameObject> objectList = new List<GameObject>();

            // 해당 key의 새로운 리스트 추가
            pooling.Add(key, objectList);
        }

        // pool이 비어있다면 새로 생성
        if (pooling[key].Count == 0)
        {
            // 생성 & list에 추가
            newObject = Instantiate(targetObject);
        }

        else
        {
            foreach (GameObject obejctInPool in pooling[key])
            {
                // 비활성화된 오브젝트가 있다면
                if (!obejctInPool.activeSelf)
                {
                    obejctInPool.SetActive(true);
                    newObject = obejctInPool;
                    pooling[key].Remove(obejctInPool);
                    break;
                }
            }
        }

        newObject.name = newObject.name.Replace("(Clone)", "");
        return newObject;
    }

    // Back to pool ==============================================================================
    public void DestroyObject(GameObject destroyObject)
    {
        // key 값 == 오프젝트 이름
        string key = destroyObject.name.ToString();

        // pooling에 해당 key 값이 없다면 새 List 생성
        if (!pooling.ContainsKey(key))
        {
            // 새 리스트 생성
            List<GameObject> newObjectList = new List<GameObject>();

            // 해당 key의 새로운 리스트 추가
            pooling.Add(key, newObjectList);
        }

        destroyObject.SetActive(false);

        pooling[key].Add(destroyObject);
    }
    // Back to pool ==============================================================================

}
