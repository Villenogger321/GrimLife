using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrimLife
{
    public class DebugWindow : MonoBehaviour
    {
        public GameObject objectToMeasure;
        private Vector3 previousPosition;
        private float speed;

        // Dictionary to store debug information
        private Dictionary<string, string> debugInfo = new Dictionary<string, string>();

        // Reference to PlayerMovement script
        private PlayerMovement playerMovement;


        void Awake()
        {
            if (playerMovement == null)
            {
                playerMovement = objectToMeasure.GetComponent<PlayerMovement>();
            }
        }
        void Start()
        {
            // Set the initial position of the GameObject
            previousPosition = objectToMeasure.transform.position;

            // Add debug information to the dictionary
            AddDebugInfo("Speed", "");
            AddDebugInfo("CanJump", "");
            AddDebugInfo("PlayerStandingOn", "");
        }

        void Update()
        {
            UpdateSpeedDebugInfo();
            UpdateCanJumpDebugInfo();
            UpdatePlayerStandingOnDebugInfo();
        }

        void OnGUI()
        {
            int y = 10;
            foreach (string key in debugInfo.Keys)
            {
                GUI.Label(new Rect(10, y, 200, 20), debugInfo[key]);
                y += 20;
            }
        }

        void AddDebugInfo(string key, string value)
        {
            debugInfo.Add(key, value);
        }

        void UpdateDebugInfo(string key, string value)
        {
            debugInfo[key] = value;
        }

        void UpdateSpeedDebugInfo()
        {
            Vector3 currentPosition = objectToMeasure.transform.position;
            float distanceTraveled = Vector3.Distance(currentPosition, previousPosition);
            speed = distanceTraveled / Time.deltaTime;
            previousPosition = currentPosition;

            UpdateDebugInfo("Speed", "Object speed: " + speed.ToString("0.00"));
        }

        void UpdateCanJumpDebugInfo()
        {
            bool canJump = CanJump();
            UpdateDebugInfo("CanJump", "CanJump: " + canJump.ToString());
        }

        void UpdatePlayerStandingOnDebugInfo()
        {
            string playerStandingOn = PlayerStandingOn();
            UpdateDebugInfo("PlayerStandingOn", "PlayerStandingOn: " + playerStandingOn);
        }

        bool CanJump()
        {
            // Check if player is grounded and ready to jump
            return playerMovement.grounded && playerMovement.readyToJump;
        }

        string PlayerStandingOn()
        {
            float raycastDistance = objectToMeasure.GetComponent<PlayerMovement>().playerHeight; // Set the distance to be just long enough to touch the ground
            RaycastHit hit;

            if (Physics.Raycast(objectToMeasure.transform.position, Vector3.down, out hit, raycastDistance))
            {
                if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
                {
                    return hit.transform.gameObject.name;
                }
                else
                {
                    return "unknown";
                }
            }
            return "air";
        }
    }
}
