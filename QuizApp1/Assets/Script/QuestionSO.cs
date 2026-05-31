using UnityEngine;

[CreateAssetMenu(fileName = "QuestionSO", menuName = "Quiz Question")]
public class QuestionSO : ScriptableObject
{
  [TextArea (2, 6)]
  [SerializeField] private string question = "What is the capital of France?";
  [SerializeField] private string[] answers = new string[4];
  [SerializeField] private int correctAnswerIndex = 0;

  public string Question => question; //public string getQuestion() { return question; }

  public string[] Answers => answers; //public string[] getAnswers() { return answers;}

  public string getAnswer(int index) { return answers[index];}
  public int CorrectAnswerIndex => correctAnswerIndex; //public int getCorrectAnswerIndex() { return correctAnswerIndex;}

  public string getQuestion()
  {
    return question;
  }
  
}
