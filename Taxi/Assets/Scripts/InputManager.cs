using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera _camera;

    private List<IClickable> _lastClickablesList = new List<IClickable>();

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(_camera.ScreenPointToRay(Input.mousePosition), 10f);

        List<IClickable> currentClickablesList = new List<IClickable>();

        foreach (RaycastHit hit in hits)
        {
            currentClickablesList.AddRange(hit.collider.GetComponentsInChildren<IClickable>());
        }

        foreach (IClickable click in currentClickablesList)
        {
            if (!_lastClickablesList.Contains(click))
            {
                click.OnOverlayEnter();
                _lastClickablesList.Add(click);
            }
            else 
            {
                click.OnOverlayStay();
            }

            if (isClicked())
            {
                click.OnClick();
            }
        }

        for (int i = 0; i < _lastClickablesList.Count; i++)
        {
            if (!currentClickablesList.Contains(_lastClickablesList[i]))
            {
                _lastClickablesList[i].OnOverlayExit();
                _lastClickablesList.Remove(_lastClickablesList[i]);
                i--;
            }
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

    public static bool isClicked()
    {
        return Input.GetMouseButtonDown(0);
    }
}
