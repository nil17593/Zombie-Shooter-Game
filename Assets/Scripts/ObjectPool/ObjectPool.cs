using System.Collections.Generic;
using UnityEngine;

namespace SuperGaming.ZombieShooter.Pools
{
    /// <summary>
    /// this is object pool class
    /// this is generic
    /// to use this we have to create new instance and pass the type 
    /// to create the pool
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> where T : Component
    {
        private GameObject _prefab;
        private Transform _parent;
        private List<T> _pool;

        public ObjectPool(GameObject prefab, Transform parent, int count)
        {
            _prefab = prefab;
            _parent = parent;

            _pool = new List<T>();

            for (int i = 0; i < count; i++)
            {
                GameObject obj = Object.Instantiate(prefab, parent);
                obj.SetActive(false);
                _pool.Add(obj.GetComponent<T>());
            }
        }

        public T GetPooledObject()
        {
            foreach (T obj in _pool)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }

            // If we've reached here, it means all lines in the pool are in use. So, increase pool size.
            GameObject newObj = Object.Instantiate(_prefab, _parent);
            T newObjComponent = newObj.GetComponent<T>();
            _pool.Add(newObjComponent);
            newObj.SetActive(true);
            return newObjComponent;
        }

        public void ClearPool()
        {
            foreach (var obj in _pool)
            {
                obj.gameObject.SetActive(false);
            }
        }
    }
}