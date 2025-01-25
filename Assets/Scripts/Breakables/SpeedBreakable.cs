using UnityEngine;

public class SpeedBreakable : Breakable
{
    [SerializeField] private float hurtingSpeedThreshold;

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.transform.GetComponent<Player>();
        if (player != null)
        {
            Vector2 collisionNormal = other.contacts[0].normal;
            var impactSpeed = Vector2.Dot(other.relativeVelocity, collisionNormal);
            if (impactSpeed > hurtingSpeedThreshold)
            {
                Debug.Log($"<color=green>Passes, {impactSpeed}</color>");
                // TODO: Sound
                TakeDamage();
            }
            else
            {
                Debug.Log($"<color=red>Doesn't pass, {impactSpeed}</color>");
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

    protected virtual void Break()
    {
        Instantiate(destroyedDecalPrefab, transform.position, Quaternion.identity, DecalParent.I);
        Destroy(gameObject);
    }
}