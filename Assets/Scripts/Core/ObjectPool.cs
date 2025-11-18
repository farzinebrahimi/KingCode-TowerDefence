using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

namespace Core
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private readonly Queue<T>  _pool = new();
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly bool _autoExpand;

        public ObjectPool(T prefab, int initialCount, Transform parent = null, bool autoExpand = true)
        {
            _prefab = prefab;
            _parent = parent;
            _autoExpand = autoExpand;
            PreSpawn(initialCount);
        }

        private void PreSpawn(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T obj = CreateInstance();
                _pool.Enqueue(obj);
            }
        }

        private T CreateInstance()
        {
            var instance = GameObject.Instantiate(_prefab, _parent);
            return instance;
        }

        public T GetFromPull(Vector3 position, Quaternion rotation)
        {
            if (_pool.Count == 0)
            {
                if (_autoExpand)
                {
                    var newObj = CreateInstance();
                    newObj.transform.position = position;
                    return newObj;
                }

                return null;
            }
            T obj = _pool.Dequeue();
            obj.transform.SetPositionAndRotation(position, rotation);
            return obj;
        }

        public void ReturnToPool(T obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}