using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera _camera;

    private IClickable _lastClickable;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            IClickable click = hit.collider.GetComponent<IClickable>();

            if (click != null)
            {
                if (click != _lastClickable)
                {
                    click.OnOverlayEnter();
                }

                if (isUseButtonPressed())
                {
                    click.OnClick();
                }
            }
            else 
            {
                if (_lastClickable != null) 
                {
                    _lastClickable.OnOverlayExit();
                }
            }

            _lastClickable = click;
        }
    }

    public static Vector2 MouseInput()
    {
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    public static bool isMoveButtonPressed()
    {
        return Input.GetMouseButton(1);
    }

    public static bool isUseButtonPressed()
    {
        return Input.GetMouseButtonDown(0);
    }

    public static bool isUpdateButtonPressed()
    {
        return Input.GetKeyDown(KeyCode.F);
    }
}
