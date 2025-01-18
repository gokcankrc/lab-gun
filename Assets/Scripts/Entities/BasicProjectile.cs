using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float lifetime = 2f;
    private bool flying;
    private Vector3 dir;

    public void Init(Vector3 dir)
    {
        this.dir = dir.normalized;
        flying = true;
    }

    private void Update()
    {
        if (flying)
        {
            flyingUpdate();
        }

        void flyingUpdate()
        {
            transform.position += dir * (speed * Time.deltaTime);
            lifetime -= Time.deltaTime;
            if (lifetime < 0)
            {
                flying = false;
            }
        }
    }
}