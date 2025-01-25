using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class ResetTrailRendererIfTooLong : MonoBehaviour
{
    [SerializeField] private float threshold = 2f;

    private Vector3 lastPos;
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void LateUpdate()
    {
        UpdateTrail();
    }

    private void UpdateTrail()
    {
        var dist = lastPos - transform.position;
        if (dist.magnitude > threshold)
        {
            trailRenderer.Clear();
        }

        lastPos = transform.position;
    }
}