using Ky;
using UnityEngine;

public class Player : Singleton<Player>
{
    [SerializeField] private PlayerMovement playerMovement;

    public Vector3 Pos => transform.position;
}