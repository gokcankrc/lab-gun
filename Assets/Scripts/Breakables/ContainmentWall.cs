using System;
using UnityEngine;

public abstract class ContainmentWall : Breakable
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"collision entered to containment wall");
        GameManager.I.ContainmentWallBroken();
    }

    private void OnTriggerEnter2D(Collider other)
    {
        Debug.Log($"trigger entered to containment wall");
        GameManager.I.ContainmentWallBroken();
    }
}