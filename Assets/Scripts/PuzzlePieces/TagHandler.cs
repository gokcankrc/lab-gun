using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagHandler : MonoBehaviour, SpecialCollisionForTag
{
    [SerializeField]Tag.Trigger trigger;
    [SerializeField]Tag.Action action;
    [SerializeField]PlayerTag tagModified;
    [SerializeField]bool ignoreCollisionForTagReset;
    private void OnCollisionEnter2D(Collision2D other)
    {
        switch ((int)trigger)
        {
            case (int)Tag.Trigger.OnPlayerCollide:
            {
                var player = other.transform.GetComponent<Player>();
                if (player != null)
                {
                    ProcessAction(player.tagList);
                }
                break;
            }
        }
        
            
    }
    private void ProcessAction(List<PlayerTag> list)
    {
        switch ((int)action)
        {
            case (int)Tag.Action.Add:
            {
                PlayerTag.AddToList(list,tagModified);
                break;
            }
            case (int)Tag.Action.Subtract:
            {
                PlayerTag.SubtractFromList(list,tagModified);
                break;
            }
            case (int)Tag.Action.Reset:
            {
                PlayerTag.ResetValue(list,tagModified.tagName);
                break;
            }
            case (int)Tag.Action.Refresh:
            {
                PlayerTag.SetValue(list,tagModified);
                break;
            }
        }
        
    }
    public bool IgnoreForCollision()
    {
        return ignoreCollisionForTagReset;
    }

}
namespace Tag
{
    public enum Action
    {
        Add,
        Subtract,
        Reset,
        Refresh
    }
    public enum Trigger
    {
        OnPlayerCollide,
        OnProjectileCollide
    }
    public enum ResetsOn
    {
        none,
        OnPlayerCollide,
        OnTimePassed
    }
}
public interface SpecialCollisionForTag{
    public bool IgnoreForCollision()
    {
        return true;
    }
}