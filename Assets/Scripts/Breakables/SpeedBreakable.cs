using UnityEngine;

public class SpeedBreakable : Breakable
{
    [SerializeField] private float hurtingSpeedThreshold;
    [SerializeField] private bool debugLog = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.transform.GetComponent<Player>();
        if (player != null)
        {
            Vector2 collisionNormal = other.contacts[0].normal;
            var impactSpeed = Vector2.Dot(other.relativeVelocity, collisionNormal);
            if (impactSpeed > hurtingSpeedThreshold)
            {
                if (debugLog)
                {
                    Debug.Log($"<color=green>Passes, {impactSpeed}</color>");
                }

                TakeDamage();
            }
            else
            {
                if (debugLog)
                {
                    Debug.Log($"<color=red>Doesn't pass, {impactSpeed}</color>");
                }
            }
        }
    }
}