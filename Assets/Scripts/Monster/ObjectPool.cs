using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Monster;

public class ObjectPool : MonoSingleTon<ObjectPool>
{
    // Dictionary pooling 
    static Dictionary<string, List<GameObject>> pooling = new Dictionary<string, List<GameObject>>();


    public GameObject BringObject(GameObject targetObj)
    {
        string key = targetObj.name.ToString();

        GameObject newObject = null;

        // pooling�� �ش� key ���� ���ٸ� �� List ����
        if (!pooling.ContainsKey(key))
        {
            // �� ����Ʈ ����
            List<GameObject> objectList = new List<GameObject>();

            // �ش� key�� ���ο� ����Ʈ �߰�
            pooling.Add(key, objectList);
        }


        foreach (GameObject obejctInPool in pooling[key])
        {
            // ��Ȱ��ȭ�� ������Ʈ�� �ִٸ�
            if (!obejctInPool.activeSelf)
            {
                obejctInPool.SetActive(true);
                newObject = obejctInPool;
                pooling[key].Remove(obejctInPool);
                return newObject;
            }
        }


        // ���� & list�� �߰�
        newObject = Instantiate(targetObj);
        newObject.name = newObject.name.Replace("(Clone)", "");

        return newObject;
    }

    // Back to pool ==============================================================================
    public void DestroyObject(GameObject destroyObject)
    {
        // key �� == ������Ʈ �̸�
        string key = destroyObject.name.ToString();

        // pooling�� �ش� key ���� ���ٸ� �� List ����
        if (!pooling.ContainsKey(key))
        {
            // �� ����Ʈ ����
            List<GameObject> newObjectList = new List<GameObject>();

            // �ش� key�� ���ο� ����Ʈ �߰�
            pooling.Add(key, newObjectList);
        }

        destroyObject.SetActive(false);

        pooling[key].Add(destroyObject);
    }
    // Back to pool ==============================================================================

}
