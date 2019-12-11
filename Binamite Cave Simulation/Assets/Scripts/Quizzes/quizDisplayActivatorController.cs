using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

/// <summary>
/// This class defines a controller that activates a quiz display UI component.
/// </summary>

public class quizDisplayActivatorController : MonoBehaviour
{
    // Difficulty enum to be set in the inspector
    public GradeLevel gradeLevel;

    // Set as quizDisplay prefab in the editor
    [SerializeField] private GameObject quizDisplay;

    // The index of the quiz displayed
    private int questionIndex = 1;

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
        quizDisplayController.OnSkipClicked skipQuestion,
        bool isFinalQuestion = false)
    {
        quizDisplayController quizController = quizDisplay.GetComponent<quizDisplayController>();

        if (!quizDisplay.activeInHierarchy)
        {
            Debug.Log("quizDisplayActivatorController - Quiz Activated!");

            // Activates interruption
            quizDisplay.SetActive(true);
        }

        // Overwrites delegates for yes and no buttons
        quizController.answerIsCorrect = answerIsCorrect;
        quizController.onSkipClicked = skipQuestion;

        // Sets the text
        quizController.setQuestionText(question);
        quizController.setFeedback("");
        if (isFinalQuestion)
        {
            quizController.setSkipText("Done");
        }


        // This "stops time" by disabling scripts that only work while timescale is set to 1
        Time.timeScale = 0;
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

        Debug.Log("Starting quiz");
        Episode currentEpisode = SceneTransitionManager.Instance.currentEpisode;

        switch (currentEpisode)
        {
            // Displays the next quiz question for Go Caving episode 1
            case Episode.caving1:
                //nextCaving1Question();
                break;
            case Episode.caving2:
                //nextCaving2Question();
                break;
            case Episode.caving3:
                //nextCaving3Question();
                break;
            case Episode.searching1:
                nextSearchingQuestionEpisode1();
                break;
            case Episode.searching2:
                //nextSearching2Question();
                break;

            default:
                Debug.LogWarning("quizDisplayActivationController - current episode has no quiz!");
                break;
        }
    }

    // Displays the next searching question common to all searching episodes
    public void nextSearchingQuestionEpisode1()
    {
        // A way of interrupting the player with a prompt and two choices
        playerInterruptionActivatorController interruptor =
            GameObject.Find("playerInterruptionActivator").GetComponent<playerInterruptionActivatorController>();

        // A reference to the most recent GameStats object
        GameStats stats = SceneTransitionManager.Instance.currentGameStats;

        Debug.Log("nextSearchingQuestionEpisode1 firing");
        int hypotheticalHeight; // Stores the height of a hypothetical tree to ask questions about it
        int hypotheticalNumNodes; // Stores the number of nodes in a hypothetical tree

        switch (questionIndex)
        {
            case 1:
                // Question 1
                activateQuizDisplay(
                    question: "How many caves are there?",
                    answerIsCorrect: x => x == stats.getNumCaves(),
                    skipQuestion: nextSearchingQuestionEpisode1
                );
                break;
            case 2:
                // Question 2
                activateQuizDisplay(
                    question: "How many moves did you make to find the pickaxe?",
                    answerIsCorrect: x => x == stats.getNumMoves(),
                    skipQuestion: nextSearchingQuestionEpisode1,
                    isFinalQuestion: gradeLevel < GradeLevel.threeThroughFive
                );
                break;
            case 3:


                /*** QUESTIONS BELOW ARE FOR GRADE 3 OR HIGHER ***/
                if (gradeLevel < GradeLevel.threeThroughFive) { prepareForQuizEnd(); return; }

                // Question 3 - see below this method for definitions of method arguments
                quizDisplay.SetActive(false); // Deactivates quiz display
                interruptor.activateInterrupt(
                    searchingEpisode1Question3YesClicked,
                    searchingEpisode1Question3NoClicked,
                    "Can you make fewer moves and still be guaranteed to find it?", "Yes", "No"
                );
                break;
            case 4:

                // Question 4
                activateQuizDisplay(
                    question: "If there were caves one level deeper, with every cave connected to 2 "
                        + "deeper caves (except the deepest ones and the one at the entrance), how "
                        + "many caves would there be?",
                    answerIsCorrect: x => x ==  numNodesInCompleteTreeOfHeight(stats.getTreeHeight() + 1),
                    skipQuestion: nextSearchingQuestionEpisode1
                );
                break;
            case 5:


                /*** QUESTIONS BELOW ARE FOR GRADE 6 OR HIGHER ***/
                if (gradeLevel < GradeLevel.threeThroughFive) { prepareForQuizEnd(); return; }

                // Question 5
                hypotheticalHeight = stats.getTreeHeight() + 1; // Height of a hypothetical tree
                hypotheticalNumNodes = numNodesInCompleteTreeOfHeight(hypotheticalHeight);

                activateQuizDisplay(
                    question: "If there were caves one level deeper, with every cave connected to 2 "
                        + "deeper caves (except the deepest ones and the one at the entrance), how many times "
                        + "would you have to move before you were guaranteed to find it?",
                    answerIsCorrect: x => x == fastestDfsInACompleteTree(hypotheticalHeight),
                    skipQuestion: nextSearchingQuestionEpisode1
                );

                break;
            case 6:
                /*** QUESTIONS BELOW ARE FOR GRADE 6 OR HIGHER ***/
                if (gradeLevel < GradeLevel.threeThroughFive) { prepareForQuizEnd(); return; }

                // Question 6
                hypotheticalHeight = stats.getTreeHeight() + 2; // Height of a hypothetical tree
                hypotheticalNumNodes = numNodesInCompleteTreeOfHeight(hypotheticalHeight);

                activateQuizDisplay(
                    question: "If there were caves two levels deeper, with every cave connected to 2 "
                        + "deeper caves (except the deepest ones and the one at the entrance), how many times "
                        + "would you have to move before you were guaranteed to find it?",
                    answerIsCorrect: x => x == fastestDfsInACompleteTree(hypotheticalHeight),
                    skipQuestion: nextSearchingQuestionEpisode1
                );
                break;

            default:
                prepareForQuizEnd(); 
                return;
        }

        // Increments question index 
        questionIndex++;

        
    }

    // Returns true if numMoves is the least number of moves before the pickaxe can be found
    bool isFastestDfs(int numMoves, int height)
    {
        return numMoves == fastestDfsInACompleteTree(height);
    }

    // Returns the lowest number of moves to guarantee finding the pickaxe in a tree of height height
    int fastestDfsInACompleteTree(int height)
    {
        return (int)(4 * (Math.Pow(2, height) - 1)) - height;
    }

    // The number of nodes in a tree where every node has 2 children but the leaves
    int numNodesInCompleteTreeOfHeight(int height)
    {
        return (int)(Math.Pow(2, (height + 1)) - 1);
    }

    // Use when ending a quiz
    void prepareForQuizEnd()
    {
        // Resets question index to first question
        questionIndex = 1;

        // Continues the game
        continueGame();
    }

    // Player answered yes to question 3 on searching episode 1 question 3
    public void searchingEpisode1Question3YesClicked()
    {
        Debug.Log("Yes clicked!");

        // A way of interrupting the player with a prompt and two choices
        playerInterruptionActivatorController interruptor =
            GameObject.Find("playerInterruptionActivator").GetComponent<playerInterruptionActivatorController>();

        // A reference to the most recent GameStats object
        GameStats stats = SceneTransitionManager.Instance.currentGameStats;

        int height = stats.getTreeHeight();
        if (isFastestDfs(stats.getNumMoves(), height))
        {
            // Message if yes was clicked but there is no faster way
            interruptor.activateInterrupt(nextSearchingQuestionEpisode1, nextSearchingQuestionEpisode1,
                "Actually, no! " + fastestDfsInACompleteTree(height) + " moves is the the fewest moves you can make"
                + " and be guaranteed to find the pickaxe.", "Cool!", "Sweet."
            );
        }
        else
        {
            // Message if yes was clicked and there is a faster way
            interruptor.activateInterrupt(
                reloadScene, nextSearchingQuestionEpisode1,
                "Right! You can make fewer moves still be guaranteed to find the pickaxe."
                + "Try again?", "Yes", "No"
            );

        }
    }

    // Player answered no to question 3 on searching episode 1 question 3
    public void searchingEpisode1Question3NoClicked()
    {
        Debug.Log("No clicked!");

        // A way of interrupting the player with a prompt and two choices
        playerInterruptionActivatorController interruptor =
            GameObject.Find("playerInterruptionActivator").GetComponent<playerInterruptionActivatorController>();

        // A reference to the most recent GameStats object
        GameStats stats = SceneTransitionManager.Instance.currentGameStats;

        int height = stats.getTreeHeight();
        if (isFastestDfs(stats.getNumMoves(), height))
        {
            // Message if no was clicked and there is no faster way
            Debug.Log("Is fastest");
            interruptor.activateInterrupt(nextSearchingQuestionEpisode1, nextSearchingQuestionEpisode1,
                "Correct! " + fastestDfsInACompleteTree(height) + " moves is the fewest you can make and still"
                + " be guaranteed to find the pickaxe.", "Cool!", "Sweet."
            );
        }
        else
        {
            // Message if no was clicked but there is a faster way
            interruptor.activateInterrupt(reloadScene, nextSearchingQuestionEpisode1,
                "Actually, you can! You can make fewer moves and be guaranteed to find the pickaxe. "
                + "Would you like to try again?", "Yes", "No"
            );
        }
    }
 
    // An empty method to pass to the showQuiz method
    void Empty() { }

    // This method loads the next scene
    void Skip()
    {
        SceneTransitionManager.Instance.loadNextScene();
    }

    // This method reloads the current scene
    void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}