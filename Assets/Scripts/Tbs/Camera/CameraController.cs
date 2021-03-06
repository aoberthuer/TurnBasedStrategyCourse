using Cinemachine;
using UnityEngine;

namespace tbs.camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 25f;

        private const float MIN_FOLLOW_Y_OFFSET = 2f;
        private const float MAX_FOLLOW_Y_OFFSET = 12f;

        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

        private CinemachineTransposer _cinemachineTransposer;
        private Vector3 _targetFollowOffset;


        private void Update()
        {
            HandleMove();
            HandleRotation();
            HandleZoom();
        }

        private void Start()
        {
            _cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            _targetFollowOffset = _cinemachineTransposer.m_FollowOffset;
        }


        private void HandleMove()
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
        }

        private void HandleRotation()
        {
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

        private void HandleZoom()
        {
            float zoomAmount = 1f;
            if (Input.mouseScrollDelta.y > 0)
            {
                _targetFollowOffset.y -= zoomAmount;
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                _targetFollowOffset.y += zoomAmount;
            }

            _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

            float zoomSpeed = 5f;
            _cinemachineTransposer.m_FollowOffset =
                Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _targetFollowOffset, Time.deltaTime * zoomSpeed);
        }
    }
}