using UnityEngine;
using UnityEngine.UI;

public class SOSManager : MonoBehaviour
{
    // Drag your Toggles from the Hierarchy into these slots in the Inspector
    public Toggle toggle911;
    public Toggle togglePolice;
    public Toggle toggleContacts;

    // Use this for your Submit Button
    public void ExecuteSOS()
    {
        if (toggle911 != null && toggle911.isOn)
        {
            Application.OpenURL("tel:911");
        }

        if (togglePolice != null && togglePolice.isOn)
        {
            // You can change this to a specific precinct number if needed
            Application.OpenURL("tel:911"); 
        }

        if (toggleContacts != null && toggleContacts.isOn)
        {
            #if UNITY_IOS
            Application.OpenURL("addressbook://");
            #else
            Debug.Log("Contacts shortcut is platform-specific.");
            #endif
        }
    }
}