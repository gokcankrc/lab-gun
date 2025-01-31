using UnityEngine;

public class DestroyOnStart : MonoBehaviour
{
    private void Start()
    {
        DestroyImmediate(gameObject);
    }
}