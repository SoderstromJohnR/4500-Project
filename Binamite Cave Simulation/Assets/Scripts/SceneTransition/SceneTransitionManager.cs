using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

/// <summary>
/// This class defines a singleton object which persists throughout each game,
/// manages the preservation of game objects between scenes, and records
/// player statistics using a public GameStats object.
/// </summary>
public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    public GameStats currentGameStats; // Statistics on player actions in the current game
    public List<GameStats> gameStatsList; // Collection of all game statistics

    // The set of transitions to be substituted with a transition to the main menu
    (string, string) mainMenuTransition = ("SearchingGame2", "CavingGame1");

    protected SceneTransitionManager()
    {
        Debug.Log("CONSTRUCTING SceneTransitionManager");

        gameStatsList = new List<GameStats>();

        // Sets newStats as SceneManager delegate method
        SceneManager.sceneLoaded += newStats;
    }

    // This method will trigger the construction of a SceneTransitionManager if one doesn't exist
    public void initialize()
    {
        Debug.Log("initialize called on scene transition manager");
    }

    /* This method creates a new GameStats object, assigns it to currentGameStats, and adds
     * it to gameStatsList. It is called by SceneManager whenever a new scene is loaded, 
     * before start functions execute. */
    void newStats(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("SceneTransitionManager - Scene loaded: " + scene.name);

        string name = scene.name;
        name = name.Remove(name.Length - 1);

        switch (name)
        {
            case "SearchingGame":
                newSearchingStats();
                break;
            case "CavingGame":
                newCavingStats();
                break;
        }
    }

    // Loads the next episode in the episode in the queue for the current game mode
    public void loadNextEpisode()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        string currentSceneName = SceneManager.GetActiveScene().name;
        string nextSceneName = sceneNameByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);

        /*if (treePreservingTransitions.Contains((currentSceneName, nextSceneName)))
        {
            preserveTree();
        }
        else */
        
        if (mainMenuTransition == (currentSceneName, nextSceneName))
        {
            nextSceneName = "MainMenu";
        }
        SceneManager.LoadScene(nextSceneName);
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

    // Creates and adds a new current SearchingGameStats object and adds it to the list
    private void newSearchingStats()
    {
        currentGameStats = new SearchingGameStats();
        gameStatsList.Add(currentGameStats);
    }

    // Creates and adds a new current CavingGameStats object and adds it to the list
    private void newCavingStats()
    {
        currentGameStats = new CavingGameStats();
        gameStatsList.Add(currentGameStats);
    }

    // Stops the tree from being destroyed
    private void preserveTree()
    {
        Debug.Log("SceneTransitionManager - preserveTree() called!");
        /*preserveObjectsWithTag("Node");
        preserveObjectsWithTag("Path");*/
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

/* ABANDONDED IMPLEMENTATION:

public string MyTestString = "WOHOO! SceneTransitionManager WORKS!!!";

public GameStats currentGameStats; // Statistics on player actions in the current game
public List<GameStats> gameStatsList; // Collection of all game statistics

// Queue of episodes in the searching game mode
private Episode[] searchingEpisodeQueue = new Episode[] { 
    Episode.searching1, Episode.searching2
};

// Queue of episodes in the caving game mode
private Episode[] cavingEpisodeQueue = new Episode[]
{
    Episode.caving1, Episode.caving2
};

private Episode[] defaultQueue = new Episode[] { };

private Episode currentEpisode;
private int currentEpisodeIndex; // The index of the current episode in either queue
private HashSet<Episode> treelessEpisodes; // Scenes that use the previous scene's tree

// Prevents non-singleton constructor use
protected SceneTransitionManager() {
    Debug.Log("CONSTRUCTING SceneTransitionManager");
    gameStatsList = new List<GameStats>();

    // Initializes hash-set of Episodes that use the previous episode's tree
    treelessEpisodes = new HashSet<Episode>();
    treelessEpisodes.Add(Episode.searching2);
    treelessEpisodes.Add(Episode.searching4);

    // Sets currentEpisodeIndex and currentEpisode to MainMenu
    currentEpisodeIndex = -1; // Sentinal for mainMenu
    currentEpisode = Episode.mainMenu;
}

// Loads the next episode in the episode in the queue for the current game mode
public void loadNextEpisode()
{
    Debug.Log("SceneTransitionManager - loadNextEpisode called!");
    Episode[] episodeQueue = defaultQueue;
    Episode episodeToLoad = Episode.mainMenu;

    // Selects the episodeQueue that corresponds to the current game mode
    switch (currentEpisode.mode())
    {
        case GameMode.searching:
            episodeQueue = searchingEpisodeQueue;

            break;

    }

    // Sets episodeToLoad to next episode in queue if it exists or mainMenu if not
    currentEpisodeIndex++;
    if (-1 < currentEpisodeIndex && currentEpisodeIndex < episodeQueue.Length)
    {
        episodeToLoad = episodeQueue[currentEpisodeIndex];
    }
    else
    {
        currentEpisodeIndex = -1; // Sentinal for mainMenu
    }
    currentEpisode = episodeToLoad;

    Debug.Log("Loading episode " + episodeToLoad.sceneName());

    // Preserves the tree if sceneToLoad doesn't have a tree
    if (treelessEpisodes.Contains(episodeToLoad))
    {
        Debug.Log("preserving tree");
        preserveTree();
    }

    SceneManager.LoadScene(episodeToLoad.sceneName());
}

public void startSearchingGame()
{
    currentEpisodeIndex = 0;
    currentEpisode = searchingEpisodeQueue[currentEpisodeIndex];
    SceneManager.LoadScene(currentEpisode.sceneName());
}

public void startCavingGame()
{
    currentEpisodeIndex = 0;
    currentEpisode = cavingEpisodeQueue[currentEpisodeIndex];
    SceneManager.LoadScene(currentEpisode.sceneName());
}*/
