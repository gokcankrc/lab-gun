using Ky;
using UnityEngine;

public class Player : Singleton<Player>
{
    public PlayerMovement movement;

    public Vector3 Pos => transform.position;
}