using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [SerializeField] QuestionSO[] questions;

    [SerializeField] TMP_Text questionText;

    [SerializeField] Button[] answerButtons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] TMP_Text correctOrNotText;
    int index = 0;
    void Start()
    {
        questionText.text = questions[0].getQuestion();
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = questions[0].getAnswer(i);
        }
    }

    public void onAnswerSelected(int buttonIndex)
    {
        if (buttonIndex == questions[this.index].CorrectAnswerIndex)
        {
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

        setButtonState(false);

            
        //for (int i = 0; i < answerButtons.Length; i++)
        //{
        //    answerButtons[i].interactable = false;
       // }

      }


    

    // Update is called once per frame
    void Update()
    {
        
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

        for (int i=0; i < answerButtons.Length; i++)
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
            index = 0;
        }
        DisplayQuestion();
        setButtonState(true);
    }

    
}
