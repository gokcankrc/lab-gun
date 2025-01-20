using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ConditionalBreakable : MonoBehaviour
{
    [SerializeField] private GameObject destroyedDecalPrefab;
    [SerializeField] private int health = 1;
    [SerializeField] private float hurtingSpeedThreshold;
    [SerializeField] private PlayerTag [] conditionList;
    [SerializeField] private bool needsAllConditions;
    [SerializeField] private bool isContainmentWall;
    [SerializeField] ConditionalBreakable [] linkedParts;
    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.transform.GetComponent<Player>();
        if (player != null)
        {
            if (player.movement.Speed > hurtingSpeedThreshold)
            {
                bool validImpact = false;

                
                if (conditionList.Length>0)
                {
                    if (needsAllConditions)
                    {
                        bool hasAll = true;
                        foreach (PlayerTag condition in conditionList)
                        {
                            if (!PlayerTag.HasTagValue(player.tagList,condition))
                            {
                                hasAll = false;
                            }
                        }
                        if (hasAll)
                        {
                            // TODO: Sound
                            TakeDamage();
                            validImpact = true;
                        }
                    }
                    else 
                    {
                        foreach (PlayerTag condition in conditionList)
                        {
                            if (PlayerTag.HasTagValue(player.tagList,condition))
                            {
                                validImpact = true;
                                // TODO: Sound
                                TakeDamage();
                                break;
                            }
                        }
                    }
                    
                }
                //If it has no conditions, then it's just a wall that doesn't trigger scientists when broken
                else 
                {
                    validImpact = true;
                    // TODO: Sound
                    TakeDamage();
                }
                
                if (!validImpact)
                {
                    // TODO: Sound
                }
            }
            else
            {
                // TODO: Sound
            }
        }
    }

    private void TakeDamage()
    {
        // TODO: Show cracks
        health -= 1;
        if (health <= 0)
        {
            Break();
        }
    }

    private void Break(bool chain = true)
    {
        if (chain)
        {
            foreach (ConditionalBreakable linked in linkedParts)
            {
                linked.Break(false);
            }
        }
        if (isContainmentWall)
        {
            GameManager.I.ContainmentWallBroken();
        }
        
        Instantiate(destroyedDecalPrefab, transform.position, Quaternion.identity, DecalParent.I);
        Destroy(gameObject);
    }
}
