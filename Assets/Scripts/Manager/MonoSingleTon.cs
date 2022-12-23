using UnityEngine;

public class MonoSingleTon<T> : MonoBehaviour where T : class, new()
{
    private static T inst = null;
    private static object _lock = new object();
    public static T Inst
    {
        get
        {
            if (inst == null)
            {
                inst = GameObject.FindObjectOfType(typeof(T)) as T;//find

                if (inst == null)
                {
                    lock (_lock)//single thread
                    {
                        GameObject newInst = new GameObject(typeof(T).ToString(), typeof(T));
                        inst = newInst.GetComponent<T>();
                    }
                }
            }
            return inst;
        }
    }
}
