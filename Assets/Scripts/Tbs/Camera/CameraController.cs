using UnityEngine;

namespace tbs.camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 10f;

        private void Update()
        {
            Vector3 inputMoveDir = new Vector3();

            if (Input.GetKey(KeyCode.W))
            {
                inputMoveDir.z = +1f;
            }

            if (Input.GetKey(KeyCode.S))
            {
                inputMoveDir.z = -1f;
            }

            if (Input.GetKey(KeyCode.D))
            {
                inputMoveDir.x = +1f;
            }

            if (Input.GetKey(KeyCode.A))
            {
                inputMoveDir.x = -1f;
            }

            Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
            transform.position += inputMoveDir * (_moveSpeed * Time.deltaTime);


            Vector3 rotationVector = new Vector3();
            if (Input.GetKey(KeyCode.Q))
            {
                rotationVector.y = +1f;
            }

            if (Input.GetKey(KeyCode.E))
            {
                rotationVector.y = -1f;
            }
            
            transform.eulerAngles += rotationVector * (_rotationSpeed * Time.deltaTime);
        }
    }
}