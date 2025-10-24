using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    public class ObjectPoolManager : MonoBehaviour
    {
        #region Private members

        [SerializeField] private int _initialCapacity = 32;

        // Internally we manage a dictionary mapping prefab names to Pools (Queues of GameObject)
        private readonly Dictionary<string, Queue<GameObject>> _pool = new();

        #endregion Private Members

        /// <summary>
        /// Set initial capacity of pools. The default is 32.
        /// The capacity will always double when trying to add
        /// a new object to a queue at current capacity.
        /// </summary>
        /// <param name="initialCapacity"></param>
        public void SetInitialCapacity(int initialCapacity)
        {
            _initialCapacity = initialCapacity;
        }

        #region Spawning

        /// <summary>
        /// Spawn a GameObject from a prefab.
        /// If a pool does not already exist for this prefab type one will be created.
        /// If the pool contains an GameObject it will be returned, otherwise, a new
        /// GameObject will be instantiated.
        /// </summary>
        /// <param name="prefab">Type of GameObject to spawn.</param>
        /// <param name="setActive">Optional parameter to enable the GameObject after spawning. The default is <c>true</c></param>
        /// <returns>The spawned GameObject.</returns>
        public GameObject SpawnGameObject(GameObject prefab, bool setActive = true)
        {
            if (this == null) return null;

            GameObject go = DequeGameObject(prefab);
            if (go != null)
            {
                go.SetActive(setActive);
            }
            else
            {
                go = InstantiateGameObject(prefab, setActive);
            }

            return go;
        }

        /// <summary>
        /// Spawn a GameObject from a prefab.
        /// If a pool does not already exist for this prefab type one will be created.
        /// If the pool contains an GameObject it will be returned, otherwise, a new
        /// GameObject will be instantiated.
        /// </summary>
        /// <param name="prefab">Type of GameObject to spawn.</param>
        /// <param name="position">Position at which to spawn the GameObject.</param>
        /// <param name="rotation">Rotation to apply to the GameObject after spawning.</param>
        /// <param name="setActive">Optional parameter to enable the GameObject after spawning. The default is <c>true</c>.</param>
        /// <returns>The spawned GameObject.</returns>
        public GameObject SpawnGameObject(GameObject prefab, Vector3 position, Quaternion rotation,
            bool setActive = true)
        {
            if (this == null) return null;
            GameObject go = DequeGameObject(prefab);
            if (go != null)
            {
                go.transform.position = position;
                go.transform.rotation = rotation;
                go.SetActive(setActive);
            }
            else
            {
                go = InstantiateGameObject(prefab, position, rotation, setActive);
            }

            return go;
        }

        #endregion Spawning

        #region Despawning / Destroying

        /// <summary>
        /// Despawn the specified GameObject.
        /// The object will be deactivated and added to the appropriate pool for later reuse.
        /// </summary>
        /// <param name="go">The GameObject to despawn.</param>
        public void DespawnGameObject(GameObject go)
        {
            if (go == null) return;
            go.SetActive(false);
            go.GetComponent<IDespawnedPoolObject>()?.ReturnedToPool();
            var pool = GetPool(go);
            pool.Enqueue(go);
        }

        /// <summary>
        /// Permanently destroy all GameObjects for the specified prefab type.
        /// All queued GameObjects for the specified prefab type will be destroyed.
        /// </summary>
        /// <param name="prefab">The type of prefab you want to permanently destroy.</param>
        public void PermanentlyDestroyGameObjectsOfType(GameObject prefab)
        {
            if (this == null) return;
            var queue = GetPool(prefab);
            GameObject go;
            while (queue?.Count > 0)
            {
                go = queue.Dequeue();
                if (go != null)
                {
                    if (go.activeSelf)
                    {
                        go.SetActive(false);
                    }

                    Destroy(go);
                }
            }
        }

        /// <summary>
        /// Destroy all pooled GameObjects and clear their pools.
        /// </summary>
        public void EmptyPool(GameObject prefab = null)
        {
            if (this == null) return;
            if (prefab != null)
            {
                var pool = GetPool(prefab);
                if (pool == null) return;
                while (pool.Count > 0)
                {
                    var go = pool.Dequeue();
                    if (go != null)
                    {
                        Destroy(go);
                    }
                }
                pool.Clear();
                return;
            }
            foreach (Queue<GameObject> pool in _pool.Values)
            {
                while (pool.Count > 0)
                {
                    GameObject go = pool.Dequeue();
                    if (go != null)
                    {
                        Destroy(go);
                    }
                }
            }

            _pool.Clear();
        }

        #endregion

        #region Private methods

        private GameObject DequeGameObject(GameObject prefab)
        {
            var queue = GetPool(prefab);
            if (queue.Count < 1) return null;
            GameObject go = queue.Dequeue();
            if (go == null)
            {
                Debug.LogWarning("Dequeued null gameObject (" + prefab.name + ") from pool.");
            }
            go.GetComponent<IRetrievedPoolObject>()?.RetrievedFromPool(prefab);
            return go;
        }

        private GameObject InstantiateGameObject(GameObject prefab, bool setActive)
        {
            var queue = GetPool(prefab);
            var go = Instantiate(prefab);
            DontDestroyOnLoad(go);
            go.SetActive(setActive);
            go.name = prefab.name;
            return go;
        }

        private GameObject InstantiateGameObject(GameObject prefab, Vector3 position, Quaternion rotation,
            bool setActive)
        {
            GameObject go = Instantiate(prefab, position, rotation);
            DontDestroyOnLoad(go);
            go.SetActive(setActive);
            go.name = prefab.name;
            return go;
        }

        private Queue<GameObject> GetPool(GameObject prefab)
        {
            Queue<GameObject> pool;

            if (_pool.ContainsKey(prefab.name))
            {
                pool = _pool[prefab.name];
            }
            else
            {
                pool = new Queue<GameObject>(_initialCapacity);
                _pool.Add(prefab.name, pool);
            }

            return pool;
        }

        #endregion
    }
}