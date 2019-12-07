public enum Episode
{
    mainMenu, caving1, caving2, searching1, searching2
}

public enum GameMode
{
    mainMenu, caving, searching, none
}


static class EpisodeMethods
{
    public static GameMode mode(this Episode episode)
    {
        switch (episode)
        {
            case Episode.mainMenu:
                return GameMode.mainMenu;
            case Episode.caving1:
            case Episode.caving2:
                return GameMode.caving;
            case Episode.searching1:
            case Episode.searching2:
                return GameMode.searching;
            default:
                return GameMode.none;
        }
    }

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
            case Episode.searching1:
                return "SearchingGame1";
            case Episode.searching2:
                return "SearchingGame2";
            default:
                return "Default";
        }
    }

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
            case "SearchingGame1":
                return Episode.searching1;
            case "SearchingGame2":
                return Episode.searching2;
            default:
                return Episode.mainMenu;
        }
    }
}