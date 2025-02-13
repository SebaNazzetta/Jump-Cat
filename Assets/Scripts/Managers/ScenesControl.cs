using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesControl : MonoBehaviour
{
    public void World1GameplayScene()
    {
        SceneManager.LoadScene("1_World_Gameplay");
    }


    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
