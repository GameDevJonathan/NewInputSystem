 using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    //1. Get a reference and start an instance of our input actions.
    private PlayerInputActions _playerInput;
    private MeshRenderer _meshRenderer;
    
    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        //2. Initialize our Controls
        _playerInput = new PlayerInputActions();

        //3. Enable Input Action Map(Player) in this case.
        _playerInput.Player.Enable();

        //4 Register Event
        _playerInput.Player.SwitchAction.performed += SwitchAction_performed;
    }

    private void SwitchAction_performed(InputAction.CallbackContext context)
    {
        _playerInput.Player.Disable();
        _playerInput.Player3D.Enable();
        _meshRenderer.material.color = Color.blue;
    }

    private void Update()
    {
        var move = _playerInput.Player.Move.ReadValue<Vector2>();
        var move3D = _playerInput.Player3D.Move3D.ReadValue<Vector2>();

        //vector2 movement       
        transform.Translate(new Vector2(move.x, move.y) * Time.deltaTime * 3f);

        //vector3 movement
        transform.Translate(new Vector3(move3D.x, 0, move3D.y) * Time.deltaTime * 3f);
    }


}
