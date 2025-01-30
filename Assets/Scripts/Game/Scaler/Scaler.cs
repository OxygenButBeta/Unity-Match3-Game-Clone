using System;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    
    Camera camera;
    private void Awake()
    {
        camera = Camera.main;
        
        float height = camera.orthographicSize * 2f;
        float width = height * Screen.width / Screen.height;
        Debug.Log($"Width: {width}, Height: {height}");
    }
}