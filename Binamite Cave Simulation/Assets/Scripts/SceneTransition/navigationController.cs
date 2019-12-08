using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// This class provides methods that facilitate the changing of scenes when the player 
/// presses a button. It defines two methods assigned to buttons in the navigationButtons,
/// navigationLeftMainMenu, and navigationRightMainMenu prefabs.
/// </summary>
/// 
public class navigationController : MonoBehaviour
{
    // Calls loadNextScene() on the SceneTransitionManager instance
    public void loadNextScene()
    {
        SceneTransitionManager.Instance.loadNextScene();        
        Debug.Log("loadNextScene firing!");
    }

    // Calls loadPreviousScene() on the SceneTransitionManager instance
    public void loadPreviousScene()
    {
        SceneTransitionManager.Instance.loadPreviousScene();
        Debug.Log("loadPreviousScene firing!");
    }
}
