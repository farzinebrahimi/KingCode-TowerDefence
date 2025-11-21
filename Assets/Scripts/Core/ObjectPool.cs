using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class ObjectPool<T> where T : Component
    {
        private readonly Queue<T> _poolQueue = new();
        private readonly Func<T> _createFunc;
        private readonly Transform _container;

        public ObjectPool(T prefab, int initialCount, Transform container)
        {
            _container = container;
            _createFunc = () => UnityEngine.Object.Instantiate(prefab, container);
        
            for (int i = 0; i < initialCount; i++)
            {
                var obj = _createFunc();
                obj.gameObject.SetActive(false);
                _poolQueue.Enqueue(obj);
            }
        }
    
        public T Get(Vector3 position, Quaternion rotation)
        {
            var obj = Get();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }

        public T Get()
        {
            if (_poolQueue.Count == 0)
                return _createFunc();

            var obj = _poolQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void ReturnToPool(T obj)
        {
            obj.gameObject.SetActive(false);
            _poolQueue.Enqueue(obj);
        }
    }
}