using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    private Rigidbody _rigidBody;
    private Vector2 _moveInput;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        PlayerControls controls = new PlayerControls();
        controls.Player.Enable();
        controls.Player.Move.performed += OnMovePerformed;
    }

    private void OnDisable()
    {
        PlayerControls controls = new PlayerControls();
        controls.Player.Disable();
        controls.Player.Move.performed -= OnMovePerformed;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(_moveInput.x, 0f, _moveInput.y);
        _rigidBody.velocity = movement * _speed;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector3>();
    }
}
