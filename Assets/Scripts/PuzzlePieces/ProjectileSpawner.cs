using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField]List<PlayerTag> tagList;
    List<TaggedProjectile> spawnedList = new List<TaggedProjectile>();
    [SerializeField]int maxProjectiles;
    [SerializeField]GameObject projectilePrefab, spawnPosition;
    float nextSpawn;
    [SerializeField]float initialSpeed,projectileMinSpeed ,spawnTimer, projectileGradDirection;
    [SerializeField]bool projectileDestroyOnEverything, projectileDestroyOnlyOnWalls, projectilePassTraitsToPlayer;
    void Update ()
    {
        
        if (nextSpawn <= 0)
        {
            if (spawnedList.Count < maxProjectiles)
            {
                nextSpawn = spawnTimer;
                SpawnProjectile();
            }
            
        }
        else 
        {
            nextSpawn -= Time.deltaTime;
        }
        //cannot directly remove items from a list during its iteration, the int list is there to save it for later
        List<int> deleteList = new List<int>();
        int counter = 0;
        foreach (TaggedProjectile tp in spawnedList)
        {
            
            if (tp == null)
            {
                deleteList.Add(counter);
            }
            counter++;
        }
        foreach (int index in deleteList)
        {
            spawnedList.RemoveAt(index);
        }
    }
    void SpawnProjectile()
    {
        TaggedProjectile spawned = Instantiate(projectilePrefab,spawnPosition.transform.position,spawnPosition.transform.rotation).GetComponent<TaggedProjectile>();

        Vector2 speed = (new Vector2 ((float)Math.Sin(projectileGradDirection*Math.PI/180),(float)Math.Cos(projectileGradDirection*Math.PI/180)))*initialSpeed;

        spawnedList.Add(spawned.Setup(speed, tagList,projectileMinSpeed,projectileDestroyOnEverything,projectileDestroyOnlyOnWalls,projectilePassTraitsToPlayer));
        //spawnedList.Add(spawned.Setup(new Vector2(1,1), tagList,projectileMinSpeed,projectileDestroyOnEverything,projectileDestroyOnCollisions,projectilePassTraitsToPlayer));
    }

}
