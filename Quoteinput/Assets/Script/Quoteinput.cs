using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Quoteinput : MonoBehaviour
{

    public TMP_InputField quoteText;

    public TMP_Text displayQuote;

    public GameObject quitPanel;

    public TMP_Text theTimeText;


    public void clear()
  {
    Debug.Log("button clicked");

    quoteText.text = "";
    displayQuote.text = "";
  }

    public void showQuote()
    {
        Debug.Log("button clicked");

        string quote = quoteText.text;

        // if else statement to check if the quote is empty
        if (quote == "")
        {
            displayQuote.text = "Please enter a quote.";
        }
        else
        {
            displayQuote.text = quote;
        }

        // character count
        int characterCount = quote.Length;
        Debug.Log("Character count: " + characterCount);

        // word count
        String [] words = quote.Split(' ');
        int wordCount = words.Length;
        Debug.Log("Word count: " + wordCount);

        // initialization of the vowelcount = 0;
        int vowelCount = 0;


        for (int i = 0; i < quote.Length; i++)
        {
            char c = quote[i];
            if (c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u' || 
                c == 'A' || c == 'E' || c == 'I' || c == 'O' || c == 'U'
                )
            {
                vowelCount++;
            }
        }
        Debug.Log("Vowel count: " + vowelCount);

        // display the quote, character count, word count and vowel count in the displayQuote text for the unity project
        displayQuote.text = $"Quote of the day: {quote}\nCharacter count: {characterCount}\n" + 
        $"Sentence Analyzer\n" +
        $"Characters: {characterCount}\n" +
        $"Word count: {wordCount}\n" +
        $"Vowel: {vowelCount}";

    }

    public void showQuitPanel()
    {
        quitPanel.SetActive(true);
    }

    public void hideQuitPanel()
    {
        quitPanel.SetActive(false);
    }

    public void quitApplication()
    {   
        Debug.Log("Quitting application");
        Application.Quit();
    }

     public void SaveNote()
    {
        PlayerPrefs.SetString("NoteContent", quoteText.text);
        PlayerPrefs.SetString("Notedisplay", displayQuote.text);
        PlayerPrefs.Save();
    }

    void Start()
    {   
        // Load saved note
        quoteText.text = PlayerPrefs.GetString("NoteContent", "");
        displayQuote.text = PlayerPrefs.GetString("Notedisplay", "");
    } 

    void Update()
    {
        // Show current time
        theTimeText.text = System.DateTime.Now.ToString();
    }
    

}


