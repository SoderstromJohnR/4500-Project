using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

/// <summary>
/// This class defines a singleton object which persists throughout each game for managing the preservation of game 
/// objects between scenes, selecting scenes to load, and storing a record of player activity using a public 
/// GameStats object.
/// </summary>
public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    public GameStats currentGameStats; // Statistics on player actions in the current game
    public List<GameStats> gameStatsList; // Collection of all game statistics
    public Episode currentEpisode; // The current episode, obtained from the scene name and episode enum

    // The set of transitions to be substituted with a transition to the main menu
    string[] mainMenuTransition = { "CavingGame3", "SearchingGame1" };

    // Called by the singleton class, intitializes gameStatsList and adds onSceneLoaded as a SceneManager delegate
    protected SceneTransitionManager()
    {
        Debug.Log("CONSTRUCTING SceneTransitionManager");

        gameStatsList = new List<GameStats>();

        // Sets prepareForScene as SceneManager delegate method
        SceneManager.sceneLoaded += onSceneLoad;
    }

    // This method will trigger the construction of a SceneTransitionManager if one doesn't exist
    public void initialize()
    {
        Debug.Log("initialize called on scene transition manager");
        prepareForScene(SceneManager.GetActiveScene());
    }

    // This method can be called to ensure a singleton SceneTransitionManager instance exists
    public void empty() { }

    // SceneManager calls this method when a new scene loads
    void onSceneLoad(Scene scene, LoadSceneMode mode)
    {
        prepareForScene(scene);
    }

    /* This method creates a new GameStats object, assigns it to currentGameStats, and adds
     * it to gameStatsList. It is called by SceneManager whenever a new scene is loaded, 
     * before start functions execute. */
    void prepareForScene(Scene scene)
    {
        Debug.Log("SceneTransitionManager - Scene loaded: " + scene.name);

        newStats(scene.name); // Adds new GameStats object
        currentEpisode = EpisodeMethods.fromSceneName(scene.name);

        Debug.Log("Current Episode: " + currentEpisode.sceneName());
    }

    /* This method creates a new GameStats object, assigns it to currentGameStats, and adds
     * it to gameStatsList. */
    void newStats(string sceneName)
    {
        Debug.Log("Adding newStats - " + sceneName);
        sceneName = sceneName.Remove(sceneName.Length - 1);

        switch (sceneName)
        {
            case "SearchingGame":
                currentGameStats = new SearchingGameStats();
                break;
            case "CavingGame":
                currentGameStats = new CavingGameStats();
                break;
        }

        gameStatsList.Add(currentGameStats);
    }

    // Loads the next episode in the episode in the queue for the current game mode
    public void loadNextScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        string currentSceneName = SceneManager.GetActiveScene().name;
        string nextSceneName = sceneNameByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);

        Debug.Log("SceneTransitionManager.loadNextScene() - currentSceneName: " + currentSceneName);
        Debug.Log("SceneTransitionManager.loadNextScene() - nextSceneName: " + nextSceneName);
        Debug.Log("SceneTransitionManager.loadNextScene() - mainMenuTransition[0]: " + mainMenuTransition[0]);
        Debug.Log("SceneTransitionManager.loadNextScene() - mainMenuTransition[1]: " + mainMenuTransition[1]);

        if (nextSceneName.Length == 0
            || mainMenuTransition[0] == currentSceneName && mainMenuTransition[1] == nextSceneName)
        {
            nextSceneName = "MainMenu";
            Debug.Log("SceneTransitionManager.loadNextScene() - loading " + nextSceneName);
        }
        SceneManager.LoadScene(nextSceneName);
    }

    // Loads the previous scene in the queue for the current game mode, or the main menu
    public void loadPreviousScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        string currentSceneName = SceneManager.GetActiveScene().name;
        string previousScene = sceneNameByBuildIndex(SceneManager.GetActiveScene().buildIndex - 1);

        Debug.Log("SceneTransitionManager.loadNextScene() - currentSceneName: " + currentSceneName);
        Debug.Log("SceneTransitionManager.loadNextScene() - nextSceneName: " + previousScene);
        Debug.Log("SceneTransitionManager.loadNextScene() - mainMenuTransition[1]: " + mainMenuTransition[1]);
        Debug.Log("SceneTransitionManager.loadNextScene() - mainMenuTransition[0]: " + mainMenuTransition[0]);
        if (previousScene.Length == 0
            || mainMenuTransition[1] == currentSceneName && mainMenuTransition[0] == previousScene)
        {
            previousScene = "MainMenu";
            Debug.Log("SceneTransitionManager.loadNextScene() - nextSceneName in transition: " + previousScene);
        }
        SceneManager.LoadScene(previousScene);
    }

    // Loads SearchingGame1
    public void startSearchingGame()
    {
        SceneManager.LoadScene("SearchingGame1");
    }

    // Loads CavingGame1
    public void startCavingGame()
    {
        SceneManager.LoadScene("CavingGame1");
    }

    // Prevents any objects with the passed tag from being destroyed when a new scene loads
    private void preserveObjectsWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            DontDestroyOnLoad(obj);
        }
    }

    // Returns the name of the scene with the passed build index
    private static string sceneNameByBuildIndex(int buildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(buildIndex);

        // Returns an empty string if the path doesn't exist
        if (path.Length == 0) return "";

        // Finds index of the beginning of the name
        int beginning = path.LastIndexOf("\\") + 1;
        if (beginning <= 0) { beginning = path.LastIndexOf("/") + 1; };

        // Finds index of the end of the name
        int end = path.LastIndexOf(".") - 1;

        // Finds name substring
        string name = path.Substring(beginning, end - beginning + 1);

        return name;
    }
}

