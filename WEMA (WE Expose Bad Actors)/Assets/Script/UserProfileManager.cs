using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserProfileManager : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField medicalInputField;
    [SerializeField] private TMP_InputField doctorInputField;

    [Header("Display Texts (The ones that auto-stretch)")]
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private TextMeshProUGUI medicalDisplay;
    [SerializeField] private TextMeshProUGUI doctorDisplay;

    [Header("Buttons")]
    [SerializeField] private Button saveButton;

    private const string KEY_NAME    = "user_name";
    private const string KEY_MEDICAL = "user_medical";
    private const string KEY_DOCTOR  = "user_doctor";

    void Start()
    {
        ValidateReferences();
        LoadData();
        
        saveButton.onClick.AddListener(SaveData);

        // We now pass the specific label into the SyncDisplay function
        nameInputField.onValueChanged.AddListener(val    => SyncDisplay("Your Name", val, nameDisplay));
        medicalInputField.onValueChanged.AddListener(val => SyncDisplay("Medical Conditions", val, medicalDisplay));
        doctorInputField.onValueChanged.AddListener(val  => SyncDisplay("Primary Care Doctor", val, doctorDisplay));
    }

    // UPDATED: Now takes a 'label' parameter and applies the color tag
    private void SyncDisplay(string label, string value, TextMeshProUGUI display)
    {
        if (display == null) return;

        // This combines your colored label with the user's input value
        display.text = $"<color=#003366>{label}: </color>{value}";
        
        // This keeps your auto-stretch working
        LayoutRebuilder.ForceRebuildLayoutImmediate(display.rectTransform);
    }

    public void SaveData()
    {
        PlayerPrefs.SetString(KEY_NAME,    nameInputField.text);
        PlayerPrefs.SetString(KEY_MEDICAL, medicalInputField.text);
        PlayerPrefs.SetString(KEY_DOCTOR,  doctorInputField.text);
        PlayerPrefs.Save();
        Debug.Log("Profile saved!");
    }

    private void LoadData()
    {
        nameInputField.text    = PlayerPrefs.GetString(KEY_NAME, "");
        medicalInputField.text = PlayerPrefs.GetString(KEY_MEDICAL, "");
        doctorInputField.text  = PlayerPrefs.GetString(KEY_DOCTOR, "");

        // Sync all displays on load with their labels
        SyncDisplay("Your Name", nameInputField.text, nameDisplay);
        SyncDisplay("Medical Conditions", medicalInputField.text, medicalDisplay);
        SyncDisplay("Primary Care Doctor", doctorInputField.text, doctorDisplay);
    }

    private void ValidateReferences()
    {
        if (nameInputField == null || nameDisplay == null || saveButton == null) 
            Debug.LogError("Missing references in the Inspector!");
    }
}