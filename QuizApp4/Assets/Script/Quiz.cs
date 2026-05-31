using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;
using Button = UnityEngine.UI.Button;
using NUnit.Framework;

public class Quiz : MonoBehaviour
{
    [SerializeField] private QuestionSO[] questions;

    [SerializeField] private TMP_Text questionText;

    [SerializeField] private Button[] answerButtons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private TMP_Text correctOrNotText;

    [SerializeField] private TMP_Text scoreText;
    ScoreCalculator scoreCalculator;

    [SerializeField] Slider progressBar;

    [SerializeField] GameObject endScreen;

    [SerializeField] private TMP_Text finalScoreText;



    public bool isFinished = false;

    int index = 0;

    float score = 0.0f;

    void Start()
    {
        scoreCalculator = FindFirstObjectByType<ScoreCalculator>();

        // Safety: Check if we actually have questions assigned
        if (questions.Length > 0)
        {
            questionText.text = questions[0].getQuestion();

            for (int i = 0; i < answerButtons.Length; i++)
            {
                // Safety: Check if the button slot in the Inspector is actually filled
                if (answerButtons[i] != null)
                {
                    answerButtons[i].GetComponentInChildren<TMP_Text>().text = questions[0].getAnswer(i);
                }
                else
                {
                    Debug.LogWarning("Button at index " + i + " is missing in the Inspector!");
                }
            }

            progressBar.maxValue = questions.Length - 1;
            progressBar.value = 0;
        }
    }

    public void onAnswerSelected(int buttonIndex)
    {
        scoreCalculator.incrementQuestionCompleted();

        if (buttonIndex == questions[this.index].CorrectAnswerIndex)
        {
            scoreCalculator.incrementCorrectAnswer();
            Debug.Log("Correct!");
            correctOrNotText.text = "Correct!";
            correctOrNotText.color = Color.green;

        }
        else
        {
            Debug.Log("Wrong!");
            correctOrNotText.text = "Wrong!";
            correctOrNotText.color = Color.red;
        }
        score = scoreCalculator.scoreAnswers();
        setButtonState(false);


        //for (int i = 0; i < answerButtons.Length; i++)
        //{
        //    answerButtons[i].interactable = false;
        // }

    }




    // Update is called once per frame
    void Update()
    {
        if (scoreText != null)
        {


            scoreText.text = "Score: " + score + "%";
        }
    }

    // setButtonState

    void setButtonState(bool state)
    {
        //foreach (Button button in answerButtons)
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = state;

        }
    }
    void DisplayQuestion()
    {
        questionText.text = questions[index].getQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TMP_Text buttonText = answerButtons[i].GetComponentInChildren<TMP_Text>();
            buttonText.text = questions[index].getAnswer(i);
        }
    }

    public void OnNextButtonClicked()
    {
        correctOrNotText.text = "";
        index++;
        if (index >= questions.Length)
        {
            isFinished = true;
            showEndScreen();
            finalScore();
            return;
        }
        DisplayQuestion();
        setButtonState(true);
        progressBar.value = index;
    }

    public void showEndScreen()
    {
        endScreen.SetActive(true);
    }

    public string finalScore()
    {
        finalScoreText.text = "Final Score: " + score + "%";

        return finalScoreText.text;

    }

    public void resetQuiz()
    {
        index = 0;
        isFinished = false;
        endScreen.SetActive(false);
        score = 0;

        correctOrNotText.text = "";

        progressBar.value = 0;

        setButtonState(true);



        DisplayQuestion();
    }




}
