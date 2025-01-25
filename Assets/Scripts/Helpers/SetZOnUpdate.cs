using UnityEngine;

public class SetZOnUpdate : MonoBehaviour
{
    private void Update()
    {
        var pos = transform.position;
        pos.z = transform.position.y * 0.001f;
        transform.position = pos;
    }
}