using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField]Controls inputScheme;
    [SerializeField]float playerSpeed;
    Rigidbody2D body;
    bool movingWithMouse;
    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        if (body is null){
            print (name+" has no Rigidbody2D");
        }
        inputScheme = new Controls();
        inputScheme.Movement.Enable();
        inputScheme.Movement.MouseClick.started += StartMovement;
        //inputScheme.Movement.MouseClick.performed += StopMovement;
        inputScheme.Movement.MouseClick.canceled += StopMovement;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movingWithMouse){
            MoveToMouse();
        }
        else {
            MoveThroughKeyboard();
        }
    }
    void MoveThroughKeyboard(){
        Vector2 moveVector = inputScheme.Movement.Directions.ReadValue<Vector2>();
        Move(moveVector);
    }
    void StartMovement (UnityEngine.InputSystem.InputAction.CallbackContext input){
        
        movingWithMouse = true;
    }
    void StopMovement (UnityEngine.InputSystem.InputAction.CallbackContext input){
        movingWithMouse = false;
        
    }
    void MoveToMouse(){
        Vector2 moveVector = (Vector2)Camera.main.ScreenToWorldPoint(inputScheme.Movement.MousePosition.ReadValue<Vector2>());
        moveVector -= (Vector2)transform.position;
		Move(moveVector);
    }
    void Move(Vector2 vector){
        Vector2 moveVector = vector.normalized;
        moveVector*= playerSpeed*Time.fixedDeltaTime;
		body.AddForce(moveVector);
    }
}
