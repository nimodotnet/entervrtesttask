using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    List<T> pool;
    T prefab;

    public ObjectPool(T prefab, int initialSize)
    {
        this.prefab = prefab;
        pool = new List<T>(initialSize);

        for (int i = 0; i < initialSize; i++)
        {
            CreateObject();
        }
    }

    public T GetObject()
    {
        T obj = pool.Find(o => !o.gameObject.activeSelf);

        if (obj == null)
        {
            obj = CreateObject();
        }

        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    private T CreateObject()
    {
        T obj = Object.Instantiate(prefab);
        obj.gameObject.SetActive(false);
        pool.Add(obj);
        return obj;
    }
}
