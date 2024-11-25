using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class showFPS : MonoBehaviour
{
    public TMP_Text fpsText;
    public float deltaTime;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
    }
}