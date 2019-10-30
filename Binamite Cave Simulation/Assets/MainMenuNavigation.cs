using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuNavigation : MonoBehaviour
{
    public void PlayCavingGame()
    {
        print("PlayCavingGame firing!");

        SceneManager.LoadScene(1);

    }

    public void PlaySearchingGame()
    {
        print("PlaySearchingGame firing!");

        SceneManager.LoadScene(2);
    }


    public void Exit()
    {
        print("Exit firing!");

        Application.Quit();
    }
}
