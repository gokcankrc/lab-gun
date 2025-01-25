using System.Collections.Generic;
using UnityEngine;

public abstract class Breakable : MonoBehaviour
{
    [SerializeField] protected GameObject destroyedDecalPrefab;
    [SerializeField] protected int health = 5;
    [SerializeField] protected List<Sprite> healthSprites = new List<Sprite>();
    [SerializeField] protected SpriteRenderer sprRenderer;

    protected virtual void TakeDamage()
    {
        // TODO: Show cracks
        health -= 1;
        UpdateSprite();
        if (health <= 0)
        {
            Break();
        }
    }

    protected virtual void UpdateSprite()
    {
        if (health < healthSprites.Count)
        {
            sprRenderer.sprite = healthSprites[health];
        }
    }

    protected virtual void Break()
    {
        Instantiate(destroyedDecalPrefab, transform.position, Quaternion.identity, DecalParent.I);
        Destroy(gameObject);
    }
}