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
/// This script defines the behavior of a UI element that displays a series of
/// questions about binary trees.
/// 
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
            feedback.SetText("Try an Integer");
        }
        else if (answerIsCorrect(answerInt))
        {
            // Sets correct answer feedback text
            feedback.color = green;
            feedback.SetText("Correct!");
        }
        else
        {
            // Sets incorrect answer feedback text
            feedback.color = red;
            feedbackText.GetComponent<TextMeshProUGUI>().SetText("Incorrect!");
        }
    }

    // Calls delegate methods associated with the skip button
    public void skipClicked()
    {
        onSkipClicked();
    }

    // Sets the question text to be displayed
    public void setQuestionText(string question)
    {
        questionText.GetComponent<TextMeshProUGUI>().SetText(question);
    }

    // Sets the text on what would otherwise be the skip button
    public void setSkipText(string nextMessage)
    {
        skipText.GetComponent<TextMeshProUGUI>().SetText(nextMessage);
    }

    // Stores text from input
    public void inputTextChanged(string input)
    {
        answerInputString = input;
        Debug.Log("quizDisplayController - answerInputString:" + answerInputString);
    }
}
