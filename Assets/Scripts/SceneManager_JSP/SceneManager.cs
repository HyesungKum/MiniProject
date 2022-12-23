using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    #region Singleton
    static SceneManager inst = null;

    static public SceneManager Inst
    {
        get
        {
            if (inst == null)
            {
                inst = GameObject.FindObjectOfType<SceneManager>();

                if (inst == null)
                {
                    inst = new GameObject("ObjectPool").AddComponent<SceneManager>();
                }
            }
            return inst;
        }
    }
    #endregion

    private void Awake()
    {
        if (inst == this)
        {
            DontDestroyOnLoad(this);
        }

        else
        {

        }
    }





}
