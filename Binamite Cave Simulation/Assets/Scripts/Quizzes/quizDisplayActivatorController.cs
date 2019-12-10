using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class defines a controller that activates a quiz display UI component.
/// </summary>

public class quizDisplayActivatorController : MonoBehaviour
{
    // Set as quizDisplay prefab in the editor
    [SerializeField] private GameObject quizDisplay;

    // The index of the quiz displayed
    private int questionIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Disables the interruption object by default
        quizDisplay.SetActive(false);
    }

    /* Sets the assigned quizDisplay game object to active, sets method arguments as delegates,
     * stops time, and passes arguments representing new message text*/
    public void activateQuizDisplay(string question, 
        quizDisplayController.OnEnterClicked answerIsCorrect, 
        quizDisplayController.OnSkipClicked skipQuestion)
    {
        if (!quizDisplay.activeInHierarchy)
        {
            Debug.Log("quizDisplayActivatorController - Quiz Activated!");

            quizDisplayController quizController = quizDisplay.GetComponent<quizDisplayController>();

            // Activates interruption
            quizDisplay.SetActive(true);

            // Overwrites delegates for yes and no buttons
            quizController.answerIsCorrect = answerIsCorrect;
            quizController.onSkipClicked = skipQuestion;

            // Sets the text of the question
            quizController.setQuestionText(question);

            // This "stops time" by disabling scripts that still work while timescale is set to 1
            Time.timeScale = 0;
        }
    }

    // This method returns the timescale to 1 and movement resumes
    private void continueGame()
    {
        Debug.Log("quizDisplayActivatorController - Game Continued");
        quizDisplay.SetActive(false);

        // Enables scripts that require a non-zero timescale
        Time.timeScale = 1;
    }

    // Starts the quiz appropriate to the current episode
    public void startQuiz()
    {
        Episode currentEpisode = SceneTransitionManager.Instance.currentEpisode;

        switch (currentEpisode)
        {
            // Displays the next quiz question for Go Caving episode 1
            /*case Episode.caving1:
                nextCaving1Question();
                break;
            case Episode.caving2:
                nextCaving2Question();
                break;
            case Episode.caving3:
                nextCaving3Question();
                break;
                */

            case Episode.searching1:
                nextSearching1Question();
                break;

                /*
            case Episode.searching2:
                nextSearching2Question();
                break;
                */

            default:
                Debug.LogWarning("quizDisplayActivationController - current episode has no quiz!");
                break;
        }
    }

    // Displays the next caving question on the quizDisplay, which is to be set in the inspector
    public void nextSearching1Question()
    {
        GameStats stats = SceneTransitionManager.Instance.currentGameStats;

        switch (questionIndex)
        {
            case 0:
                questionIndex++;
                continueGame();
                activateQuizDisplay(question: "How many caves are there?",
                    answerIsCorrect: x => x == stats.getNumCaves(),
                    skipQuestion: nextSearching1Question);
                
                Debug.Log("Question index: " + questionIndex);
                break;
            case 1:
                activateQuizDisplay(question: "How many moves did you make?",
                    answerIsCorrect: x => x == stats.getNumMoves(),
                    skipQuestion: SceneTransitionManager.Instance.loadNextScene);
                Debug.Log("CASE 1 FIRING");

                // Resets index at the end of the quiz
                questionIndex = 0;

                break;
            default:
                Debug.LogWarning("Warning: nextCaving1Question fell through!");
                break;
        }
    }

    // An empty method to pass to the showQuiz method
    void Empty() { }

    void Skip()
    {
        SceneTransitionManager.Instance.loadNextScene();
    }
}