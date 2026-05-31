using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    public TMP_InputField theText;
    public AudioSource clearSound;
    public GameObject thePanel;
    
    public TMP_Text theTimeText;


    public void ClearText()
    {
        Debug.Log("button clicked");

        theText.text = "";
        clearSound.Play();
    }

    public void CancelButton()
    {
        thePanel.SetActive(false);
    }

    public void CloseButton()
    {
        thePanel.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void SaveNote()
    {
        PlayerPrefs.SetString("NoteContent", theText.text);
        PlayerPrefs.Save();
    }

    public void Start()
    {   
        theText.text = PlayerPrefs.GetString("NoteContent");

    } 

    public void Update()
    {
     theTimeText.text = System.DateTime.Now.ToString();   
    }

}
