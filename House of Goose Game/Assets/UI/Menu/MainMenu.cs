using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1)
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
