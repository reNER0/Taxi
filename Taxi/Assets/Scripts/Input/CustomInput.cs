using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInput : MonoBehaviour
{
    public static Vector2 MouseInput()
    {
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    public static bool IsMoveButtonPressed()
    {
        return Input.GetMouseButton(1);
    }

    public static bool IsClicked()
    {
        return Input.GetMouseButtonDown(0);
    }
}
