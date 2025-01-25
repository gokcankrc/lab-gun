using UnityEngine;

public abstract class Breakable : MonoBehaviour
{
    [SerializeField] protected GameObject destroyedDecalPrefab;
    [SerializeField] protected int health = 5;
}