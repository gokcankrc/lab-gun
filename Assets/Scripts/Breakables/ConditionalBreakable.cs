using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalBreakable : MonoBehaviour
{
    [SerializeField] private GameObject destroyedDecalPrefab;
    [SerializeField] private int health = 1;
    [SerializeField] private float hurtingSpeedThreshold;
    [SerializeField] private PlayerTag condition;
    [SerializeField] private bool exactValueForCondition;
    [SerializeField] private bool isContainmentWall;
    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.transform.GetComponent<Player>();
        if (player != null)
        {
            if (player.movement.Speed > hurtingSpeedThreshold)
            {
                
                if (PlayerTag.HasTagValue(player.tagList,condition,exactValueForCondition))
                {
                    // TODO: Sound
                    TakeDamage();
                }
                else
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

    private void Break()
    {
        if (isContainmentWall)
        {
            GameManager.I.ContainmentWallBroken();
        }
        
        Instantiate(destroyedDecalPrefab, transform.position, Quaternion.identity, DecalParent.I);
        Destroy(gameObject);
    }
}
