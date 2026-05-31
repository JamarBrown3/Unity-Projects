using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements.Experimental;

public class ScoreCalculator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     int correctAnswer = 0;
     int questionCompleted = 0;



    public int scoreAnswers()
    {
        if (questionCompleted == 0)
         return 0;
       return (int)((float)correctAnswer/questionCompleted * 100.0f);

    }

    public void incrementCorrectAnswer()
    {
        correctAnswer++;
    }

    public void incrementQuestionCompleted()
    {
        questionCompleted++;
    }

    
}
