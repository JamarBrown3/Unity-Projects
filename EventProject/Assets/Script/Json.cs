using UnityEngine;
using System.IO;
using TMPro;
using System;
using UnityEngine.UI; // Added for parsing

public class Json : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_InputField eventNameInput;
    public TMP_InputField timeInput; // Expecting: YYYY/MM/DD HH:MM
    public TMP_InputField eventTypeInput;
    public TMP_InputField descriptionInput;
    // Testing out boilerplate
    public TMP_Dropdown solvedDropdown;

    [Header("Display Text Elements")]
    public TMP_Text eventTextElement;
    public TMP_Text timeTextElement;
    public TMP_Text eventTypeTextElement;
    public TMP_Text solvedTextElement;

    public TMP_Text eventDescriptionTextElement;


    private string savePath;

    void Start()
    {
        savePath = Path.Combine(Application.dataPath, "eventData.json");

        solvedDropdown.options.Clear();
        solvedDropdown.options.Add(new TMP_Dropdown.OptionData("Solved"));
        solvedDropdown.options.Add(new TMP_Dropdown.OptionData("Not Solved"));
    }

    public void SaveData()
    {
        Events data = new Events();
        data.name = eventNameInput.text;
        data.eventType = eventTypeInput.text;
        data.eventDescription = descriptionInput.text;
        data.solved = solvedDropdown.value == 0;

        // --- TIME PARSING LOGIC ---
        // I used Ai boilerplate code to help me with the parsing of the time input
        // Simple split: assuming format "2026/04/19 10:30"
        try
        {
            string[] parts = timeInput.text.Split(new char[] { '/', ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
            data.time = new Time(
                int.Parse(parts[0]), // Year
                int.Parse(parts[1]), // Month
                int.Parse(parts[2]), // Day
                int.Parse(parts[3]), // Hour
                int.Parse(parts[4])  // Minute
            );
        }
        catch
        {
            Debug.LogError("Time format wrong! Use: YYYY/MM/DD HH:MM");
            data.time = new Time(2026, 1, 1, 0, 0); // Default fallback
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
        Debug.Log("Saved: " + json);
    }

    public void LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            Events data = JsonUtility.FromJson<Events>(json);

            // Update UI with loaded data
            eventTextElement.text = "Event: " + data.name;
            timeTextElement.text = "Time: " + data.time.ToString();
            eventTypeTextElement.text = "Type: " + data.eventType;
            eventDescriptionTextElement.text = "Description: " + data.eventDescription;
            solvedTextElement.text = "Status: " + (data.solved ? "Solved" : "Not Solved");

            // Sync Inputs back
            eventNameInput.text = data.name;
            timeInput.text = data.time.ToString();
            solvedDropdown.value = data.solved ? 0 : 1;
            eventTypeInput.text = data.eventType;
            descriptionInput.text = data.eventDescription;

            Debug.Log("Loaded: " + data.name);
        }
    }
}