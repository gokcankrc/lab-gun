using UnityEngine;

public class ContainmentWall : Breakable
{
    [SerializeField] private GameObject destroyedDecalPrefab;

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameManager.I.ContainmentWallBroken();
        Break();
    }

    private void Break()
    {
        Instantiate(destroyedDecalPrefab, transform.position, Quaternion.identity, DecalParent.I);
        Destroy(gameObject);
    }
}