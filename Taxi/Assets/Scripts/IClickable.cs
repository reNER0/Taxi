using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable
{
    void OnOverlayEnter();
    void OnOverlayExit();
    void OnClick();
}
