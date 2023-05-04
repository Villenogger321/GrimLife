using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // required for new input system

namespace GrimLife
{
    public class ThirdPersonCam : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform player;
        Transform orientation;
        Transform playerObj;
        Rigidbody rb;

        public float rotationSpeed = 3f;

        // Vector2 to store the input value
        private Vector2 movementInput;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            player = GameObject.FindGameObjectWithTag("Player").transform;
            playerObj = player.GetChild(0);
            orientation = player.GetChild(1);
            rb = player.GetComponent<Rigidbody>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
        }

        private void Update()
        {
            Vector3 camToPlayer = player.position - new Vector3(Camera.main.transform.position.x, player.position.y, Camera.main.transform.position.z);
            Vector3 newOrientationPos = player.position + camToPlayer.normalized;
            orientation.position = newOrientationPos;
            orientation.forward = -camToPlayer.normalized;

            Vector3 inputDirection = orientation.forward * movementInput.y + orientation.right * movementInput.x;

            if (inputDirection != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
            }
        }
    }
}
