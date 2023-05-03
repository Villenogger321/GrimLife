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

        void Start()
        {
            // Set the initial position of the GameObject
            previousPosition = objectToMeasure.transform.position;
        }

        void Update()
        {
            Vector3 currentPosition = objectToMeasure.transform.position;
            float distanceTraveled = Vector3.Distance(currentPosition, previousPosition);
            speed = distanceTraveled / Time.deltaTime;
            previousPosition = currentPosition;
        }

        void OnGUI()
        {
            if (speed > 0.1)
            {
                GUI.Label(new Rect(10, 10, 200, 20), "Object speed: " + speed);
            }
        }
    }
}
