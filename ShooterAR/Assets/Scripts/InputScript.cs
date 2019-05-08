using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterAR
{
    public class InputScript : MonoBehaviour
    {
        private Command Fire;

        private void Start()
        {
            Fire = new FireWeapon();
        }

        void Update()
        {
            HandleInput();
        }

        public void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire.Execute();
            }

            if (Input.GetMouseButtonUp(0))
            {
                Fire.Stop();
            }
        }
    }
}
