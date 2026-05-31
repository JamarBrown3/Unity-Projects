using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class NameInput : MonoBehaviour
{
  public TMP_InputField nameText;

  public TMP_Text Hello;

  public void showGreeting()
  {
    Debug.Log("button clicked");

    Hello.text = $"Hello, {nameText.text}! Welcome to our Unity App"; 
  }  


  public void clear()
  {
    Debug.Log("button clicked");

    Hello.text = "";
    nameText.text = "";
  }
  
}
