using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuNavigation : MonoBehaviour
{
    void Awake()
    {
        // Initializes scene transition manager instance
        SceneTransitionManager.Instance.initialize();
    }

    public void PlayCavingGame()
    {
        print("PlayCavingGame firing!");

        SceneManager.LoadScene("CavingGame1");

    }

    public void PlaySearchingGame()
    {
        print("PlaySearchingGame firing!");

        SceneManager.LoadScene("SearchingGame1");
    }


    public void Exit()
    {
        print("Exit firing!");

        Application.Quit();
    }
}
