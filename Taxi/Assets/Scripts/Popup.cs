using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private float _time = 1f;
    [SerializeField] private float _speed = 0.25f;

    private void Start()
    {
        Destroy(gameObject, _time);
    }
    private void Update()
    {
        transform.position += Time.deltaTime * _speed * Vector3.up;
    }

    public void DrawValue(int value) 
    {
        Color color;
        string text;
        if (value < 0)
        {
            color = Color.red;
            text = "-";
        }
        else 
        {
            color = Color.yellow;
            text = "+";
        }
        GetComponent<Text>().color = color;
        GetComponent<Text>().text = text + Mathf.Abs(value) + "$";
    }
}
