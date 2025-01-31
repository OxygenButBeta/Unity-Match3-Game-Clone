using System;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    
    Camera _camera;
    private void Awake()
    {
        _camera = Camera.main;
        
        float height = _camera.orthographicSize * 2f;
        float width = height * Screen.width / Screen.height;
        Debug.Log($"Width: {width}, Height: {height}");
    }
}