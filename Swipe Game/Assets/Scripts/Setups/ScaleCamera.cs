using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCamera : MonoBehaviour
{
    [SerializeField] private float cameraSize = 5f; // Нужное значение размера камеры
    [SerializeField] private float widthAspect = 16.0f; // Соотношение под которое подобран размер
    [SerializeField] private float hightAspect = 9.0f; // Соотношение под которое подобран размер
    private new Camera camera;

    private void Start()
    {
        Log.WriteLog("Set camera size.", Log.LevelsOfLogs.INFO, "ScaleCamera");
        camera = GetComponent<Camera>();
        float desiredAspect = widthAspect / hightAspect;
        float aspect = camera.aspect;
        float ratio = desiredAspect / aspect;
        camera.orthographicSize = cameraSize * ratio;
    }
}
