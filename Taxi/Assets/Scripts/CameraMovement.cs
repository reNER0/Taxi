using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10;

    private Vector3 _position;


    private void Start()
    {
        _position = transform.position;
    }

    private void Update()
    {
        MoveCamera();
    }


    private void MoveCamera()
    {
        if (CustomInput.IsMoveButtonPressed())
        {
            Vector3 direction = 
                (transform.forward * CustomInput.MouseInput().y 
                + transform.right * CustomInput.MouseInput().x
                ) * Time.deltaTime * _moveSpeed;

            direction = Quaternion.AngleAxis(-transform.localEulerAngles.x, transform.right) * direction;

            _position -= direction;
        }

        transform.position = Vector3.Lerp(transform.position, _position, Time.deltaTime * _moveSpeed);
    }
}
