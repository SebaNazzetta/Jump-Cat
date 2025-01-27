using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject exitConfirmationPanel;

    public bool isVibrationOn = true;

    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void HideSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ShowExitConfirmation()
    {
        HideSettings();
        exitConfirmationPanel.SetActive(true);
    }

    public void HideExitConfirmation()
    {
        exitConfirmationPanel.SetActive(false);
        ShowSettings();
    }

    public void VibrationToggle(bool vibration)
    {
        if(isVibrationOn)
        {
            isVibrationOn = false;
        }
        else
        {
            isVibrationOn = true;
        }
    }

    public void DefaultVibration()
    {
        if(isVibrationOn) Handheld.Vibrate();
    }

    public void MuteToggle(bool muted)
    {
        if (!muted)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
