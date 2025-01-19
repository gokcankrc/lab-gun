using UnityEngine;
using Logger = Ky.Logger;

public class ContainmentWall : Breakable
{
    [SerializeField] private GameObject destroyedDecalPrefab;
    [SerializeField] private int health = 5;
    [SerializeField] private float hurtingSpeedThreshold;

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.transform.GetComponent<Player>();
        if (player != null)
        {
            var speed = other.relativeVelocity.magnitude;
            if (speed > hurtingSpeedThreshold)
            {
                Debug.Log($"<color=green>Passes, {speed}</color>");
                // TODO: Sound
                TakeDamage();
            }
            else
            {
                Debug.Log($"<color=red>Doesn't pass, {speed}</color>");
                // TODO: Sound
            }
        }
    }

    private void TakeDamage()
    {
        // TODO: Show cracks
        health -= 1;
        if (health <= 0)
        {
            Break();
        }
    }

    private void Break()
    {
        GameManager.I.ContainmentWallBroken();
        Instantiate(destroyedDecalPrefab, transform.position, Quaternion.identity, DecalParent.I);
        Destroy(gameObject);
    }
}