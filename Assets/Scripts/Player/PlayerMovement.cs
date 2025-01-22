using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
	[SerializeField]Controls inputScheme;
    [SerializeField]float playerSpeed;
    [SerializeField]float maxSpeed, resetTime;
    Rigidbody2D body;
    bool movingWithMouse;
    bool resetting;
    float resetTimeRemaining;

    public float Speed => body.velocity.magnitude;

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
        inputScheme.Movement.Reset.started += StartReset;
        inputScheme.Movement.Reset.canceled += StopReset;
        
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
        LimitSpeed();
        CheckResetTimer();
    }
    void CheckResetTimer()
    {
        if (resetting)
        {
            resetTimeRemaining -= Time.fixedDeltaTime;
            if (resetTimeRemaining<=0f)
            {
                ResetStage();
            }
        }
    }
    public static void ResetStage ()
    {
        SceneManager.LoadScene("Main Scene");
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
    public void StartReset (UnityEngine.InputSystem.InputAction.CallbackContext input)
    {
        resetTimeRemaining = resetTime;
        resetting = true;
    }
    void StopReset (UnityEngine.InputSystem.InputAction.CallbackContext input)
    {
        resetting = false;
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
    void LimitSpeed()
    {
        Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
        if (rb.velocity.magnitude>maxSpeed)
        {
            rb.velocity = rb.velocity.normalized*maxSpeed;
        }
    }
}
