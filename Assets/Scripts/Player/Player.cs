using Ky;
using UnityEngine;

public class Player : Singleton<Player>
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private Camera cam;
    public Vector3 Pos => transform.position;

    private void Update()
    {
        /*
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        transform.position += new Vector3(x, y, 0) * (Time.deltaTime * speed);
        */
    }
}