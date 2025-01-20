using Ky;
using UnityEngine;
using System.Collections.Generic;
public class Player : Singleton<Player>
{
    public PlayerMovement movement;
    public List<PlayerTag> tagList = new List<PlayerTag>(); 
    public int health;
    public Vector3 Pos => transform.position;

    void Update()
    {
        List<string> removeList = new List<string>(); 
        foreach (PlayerTag pt in tagList)
        {
            
            if (pt.ProcessResetCondition(Tag.ResetsOn.OnTimePassed,Time.deltaTime))
            {
                removeList.Add(pt.tagName);
            }
            
        }
        foreach (string tagName in removeList)
        {
            PlayerTag.ResetValue(tagList,tagName);
        }
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        List<string> removeList = new List<string>(); 
         foreach (PlayerTag pt in tagList)
        {
            SpecialCollisionForTag special = other.transform.GetComponent<SpecialCollisionForTag>();
            if (special != null && special.IgnoreForCollision())
            {
                return;
            }

            if (pt.ProcessResetCondition(Tag.ResetsOn.OnPlayerCollide,1f))
            {
                removeList.Add(pt.tagName);
            }
           
            
        }
        foreach (string tagName in removeList)
        {
            PlayerTag.ResetValue(tagList,tagName);
        }
    }
}