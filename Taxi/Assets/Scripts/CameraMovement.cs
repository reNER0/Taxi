using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10;

    private Vector3 _position;

    private Camera _camera;

    private void Start()
    {
        _position = transform.position;
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        if (InputManager.isMoveButtonPressed())
        {
            Vector3 direction = 
                (transform.forward * InputManager.MouseInput().y 
                + transform.right * InputManager.MouseInput().x
                ) * Time.deltaTime * _moveSpeed;

            direction = Quaternion.AngleAxis(-transform.localEulerAngles.x, transform.right) * direction;

            _position -= direction;
        }

        transform.position = Vector3.Lerp(transform.position, _position, Time.deltaTime * _moveSpeed);
    }
}
