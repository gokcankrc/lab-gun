using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaggedProjectile : MonoBehaviour,TaggedObject,SpecialCollisionForTag
{
    public List<PlayerTag> tagList = new List<PlayerTag>(); 
    Rigidbody2D rb;
    [SerializeField]float minSpeed;
    [SerializeField]bool removesTagsOnCollision;
    bool destroyOnEverything = false, destroyOnCollisions = false, passTrait = false;
    void Update()
    {
        //Since it's a physical object, it will crash into walls, this is meant to kill the projectile if it does. it also gives them a limited lifespan since they're meant to have drag
        if (rb.velocity.magnitude <= minSpeed)
        {
            Destroy(gameObject);
        }
    }
    public List<PlayerTag> GetTagList()
    {
        return tagList;
    }
    public TaggedProjectile Setup (Vector2 direction, float speed, List<PlayerTag> taglist,float autoDestroySpeed,bool destroyOnCollision = true,bool destoyOnlyOnWalls = true,bool passTraits = false)
    {
        tagList = taglist;
        rb.velocity = (Vector3)direction*speed;
        destroyOnCollisions = destroyOnCollision;
        destroyOnEverything = !destoyOnlyOnWalls;
        minSpeed = autoDestroySpeed;
        passTrait = passTraits;
        return this;
    }
    public TaggedProjectile Setup (Vector2 velocity, List<PlayerTag> taglist,float autoDestroySpeed,bool destroyOnCollision = true,bool destoyOnlyOnWalls = true,bool passTraits = false)
    {
        rb = this.transform.GetComponent<Rigidbody2D>();
        tagList = taglist;
        rb.velocity = (Vector3)velocity;
        destroyOnCollisions = destroyOnCollision;
        destroyOnEverything = !destoyOnlyOnWalls;
        minSpeed = autoDestroySpeed;
        passTrait = passTraits;
        return this;
    }
    public bool IgnoreForCollision()
    {
        return !removesTagsOnCollision;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (passTrait)
        {
            var tagObject = other.transform.GetComponent<TaggedObject>();
            if (tagObject != null)
            {
                foreach (PlayerTag pt in tagList){
                    if (!PlayerTag.HasTagValue(tagObject.GetTagList(),pt))
                    {
                        PlayerTag.AddToList(tagObject.GetTagList(),pt);
                    }
                }
            }
        }
        if (destroyOnCollisions)
        {
            if (destroyOnEverything){
                Destroy(gameObject);
                return;
            }
            var special = other.transform.GetComponent<SpecialCollisionForTag>();
            if (special == null || !special.IgnoreForCollision())
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}
