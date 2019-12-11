using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro; //The TextMeshPro library

// Define delegates for methods to call when yes or no are clicked on an interruption
public delegate void OnEnterClicked();
public delegate void OnSkipClicked();

/// <summary>
/// This script defines the behavior of a UI element that displays a question
/// about binary trees. It is part of the questionSystem prefab.
/// 
/// The text of the question can be set with the method setQuestionText. When the user
/// enters an answer that cannot be converted to an integer, the message "Try an integer" 
/// will be displayed in the feedback text. Otherwise, the answer is converted to an integer,
/// and presses the enter button, it will be passed as an argument to the OnEnterClicked delegate
/// functions assigned to answerIsCorrect, which should return true if it is. If this function does 
/// return true, "Correct!" will be displayed and the text on the "Skip" button will be changed to 
/// "Next." Otherwise, "Incorrect" is displayed.
/// 
/// When the player enters a string that can't be converted to an integer, the message
/// "Try an integer" will be displayed in the feedback text.
/// </summary>

public class quizDisplayController : MonoBehaviour
{
    // Delegate definitions
    public delegate bool OnEnterClicked(int answer);
    public delegate void OnSkipClicked();

    // Delegate functions
    public OnEnterClicked answerIsCorrect;
    public OnSkipClicked onSkipClicked;

    // Color definitions
    Color32 green = new Color32(0, 128, 0, 255);
    Color32 red = new Color32(255, 0, 0, 255);

    // TextMeshPro game objects to be set in inspector
    public GameObject questionText; // The text field with the question
    public GameObject feedbackText; // The text field with the feedback
    public GameObject enterText;    // The text on the enter button
    public GameObject skipText;     // The text on the skip button

    public GameObject answerTextField;      // The text field where the player enters an answer

    private string answerInputString = "";  // The input string, updated by inputTextChanged

    // Calls delegate methods associated with the enter button
    public void enterClicked()
    {
        // Gets the TextMeshProUGUI component of the feedbackText
        TextMeshProUGUI feedback = feedbackText.GetComponent<TextMeshProUGUI>();

        // Parses input text to an integer
        int answerInt = 0;
        bool answerIsAnInteger = Int32.TryParse(answerInputString, out answerInt);

        // Sets feedback text
        if (!answerIsAnInteger)
        {
            // Sets feedback if the answer is not an integer
            feedback.color = red;
            feedback.SetText("Invalid");
        }
        else if (answerIsCorrect(answerInt))
        {
            // Sets correct answer feedback text and skip button text to "Next"
            feedback.color = green;
            feedback.SetText("Correct!");
            skipText.GetComponent<TextMeshProUGUI>().SetText("Next");
        }
        else
        {
            // Sets incorrect answer feedback text
            feedback.color = red;
            feedbackText.GetComponent<TextMeshProUGUI>().SetText("Try Again");
        }
    }

    // Calls delegate methods associated with the skip button
    public void skipClicked()
    {
        Debug.Log("On skip clicked: " + onSkipClicked);
        Debug.Log("answerIsCorrect: " + answerIsCorrect);
        onSkipClicked();
    }

    // Sets the question text to be displayed
    public void setQuestionText(string question)
    {
        Debug.Log("quizDisplayController - setting question: " + question);
        questionText.GetComponent<TextMeshProUGUI>().SetText(question);
    }

    // Sets the feedback text
    public void setFeedback(string feedback)
    {
        feedbackText.GetComponent<TextMeshProUGUI>().SetText(feedback);
    }

    // Sets the text on what would otherwise be the skip button
    public void setSkipText(string nextMessage)
    {
        Debug.Log("quizDisplayController - setting nextMessage: " + nextMessage);
        skipText.GetComponent<TextMeshProUGUI>().SetText(nextMessage);
    }

    // Stores text from input. Should be an onTextChanged event for the text field in the inspector
    public void inputTextChanged(string input)
    {
        answerInputString = input;
    }
}
