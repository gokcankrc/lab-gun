using Sirenix.OdinInspector;
using UnityEngine;

public class SetZOnAwake : MonoBehaviour
{
    private void Awake()
    {
        SetZ();
    }

    private void Start()
    {
        Destroy(this);
    }

    [Button]
    private void SetZ()
    {
        var pos = transform.position;
        pos.z = transform.position.y * 0.001f;
        transform.position = pos;
    }
}