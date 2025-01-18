using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour { }

internal enum EnemyState
{
    Idle,
    Following,
    Attacking,
}