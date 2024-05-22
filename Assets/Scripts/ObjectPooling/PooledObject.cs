using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PooledObject : MonoBehaviour
{
    private ObjectPool associatedPool;
    public UnityEvent OnReset;

    private float timer = 0;
    private float destroyTime = 0;
    private bool setToDestroy = false;

    public void SetObjectPool(ObjectPool pool)
    {
        associatedPool = pool;
    }

    // Update is called once per frame
    void Update()
    {
        if (setToDestroy)
        {
            timer += Time.deltaTime;
            if (timer >= destroyTime)
            {
                setToDestroy = false;
                timer = 0;
                Destroy();
            }
        }
    }

    public void ResetObject()
    {
        OnReset?.Invoke();
    }

    public void Destroy()
    {
        if (associatedPool != null)
        {
            //Restore Object
        }
    }

    public void Destroy(float time)
    {
        setToDestroy = true;
        destroyTime = time;
    }
}
