using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [SerializeField] QuestionSO question;

    [SerializeField] TMP_Text questionText;

    [SerializeField] Button[] answerButtons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questionText.text = question.getQuestion();
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = question.getAnswer(i);
        }
    }

    public void onAnswerSelected(int index)
    {
        if (index == question.CorrectAnswerIndex)
        {
            Debug.Log("Correct!");
        }
        else
        {
            Debug.Log("Wrong!");
        }
      }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
