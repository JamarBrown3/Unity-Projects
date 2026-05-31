using TMPro;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
   public TMP_InputField usernameInput;

   public TMP_InputField passwordInput;

   public TMP_InputField number1;

   public TMP_InputField number2;


   public TMP_Text messageText;

   public TMP_Text computationResult;

   public TMP_InputField fahrenheitInput;
    public TMP_Text resultText;

   public void OnloginButtonClick()
{
    string username = usernameInput.text;
    string password = passwordInput.text; 

    if (username == "admin" && password == "1234")
        {
            messageText.text = "Login successful!";
            messageText.color = Color.green;
        }  

    else
        {
            messageText.text = "Invalid username or Password";
            messageText.color = Color.red;
        }
}

public void OnRestButtonClick()
    {
        usernameInput.text = "" ;
        passwordInput.text = "";
        messageText.text = "";
    }

public void calculatorAdd()
    {
       double number_1 = double.Parse(number1.text);

       double number_2 = double.Parse(number2.text);

       double total = number_1 + number_2;

       string result = "The result is:" + total.ToString("F2");

       computationResult.color = Color.darkKhaki;

       computationResult.text = result;
    }

    public void calculatorSub()
    {
       double number_1 = double.Parse(number1.text);

       double number_2 = double.Parse(number2.text);

       double total = number_1 - number_2;

       string result = "The result is:" + total.ToString("F2");

       computationResult.color = Color.darkKhaki;

       computationResult.text = result;
    }

    public void calculatorMul()
    {
       double number_1 = double.Parse(number1.text);

       double number_2 = double.Parse(number2.text);

       double total = number_1 * number_2;

       string result =  "The result is:" + total.ToString("F2");

       computationResult.color = Color.darkKhaki;

       computationResult.text = result;
    }

    public void calculatorDiv()
    {
       double number_1 = double.Parse(number1.text);

       double number_2 = double.Parse(number2.text);

       double total = number_1 / number_2;

       string result = "The result is:" + total.ToString("F2");

       computationResult.color = Color.darkKhaki;

       computationResult.text = result;
    }

    public void Convert()
    {
        double f = double.Parse(fahrenheitInput.text);
        double c = (f - 32) * 5 / 9;
        resultText.text =  "C°" + c.ToString("F2");
    }
    
    
}


