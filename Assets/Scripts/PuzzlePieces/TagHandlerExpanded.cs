using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//mostly for triggers and effect indipendent from tags (such as healing or speedups)
public class TagHandlerExpanded : TagHandler
{
    [SerializeField]List<SpecialActions> special;
    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.transform.GetComponent<Player>();
        switch ((int)trigger)
        {
            case (int)Tag.Trigger.OnPlayerCollide:
            {
                
                if (player != null)
                {
                    ProcessAction(player.tagList);
                    player.PlayerTagsChanged();
                }
                
                break;
            }
        }
        ProcessSpecial(player,AdditionalActions.Trigger.PlayerEnters);
        
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.transform.GetComponent<Player>();
        switch ((int)trigger)
        {
            case (int)Tag.Trigger.OnPlayerCollide:
            {
                if (player != null)
                {
                    ProcessAction(player.tagList);
                    player.PlayerTagsChanged();
                }
                break;
            }
        }   
        ProcessSpecial(player,AdditionalActions.Trigger.PlayerEnters);
    }
    void ProcessSpecial(Player target ,AdditionalActions.Trigger trigger)
    {
        foreach (SpecialActions sa in special)
        {
            if (sa.trigger == trigger)
            {
                switch ((int)sa.action)
                {
                    case (int)AdditionalActions.Action.Accellerate:
                    {
                        target.transform.GetComponent<Rigidbody2D>().velocity *= sa.value;
                        break;
                    }
                }
            }
        }
    }
}


