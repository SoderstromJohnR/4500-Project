/* This file contains enumerated types useful for identifying episodes, 
 * scene names, and game modes. It needs to be updated when new scenes 
 * and game modes are added. */

/// <summary>
/// The Episode enum contains one entry for each episode, mainMenu, and none.
/// </summary>
public enum Episode
{
    mainMenu, caving1, caving2, caving3, searching1, searching2, none
}

/// <summary>
/// The GameMode enum contains an entry for each game mode, mainMenu, and none.
/// </summary>
public enum GameMode
{
    mainMenu, caving, searching, none
}

/// <summary>
/// EpisodeMethods defines static extension methods for retrieving a GameMode from
/// the corresponding episode, a scene name from the corresponding episode, or an
/// episode from a string matching the scene name of an episode.
/// </summary>
static class EpisodeMethods
{
    // Returns the GameMode of an Episode
    public static GameMode mode(this Episode episode)
    {
        switch (episode)
        {
            case Episode.mainMenu:
                return GameMode.mainMenu;
            case Episode.caving1:
            case Episode.caving2:
            case Episode.caving3:
                return GameMode.caving;
            case Episode.searching1:
            case Episode.searching2:
                return GameMode.searching;
            default:
                return GameMode.none;
        }
    }

    // Returns the name of a scene corresponding to an episode
    public static string sceneName(this Episode episode)
    {
        switch (episode)
        {
            case Episode.mainMenu:
                return "MainMenu";
            case Episode.caving1:
                return "CavingGame1";
            case Episode.caving2:
                return "CavingGame2";
            case Episode.caving3:
                return "CavingGame3";
            case Episode.searching1:
                return "SearchingGame1";
            case Episode.searching2:
                return "SearchingGame2";
            default:
                return "Default";
        }
    }

    // Returns an Episode corresponding to a scene name
    public static Episode fromSceneName(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
                return Episode.mainMenu;
            case "CavingGame1":
                return Episode.caving1;
            case "CavingGame2":
                return Episode.caving2;
            case "CavingGame3":
                return Episode.caving3;
            case "SearchingGame1":
                return Episode.searching1;
            case "SearchingGame2":
                return Episode.searching2;
            default:
                return Episode.none;
        }
    }
}