using UnityEngine;
using System.IO;
using TMPro;

public class Json : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_InputField courseInput;
    public TMP_InputField instructorInput;
    public TMP_InputField textbookInput;

    [Header("Display Text Elements")]
    public TMP_Text courseTextElement;
    public TMP_Text instructorTextElement;
    public TMP_Text textbookTextElement;

    private string savePath;

    void Start()
    {
        // Changed ApplicationException to Application
        // Using dataPath puts the file in your Project/Assets folder
        savePath = Path.Combine(Application.dataPath, "courseData.json");
        
        Debug.Log("The file will be at: " + savePath);
    }

    public void SaveData()
    {
        Course data = new Course();
        data.courseName = courseInput.text;
        data.instructor = instructorInput.text;
        data.textbook = textbookInput.text;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        // This refreshes the Project window so the file appears immediately
        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        #endif

        Debug.Log("Saved JSON: " + json);
    }

    public void LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            Course data = JsonUtility.FromJson<Course>(json);

            // Setting your TMP_Text elements
            courseTextElement.text = data.courseName;
            instructorTextElement.text = data.instructor;
            textbookTextElement.text = data.textbook;

            Debug.Log("Loaded: " + data.courseName);
        }
        else
        {
            Debug.LogError("No file found! Hit Save first.");
        }
    }
}