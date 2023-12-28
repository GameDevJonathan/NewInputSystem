using UnityEngine;
using UnityEngine.InputSystem;

public class BouncingBall : MonoBehaviour
{
    //create a reference to the input actions
    private PlayerInputActions _input;

    private Rigidbody _rb;
    private Keyboard keyboard;


    // Start is called before the first frame update
    void Start()
    {
        keyboard = Keyboard.current;

        _rb = GetComponent<Rigidbody>();
        //initialze the input actions
        _input = new PlayerInputActions();

        //enable the actionmap
        _input.Player.Enable();

        //register the events
        //_input.Player.Jump.started += Jump_started;
        //_input.Player.Jump.performed += Jump_performed;
        //_input.Player.Jump.canceled += Jump_canceled;        
    }

    private void Jump_started(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _rb.AddForce(Vector3.up * 5 , ForceMode.Impulse);
        
    }
    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _rb.AddForce(Vector3.up * 5 , ForceMode.Impulse);


    }
    private void Jump_canceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _rb.velocity = Vector3.zero;

    }    
    
    // Update is called once per frame
    void Update()
    {
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Was pressed this frame");
            _rb.AddForce(Vector3.up * 50 * Time.deltaTime, ForceMode.Impulse);
        }

        if(keyboard.spaceKey.isPressed)
        {
            Debug.Log("Holding Key");
            _rb.AddForce(Vector3.up * 50 * Time.deltaTime, ForceMode.Impulse);

        }

        if (keyboard.spaceKey.wasReleasedThisFrame)
        {
            Debug.Log("Key was released");
            _rb.velocity = Vector3.zero;

        }

    }
}
