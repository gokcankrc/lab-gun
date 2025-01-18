using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField]Controls inputScheme;
    [SerializeField]float playerSpeed;
    Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        if (body is null){
            print (name+" has no Rigidbody2D");
        }
        inputScheme = new Controls();
        inputScheme.Movement.Enable();
        inputScheme.Movement.MouseClick.performed += PushToMouse;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PushToMouse(UnityEngine.InputSystem.InputAction.CallbackContext input){
        Vector2 mousePos = inputScheme.Movement.MousePosition.ReadValue<Vector2>();
        if (mousePos.x > mousePos.y){
            mousePos /= mousePos.x;
        }
        else{
            mousePos /= mousePos.y;
        }
        
        mousePos-= new Vector2(0.5f,0.5f);
        mousePos*= playerSpeed;
        body.AddForce(mousePos);
    }
}
