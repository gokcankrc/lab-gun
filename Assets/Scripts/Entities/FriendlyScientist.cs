using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyScientist : MonoBehaviour
{
    AnimatedCharacter animationController;
    [SerializeField] float minSpeed;
    [SerializeField]GameObject body;
    [SerializeField]PlayerTag pTag;
    bool alive = true;
    void Start()
    {
        animationController = gameObject.GetComponent<AnimatedCharacter>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!alive)
        {
            return;
        }
        var player = other.transform.GetComponent<PlayerMovement>();
        print (player.Speed);
        if (player != null && player.Speed >minSpeed)
        {
            Die();
            PlayerTag.AddToList(other.transform.GetComponent<Player>().GetTagList(),pTag);
            alive =false;
        }
    }
    void Die ()
    {
        alive = false;
        
        float timeToDie = animationController.StartAnimation(Animation.AnimationId.die,Animation.Direction.none, false);
        Destroy(gameObject.GetComponent<Collider2D>());
        if (body != null)
        {
            FadeInItem corpse = Instantiate (body, transform.position+new Vector3(0,0,0.01f), Quaternion.identity).GetComponent<FadeInItem>();
            if (corpse != null)
            {
                corpse.Setup(timeToDie);
            }
        }

        Destroy(gameObject, timeToDie);
        
    }
}
