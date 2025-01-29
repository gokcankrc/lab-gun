using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
	[SerializeField]Controls inputScheme;
    [SerializeField]float playerSpeed, playerSpeedWalk;
    [SerializeField]float maxSpeed,maxSpeedWalk, resetTime;
    Rigidbody2D body;
    bool movingWithMouse;
    bool ballMode;
    public static bool resetting;
    bool canInterruptReset = true;
    float resetTimeRemaining;
    AnimatedCharacter animationController;
    Animation.Direction currentDir = Animation.Direction.down;

    public float Speed => body.velocity.magnitude;
    public float GetSpeed()
    {
        return Speed;
    }
    // Start is called before the first frame update
    void Start()
    {
        animationController = gameObject.GetComponent<AnimatedCharacter>();
        if (animationController == null)
        {
            print (name+" has no Animator");
        }
        body = gameObject.GetComponent<Rigidbody2D>();
        if (body == null)
        {
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
    void OnDestroy()
    {
        inputScheme.Movement.Disable();
        inputScheme.Movement.MouseClick.started -= StartMovement;
        //inputScheme.Movement.MouseClick.performed -= StopMovement;
        inputScheme.Movement.MouseClick.canceled -= StopMovement;
        inputScheme.Movement.Reset.started -= StartReset;
        inputScheme.Movement.Reset.canceled -= StopReset;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inputScheme.Movement.Directions.ReadValue<Vector2>() != Vector2.zero || movingWithMouse)
        {
            if (movingWithMouse){
            SetBallMode(true);
            MoveToMouse();
            }
            else {
                SetBallMode(false);
                MoveThroughKeyboard();
            }
        }
        else 
        {
            
            if (!movingWithMouse && Speed> 0.05f)
            {
                body.velocity /= 1.5f;
            }
            
            if (Speed <=0.2f)
            {
                if (Speed <=0.05f &&Speed>0)
                {
                    body.velocity *=0;
                }
                
                animationController.StartAnimation(Animation.AnimationId.idle,Animation.Direction.none, false);
            }
        }
        
        LimitSpeed();
        CheckResetTimer();
        CheckRotation();
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
    void SetBallMode(bool flag)
    {
        ballMode = flag;
        if (flag)
        {

        }
    }
    void CheckRotation()
    {
        if (AnimatedCharacter.VectorToDirection((Vector2)body.velocity) != currentDir)
        {
            currentDir = AnimatedCharacter.VectorToDirection((Vector2)body.velocity);
            animationController.Turn(currentDir);
        }
    }
    public static void ResetStage ()
    {
        //print ("reset");
        MusicManager.I.Resetting();
        SceneManager.LoadScene("Main Scene");
        resetting = false;
    }
    void MoveThroughKeyboard(){
        if (resetting)
        {
            return;
        }
        animationController.StartAnimation(Animation.AnimationId.walk,Animation.Direction.none, false);
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
        if (canInterruptReset)
        {
            canInterruptReset = true;
            Reset();
        }
        
    }
    public void ForceReset ()
    {
        canInterruptReset = false;
        Reset();
    }
    void Reset()
    {
        if (!resetting)
        {
            //print ("Starting Reset");
            animationController.StartAnimation(Animation.AnimationId.rewindingTime,Animation.Direction.none, true);
            resetTimeRemaining = resetTime;
            resetting = true;
        }
        
    }
    void StopReset (UnityEngine.InputSystem.InputAction.CallbackContext input)
    {
        if (resetTimeRemaining <= 0 || !canInterruptReset)
        {
            return;
        }
        //print ("stopping reset");
        animationController.StopAnimation();
        resetting = false;
    }
    void MoveToMouse(){
        if (resetting)
        {
            return;
        }
        animationController.StartAnimation(Animation.AnimationId.run,Animation.Direction.none, false);
        Vector2 moveVector = (Vector2)Camera.main.ScreenToWorldPoint(inputScheme.Movement.MousePosition.ReadValue<Vector2>());
        moveVector -= (Vector2)transform.position;
		Move(moveVector);
    }
    void Move(Vector2 vector){
        
        Vector2 moveVector = vector.normalized;
        if (movingWithMouse)
        {
            moveVector*= playerSpeed*Time.fixedDeltaTime;
        }
        else 
        {
            moveVector*= playerSpeedWalk*Time.fixedDeltaTime;
        }
		body.AddForce(moveVector);
    }
    void LimitSpeed()
    {
        Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
        if (ballMode)
        {
            if (rb.velocity.magnitude>maxSpeed)
            {
                rb.velocity = rb.velocity.normalized*maxSpeed;
            }
        }
        else 
        {
            if (rb.velocity.magnitude>maxSpeedWalk)
            {
                rb.velocity = rb.velocity.normalized*maxSpeedWalk;
            }
        }
        
    }
}
