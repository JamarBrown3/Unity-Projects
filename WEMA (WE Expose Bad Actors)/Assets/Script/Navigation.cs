using UnityEngine;

public class Navigation : MonoBehaviour
{
    // loading scenes

    // 02_SOS scene is the main scene of the app, where the user can see the map and the SOS button.
    public void loadSOS_Scene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("02_SOS");
    }
    // 00_Login scene is the login scene of the app, where the user can log in or sign up.
    public void loadLogin_Scene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("00_Login");
    }

    //01_Home scene is the home scene of the app, where the user can see the news and the weather.
    public void loadHome_Scene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("01_Home");
    }

    // 03_ReportForm scene is the report form scene of the app, where the user can fill out a form to report an emergency.
    public void loadReportForm_Scene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("03_ReportForm");
    }

    // 04_Profile scene is the profile scene of the app, where the user can see their profile and edit it.
    public void loadProfile_Scene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("04_Profile");
    }

    //05_Contacts scene is the contacts scene of the app, where the user can see their contacts and add new ones.
    public void loadContacts_Scene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("05_Contacts");
    }
    
}
