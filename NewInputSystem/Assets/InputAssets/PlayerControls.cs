 using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    //1. Get a reference and start an instance of our input actions.
    private PlayerInputActions _playerInput;
    
    private void Start()
    {
        //2. Initialize our Controls
        _playerInput = new PlayerInputActions();


        //3. Enable Input Action Map(Player) in this case.
        _playerInput.Player.Enable();        
    }

    private void Update()
    {
        var move = _playerInput.Player.Move.ReadValue<Vector2>();
        
        //vector2 movement
        //transform.Translate(move * Time.deltaTime * 3f); 

        //vector3 movement
        transform.Translate(new Vector3(move.x,0,move.y) * Time.deltaTime * 3f);
    }


}
