using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DaVanciInk.GenericPooler
{
    public class ReturnTuple<T1, T2>
    {
        public T1 Single { get; }
        public T2 Multiple { get; }

        public ReturnTuple(T1 item1, T2 item2)
        {
            Single = item1;
            Multiple = item2;
        }

    }

    [Serializable]
    public class Pooler<T> where T : Component
    {
        #region Public functions
        public ObjectPoolItem<T> m_Pool;

        private List<T> returnList = new List<T>();
        private Transform DefaultParent;

        /// <summary>
        /// return amount of active objects in specific pool
        /// </summary>
        /// <param name="_tag">Tag of ObjectPoolItem</param>
        /// <returns></returns>
        public int GetActivePooledObjects()
        {
            return m_Pool.m_ActiveObjects;
        }

        /// <summary>
        /// Spawn or despawn objects from pool
        /// </summary>
        /// <param name="_tag">Tag of ObjectPoolItem </param>
        /// <param name="_amount">If _amount Postive spawn(_amount) objects from pool,If _amount negative despawn(_amount) objects from pool </param>
        /// <param name="_position">spawn position</param>
        /// <param name="_rotation">spawn rotation</param>
        public ReturnTuple<T, List<T>> UpdateFromPool(int _amount = 1,bool activateObject = true, Vector3 _position = new Vector3(), Vector3? _minPosition = null, Vector3? _maxPosition = null, Quaternion _rotation = new Quaternion())
        {
            if (_amount == 0) return null;

            if (_amount > 0)
            {
                return SpawnFromPool(_amount, _position, _rotation, activateObject, _minPosition, _maxPosition);
            }
            else
            {
                DespawnFromPool(_amount);
                return ReturnTuple;
            }
        }
        #endregion
        public T Instantiate(T prefab, Transform parent)
        {
            T poolObject = (T)Object.Instantiate(prefab, parent);
            return poolObject;
        }
        public void Destroy(T GO)
        {
            Object.Destroy(GO.gameObject);
        }
        /// <summary>
        /// Initialze using SingletonMB initialisation
        /// </summary>
        public void Initialize(Transform defaultParent)
        {
            DefaultParent = defaultParent;

            List<T> objectPool = new List<T>();

            var poolParent = new GameObject(m_Pool.Tag);

            if (m_Pool.m_Parent == null)
            {
                poolParent.transform.SetParent(DefaultParent);
                m_Pool.m_Parent = poolParent.transform;
            }
            else
            {
                poolParent.transform.SetParent(m_Pool.m_Parent);
            }



            m_Pool.objectToPool.gameObject.SetActive(false);

            for (int i = 0; i < m_Pool.amountToPool; i++)
            {
                T po = Instantiate(m_Pool.objectToPool, m_Pool.m_Parent);
                //po.gameObject.SetActive(false); 
                objectPool.Add(po);
            }
            m_Pool.PooledObjects = objectPool;
        }

        public void Clear()
        {
            for (int i = 0; i < m_Pool.amountToPool; i++)
            {
                Destroy(m_Pool.PooledObjects[i]);
            }
        }
        #region Private funtions


        /// <summary>
        /// check if pool extandable,and extended it with specific value when needed
        /// </summary>
        /// <param name="_tag">Tag of ObjectPoolItem </param>
        /// <param name="_amount">amount to add</param>
        private void CheckExtandPool(int _amount)
        {
            if (_amount + m_Pool.m_ActiveObjects > m_Pool.amountToPool)
            {
                if (m_Pool.Expandable)
                {
                    int amountToAdd = (_amount + m_Pool.m_ActiveObjects) - m_Pool.amountToPool;

                    AddObjectToPool(amountToAdd);
                    m_Pool.amountToPool = _amount + m_Pool.m_ActiveObjects;

                }
                else
                {
                    DeSpawnFromPoolFirst();
                }
            }
        }

        /// <summary>
        /// Add objects (prefabs clone) to pool
        /// </summary>
        /// <param name="_item">ObjectPoolItem</param>
        /// <param name="_amountToAdd">_amountToAdd</param>
        private void AddObjectToPool(int _amountToAdd)
        {
            for (int i = 0; i < _amountToAdd; i++)
            {
                var poolObject = Instantiate(m_Pool.objectToPool, m_Pool.m_Parent);
                poolObject.gameObject.SetActive(false);
                m_Pool.PooledObjects.Insert(0, poolObject);
            }
        }
        private T objectToSpawn;
        private ReturnTuple<T, List<T>> ReturnTuple;

        private ReturnTuple<T, List<T>> SpawnFromPool(int _amount, Vector3 _position, Quaternion _rotation,bool activateObject = true, Vector3? _minPosition = null, Vector3? _maxPosition = null)
        {
            CheckExtandPool(_amount);

            returnList.Clear();

            for (int i = 0; i < _amount; i++)
            {
                returnList.Add(SpawnFromPool(_position, _rotation, activateObject, _minPosition, _maxPosition));
            }
            ReturnTuple = new ReturnTuple<T, List<T>>(returnList[0], returnList);
            return ReturnTuple;
        }

        private T SpawnFromPool(Vector3 _position, Quaternion _rotation, bool activateObject, Vector3? _minPosition = null, Vector3? _maxPosition = null)
        {
            objectToSpawn = m_Pool.PooledObjects[0];

            m_Pool.PooledObjects.Remove(objectToSpawn);

            objectToSpawn.gameObject.SetActive(activateObject);

            if (_minPosition != null && _maxPosition != null)
            {
                _position = _position.Random((Vector3)_minPosition, (Vector3)_maxPosition);
            }

            objectToSpawn.transform.position = _position;

            objectToSpawn.transform.rotation = _rotation;

            m_Pool.PooledObjects.Add(objectToSpawn);

            m_Pool.m_ActiveObjects++;

            m_Pool.OnVariableChange?.Invoke(this, EventArgs.Empty);

            return objectToSpawn;
        }

        public void DespawnObjectFromPool(T _itemToRemove)
        {
            DeSpawnFromPool(_itemToRemove);
        }

        private T objectToDeSpawn;



        private void DespawnFromPool(int _amount)
        {
            _amount = Mathf.Abs(_amount);

            if (_amount > m_Pool.m_ActiveObjects)
                _amount = m_Pool.m_ActiveObjects;


            for (int i = 0; i < _amount; i++)
            {
                DeSpawnFromPool();
            }
        }

        private void DeSpawnFromPool()
        {
            objectToDeSpawn = m_Pool.PooledObjects.Last();

            m_Pool.PooledObjects.Remove(objectToDeSpawn);

            objectToDeSpawn.gameObject.SetActive(false);

            m_Pool.PooledObjects.Insert(0, objectToDeSpawn);

            m_Pool.m_ActiveObjects--;
            m_Pool.OnVariableChange?.Invoke(this, EventArgs.Empty);

        }
        private void DeSpawnFromPoolFirst()
        {
            objectToDeSpawn = m_Pool.PooledObjects.First();

            m_Pool.PooledObjects.Remove(objectToDeSpawn);

            objectToDeSpawn.gameObject.SetActive(false);

            m_Pool.PooledObjects.Insert(m_Pool.PooledObjects.Count - 1, objectToDeSpawn);

            m_Pool.m_ActiveObjects--;

            m_Pool.OnVariableChange?.Invoke(this, EventArgs.Empty);

        }
        private void DeSpawnFromPool(T _itemToRemove)
        {
            if (m_Pool.PooledObjects.Contains(_itemToRemove))
            {
                m_Pool.PooledObjects.Remove(_itemToRemove);

                _itemToRemove.gameObject.SetActive(false);

                m_Pool.PooledObjects.Insert(0, _itemToRemove);

                m_Pool.m_ActiveObjects--;

                m_Pool.OnVariableChange?.Invoke(this, EventArgs.Empty);

            }
        }
        #endregion
    }

    [Serializable]
    public class ObjectPoolItem<T>
    {
        public string Tag;

        public List<T> PooledObjects;

        public T objectToPool;
        public int amountToPool;
        public bool Expandable = true;
        public Transform m_Parent;
        public int m_ActiveObjects;

        public EventHandler OnVariableChange;

        public void Reset()
        {
            Tag = string.Empty;
            PooledObjects.Clear();
            Expandable = false;
            m_Parent = null;
            amountToPool = 0;
            m_ActiveObjects = 0;
        }
    }

}