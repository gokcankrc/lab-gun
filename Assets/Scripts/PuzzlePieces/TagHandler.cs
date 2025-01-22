using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagHandler : MonoBehaviour, SpecialCollisionForTag
{
    [SerializeField]protected Tag.Trigger trigger;
    [SerializeField]private Tag.Action action;
    [SerializeField]private PlayerTag tagModified;
    [SerializeField]private bool ignoreCollisionForTagReset;
    private void OnCollisionEnter2D(Collision2D other)
    {
        switch ((int)trigger)
        {
            case (int)Tag.Trigger.OnPlayerCollide:
            {
                var target = other.transform.GetComponent<TaggedObject>();
                if (target != null)
                {
                    ProcessAction(target.GetTagList());
                }
                break;
            }
        }
        
            
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch ((int)trigger)
        {
            case (int)Tag.Trigger.OnPlayerCollide:
            {
                var target = other.transform.GetComponent<TaggedObject>();
                if (target != null)
                {
                    ProcessAction(target.GetTagList());
                }
                break;
            }
        }
        
            
    }
    protected void ProcessAction(List<PlayerTag> list)
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
        None,
        Add,
        Subtract,
        Reset,
        Refresh
    }
    public enum Trigger
    {
        None,
        OnPlayerCollide,
        OnProjectileCollide
    }
    public enum ResetsOn
    {
        None = 0b_0000_0000,
        OnPlayerCollide = 0b_0000_0001,
        OnTimePassed = 0b_0000_0010,
        OnSpeedReached = 0b_0000_0100,
        OnSpeedNotReached= 0b_0000_1000
    }
}
public interface SpecialCollisionForTag{
    public bool IgnoreForCollision()
    {
        return true;
    }
}
public interface TaggedObject
{
    public List<PlayerTag> GetTagList()
    {

        return new List<PlayerTag>();
    }
}