using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ctx = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace Multiplatform
{
    public class PlayerController : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        float speed, rotationSpeed;
        bool sprint;

        DefaultActions actions;

        private void Awake()
        {
            if (photonView.IsMine)
            {
                actions = new DefaultActions();
                actions.Enable();

                actions.CharacterController.Sprint.started += (ctx _o) => sprint = true;
                actions.CharacterController.Sprint.canceled += (ctx _o) => sprint = false;
            }
            else 
            {
                GetComponentInChildren<Camera>().enabled = false;
                GetComponentInChildren<AudioListener>().enabled = false;
            }
        }

        void Update()
        {
            if (photonView.IsMine)
            {
                float rotateInput = actions.CharacterController.Rotate.ReadValue<float>();

                if (sprint) rotateInput *= 2f;

                transform.Rotate(new Vector3(0, rotateInput, 0) * Time.deltaTime * rotationSpeed);

                Vector2 input = actions.CharacterController.Movement.ReadValue<Vector2>();

                if (sprint) input *= 2f;

                Vector3 movement = new Vector3(input.x, 0, input.y) * Time.deltaTime * speed;

                transform.Translate(movement, Space.Self);

            }
        }
    }
}