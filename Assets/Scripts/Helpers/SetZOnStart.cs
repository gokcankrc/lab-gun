using UnityEngine;

public class SetZOnStart : MonoBehaviour
{
    private void Start()
    {
        var pos = transform.position;
        pos.z = transform.position.y * 0.001f;
        transform.position = pos;
        Destroy(this);
    }
}