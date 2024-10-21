using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int bulletPoolSize = 20;
    public List<GameObject> bulletPool;

    // Start is called before the first frame update
    void Start()
    {
        bulletPool = new List<GameObject>();

        for (int i = 0; i < bulletPoolSize; i++)
        {
            //spawn bullet
            GameObject bullet = Instantiate(bulletPrefab);
            //deactivate bullet
            bullet.SetActive(false);
            //add bullet to pool
            bulletPool.Add(bullet);
        }
    }

    //make bullet pooling method, foreach loop bullet in the pool, if bullet is not active in hierarchy, return bullet
    public GameObject GetPooledBullet()
    {
        foreach(GameObject bullet in bulletPool)
        {
            if(!bullet.activeInHierarchy)
            {
                return (bullet);
            }
        }

        // If no bullet is available, return null
        return null;
    }

}
