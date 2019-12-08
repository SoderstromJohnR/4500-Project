using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class navigationController : MonoBehaviour
{
    public void loadNextScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneTransitionManager.Instance.loadNextScene();        
        Debug.Log("loadNextScene firing!");
    }

    public void loadPreviousScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        SceneTransitionManager.Instance.loadPreviousScene();
        Debug.Log("loadPreviousScene firing!");
    }
}
