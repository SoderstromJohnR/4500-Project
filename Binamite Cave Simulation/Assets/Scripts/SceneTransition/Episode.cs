public enum Episode
{
    mainMenu = 0,

    caving1 = 11,
    cavingQuiz1 = 12,

    caving2 = 21,
    cavingQuiz2 = 22,

    caving3 = 31,
    cavingQuiz3 = 32,

    searching1 = 13,
    searchingQuiz1 = 14,

    searching2 = 23,
    searchingQuiz2 = 24,

    searching3 = 33,
    searchingQuiz3 = 34,

    searching4 = 34,
    searchingQuiz4 = 44
}

static class EpisodeExtensionMethods
{
    public static GameMode mode(this Episode episode)
    {
        int modeNum = ((int)episode) % 10;
        return (GameMode)modeNum;
    }

    public static int number(this Episode episode)
    {
        int episodeNumber = ((int)episode) / 10;
        return episodeNumber;
    }

    public static string sceneName(this Episode episode)
    {
        switch (episode)
        {
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
            case Episode.searching3:
                return "SearchingGame3";
            case Episode.searching4:
                return "SearchingGame4";
            case Episode.mainMenu:
                return "MainMenu";
            default:
                return "Default";
        }
    }
}