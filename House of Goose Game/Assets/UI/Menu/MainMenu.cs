using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        AudioManager am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        am.FadeAudioBetween(2f);
        SceneManager.LoadScene("MainScene");
    }

    public void GoToInputScreen()
    {
        SceneManager.LoadScene("InputScreen");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScreen");
    }

    public void GoToEndScreen()
    {
        SceneManager.LoadScene("EndScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
