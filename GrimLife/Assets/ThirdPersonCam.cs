using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // required for new input system

namespace GrimLife
{
    public class ThirdPersonCam : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform orientation;
        [SerializeField] private Transform player;
        [SerializeField] private Transform playerObj;
        [SerializeField] private Rigidbody rb;

        public float rotationSpeed = 3f;

        // Vector2 to store the input value
        private Vector2 movementInput;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            //Debug.Log("Movement input: " + context);

            movementInput = context.ReadValue<Vector2>();
        }

        private void Update()
        {
            Vector2 viewDirection = player.position - new Vector3(Camera.main.transform.position.x, player.position.y, Camera.main.transform.position.z);
            orientation.forward = viewDirection.normalized;

            //rotate player object
            Vector3 inputDirection = orientation.forward * movementInput.y + orientation.right * movementInput.x;

            if (inputDirection != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
            }
        }
    }
}
