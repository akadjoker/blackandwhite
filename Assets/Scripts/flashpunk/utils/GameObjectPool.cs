using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace flashpunk.utils
{
    public class GameObjectPool
    {

        //Don't pool more than 100 objects, after this we'll just destroy anything returned to the pool.
        static public int maxPool = 100;

        //Get some GameObjects ready. This is nice to do in advance for a small performance boost.
        static public int preAllocate = 10;

        //Set this to a value greater than 0 if you want to hard limit the number of game objects.
        //This will ensure you don't create too many game objects for resource limited machines
        // however, it might result in unexpected behavior. (But maybe more expected than running out
        // of resources? Up to you.)
        static public int hardlimit = -1;

        //Create an object to make the parent of all the pooled objects
        static GameObject objectParent = new GameObject("LifeGaurd");

        static Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

        //One pool per prefab.
        class Pool
        {
            GameObject sourcePrefab;
            Stack<GameObject> objectPool;

            public Pool(GameObject sourcePrefab)
            {
                this.sourcePrefab = sourcePrefab;
                objectPool = new Stack<GameObject>(preAllocate);
                Allocate(preAllocate);
            }

            public GameObject Spawn()
            {
                if (objectPool.Count > 0)
                {
                    return RemoveFromPool();
                }
                else
                {
                    GameObject go = GameObject.Instantiate(sourcePrefab) as GameObject;
                    //For this simple example we rely on the name to discover the prefab pool, set it to the parent name
                    go.name = sourcePrefab.name;
                    return go;
                }
            }

            public void Recycle(GameObject go)
            {
                if (objectPool.Count < maxPool)
                {
                    AddToPool(go);
                }
                else
                {
                    GameObject.Destroy(go);
                }
            }

            private void Allocate(int allocateCount)
            {
                if ((hardlimit > 0) && objectPool.Count + allocateCount > maxPool)
                    allocateCount = hardlimit - objectPool.Count;

                for (int a = 0; a < allocateCount; a++)
                {
                    GameObject go = GameObject.Instantiate(sourcePrefab) as GameObject;
                    //For this simple example we rely on the name to discover the prefab pool, set it to the parent name
                    go.name = sourcePrefab.name;
                    AddToPool(go);
                }
            }

            private void AddToPool(GameObject go)
            {
                go.SetActive(false);
                go.transform.parent = objectParent.transform;

                objectPool.Push(go);
            }

            private GameObject RemoveFromPool()
            {
                GameObject go = objectPool.Pop();
                go.transform.parent = null;
                go.transform.position = Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.transform.rotation = Quaternion.identity;
                go.SetActive(true);
                return go;
            }
        }

        public static void Initialize(GameObject poolObject)
        {
            if (!pools.ContainsKey(poolObject.name))
            {
                pools.Add(poolObject.name, new Pool(poolObject));
            }
        }

        static public GameObject Spawn(GameObject poolObject)
        {
            if (!pools.ContainsKey(poolObject.name))
            {
                pools.Add(poolObject.name, new Pool(poolObject));
            }
            return pools[poolObject.name].Spawn();
        }

        static public void Recycle(GameObject go)
        {
            if (pools.ContainsKey(go.name))
            {
                pools[go.name].Recycle(go);
            }
            else
            {
                GameObject.Destroy(go);
            }
        }
    }
}