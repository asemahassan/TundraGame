/*PlayerController:

This class handles the movements of player based on tactile floor data.
1. Player only move forwards.
2. Strafe left or right
3. No backward movement is allowed
4. No rotation of camera.
*/

//For collision move with Character Controller.Move
using UnityEngine;
using System.Collections;

namespace _3DRunner
{

    public class PlayerController : MonoBehaviour
    {
        public float speed = 6.0F;
        public float jumpSpeed = 8.0F;
        public float gravity = 20.0F;
        private Vector3 moveDirection = Vector3.zero;

        void Update()
        {

			if (GameController.GameStates == GameState.Runner) {
				CharacterController controller = GetComponent<CharacterController>();
				if (controller.isGrounded)
				{
					float Hor = Input.GetAxis("Horizontal");
					float Ver = Input.GetAxis("Vertical");

					//only move forward
					if (Ver >= 0)
					{
						moveDirection = new Vector3(Hor, 0, Ver);
						moveDirection = transform.TransformDirection(moveDirection);
						moveDirection *= speed;
						if (Input.GetButton("Jump"))
							moveDirection.y = jumpSpeed;
					}
				}
				moveDirection.y -= gravity * Time.deltaTime;
				controller.Move(moveDirection * Time.deltaTime);
			}
        }

        public void MoveSideWays(bool isLeft)
        {
            CharacterController controller = GetComponent<CharacterController>();

            if (isLeft)
            {
                if (controller.isGrounded)
                {
                    //only move forward
                    moveDirection = new Vector3(-1, 0, 0);
                    moveDirection = transform.TransformDirection(moveDirection);
                    moveDirection *= speed;
                }
            }
            else
            {
                if (controller.isGrounded)
                {
                    //only move forward
                    moveDirection = new Vector3(1, 0, 0);
                    moveDirection = transform.TransformDirection(moveDirection);
                    moveDirection *= speed;
                }

            }

            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }

        /// <summary>
        /// Function to move forward
        /// </summary>
        /// <param name="speedModifier">Allows different kind of speed depending on gesture</param>
        public void MoveForward(float speedModifier)
        {
            CharacterController controller = GetComponent<CharacterController>();
            if (controller.isGrounded)
            {
                moveDirection = new Vector3(0, 0, 1);
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= (speed * speedModifier);
            }
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}


