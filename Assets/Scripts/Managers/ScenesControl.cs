using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesControl : MonoBehaviour
{
    public void PortraitScene()
    {
        SceneManager.LoadScene("GameplayPortrait");
    }

    public void LandscapeScene()
    {
        SceneManager.LoadScene("GameplayLandscape");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
