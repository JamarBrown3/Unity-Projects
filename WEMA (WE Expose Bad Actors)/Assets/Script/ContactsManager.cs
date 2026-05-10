using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContactsManager : MonoBehaviour
{
    [Header("Add Form Fields")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField relationInput;
    [SerializeField] private TMP_InputField phoneInput;
    [SerializeField] private Button addButton;

    [Header("Scroll Content")]
    [SerializeField] private RectTransform contentScroll;

    [Header("Optional")]
    [SerializeField] private TextMeshProUGUI errorText;

    static readonly Color COL_DARK_BLUE =
        new Color(0f, 0.2f, 0.4f, 1f);

    static readonly Color COL_WHITE =
        Color.white;

    static readonly Color COL_RED =
        new Color(0.85f, 0.15f, 0.15f, 1f);

    static readonly Color COL_TOGGLE_ON =
        new Color(0.298f, 0.686f, 0.314f, 1f);

    static readonly Color COL_TOGGLE_OFF =
        new Color(0.8f, 0.8f, 0.8f, 1f);

    [System.Serializable]
    private class ContactEntry
    {
        public string name;
        public string relation;
        public string phone;
        public bool notifyOn;
    }

    [System.Serializable]
    private class SaveWrapper
    {
        public List<ContactEntry> list;
    }

    private List<ContactEntry> contacts =
        new List<ContactEntry>();

    private const string SAVE_KEY =
        "wema_contacts";

    void Start()
    {
        if (addButton)
            addButton.onClick.AddListener(OnAdd);

        if (errorText)
            errorText.text = "";

        LoadAndRebuild();
    }

    void OnAdd()
    {
        string n =
            nameInput ?
            nameInput.text.Trim() : "";

        string r =
            relationInput ?
            relationInput.text.Trim() : "";

        string p =
            phoneInput ?
            phoneInput.text.Trim() : "";

        if (string.IsNullOrEmpty(n))
        {
            SetError("Please enter a name.");
            return;
        }

        contacts.Add(new ContactEntry
        {
            name = n,
            relation = r,
            phone = p,
            notifyOn = false
        });

        Save();

        SpawnCard(contacts.Count - 1);

        if (nameInput) nameInput.text = "";
        if (relationInput) relationInput.text = "";
        if (phoneInput) phoneInput.text = "";

        SetError("");
    }

    void SpawnCard(int index)
    {
        if (!contentScroll)
            return;

        ContactEntry entry =
            contacts[index];

        // ───────────────── CARD ROOT ─────────────────
        GameObject card =
            new GameObject("ContactCard_" + index);

        card.transform.SetParent(
            contentScroll,
            false);

        card.transform.localScale =
            Vector3.one;

        RectTransform cardRT =
            card.AddComponent<RectTransform>();

        cardRT.anchorMin =
            new Vector2(0, 1);

        cardRT.anchorMax =
            new Vector2(1, 1);

        cardRT.pivot =
            new Vector2(0.5f, 1);

        LayoutElement layout =
            card.AddComponent<LayoutElement>();

        layout.minHeight = 260;

        ContentSizeFitter fitter =
            card.AddComponent<ContentSizeFitter>();

        fitter.verticalFit =
            ContentSizeFitter.FitMode.PreferredSize;

        Image cardImg =
            card.AddComponent<Image>();

        cardImg.color = COL_WHITE;

        cardImg.sprite =
            Resources.GetBuiltinResource<Sprite>(
                "UI/Skin/UISprite.psd");

        cardImg.type =
            Image.Type.Sliced;

        HorizontalLayoutGroup hlg =
            card.AddComponent<HorizontalLayoutGroup>();

        hlg.padding =
            new RectOffset(40, 40, 35, 35);

        hlg.spacing = 30;

        hlg.childAlignment =
            TextAnchor.MiddleLeft;

        hlg.childControlWidth = true;
        hlg.childControlHeight = true;

        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = false;

        // ───────────────── TEXT STACK ─────────────────
        GameObject textStack =
            new GameObject("TextStack");

        textStack.transform.SetParent(
            card.transform,
            false);

        textStack.transform.localScale =
            Vector3.one;

        LayoutElement textLE =
            textStack.AddComponent<LayoutElement>();

        textLE.flexibleWidth = 1;
        textLE.minWidth = 500;

        VerticalLayoutGroup textVLG =
            textStack.AddComponent<VerticalLayoutGroup>();

        textVLG.spacing = 12;

        textVLG.childAlignment =
            TextAnchor.UpperLeft;

        textVLG.childControlWidth = true;
        textVLG.childControlHeight = true;

        textVLG.childForceExpandHeight = false;

        AddTMP(
            textStack.transform,
            "NameText",
            $"<b>{entry.name}</b>",
            52
        );

        if (!string.IsNullOrEmpty(entry.relation))
        {
            AddTMP(
                textStack.transform,
                "RelationText",
                entry.relation,
                40
            );
        }

        if (!string.IsNullOrEmpty(entry.phone))
        {
            AddTMP(
                textStack.transform,
                "PhoneText",
                entry.phone,
                40
            );
        }

        // ───────────────── NOTIFY STACK ─────────────────
        GameObject notifyStack =
            new GameObject("NotifyStack");

        notifyStack.transform.SetParent(
            card.transform,
            false);

        notifyStack.transform.localScale =
            Vector3.one;

        LayoutElement notifyLE =
            notifyStack.AddComponent<LayoutElement>();

        notifyLE.minWidth = 180;
        notifyLE.preferredWidth = 180;
        notifyLE.flexibleWidth = 0;

        VerticalLayoutGroup notifyVLG =
            notifyStack.AddComponent<VerticalLayoutGroup>();

        notifyVLG.spacing = 15;

        notifyVLG.childAlignment =
            TextAnchor.UpperCenter;

        notifyVLG.childControlWidth = true;
        notifyVLG.childControlHeight = true;

        notifyVLG.childForceExpandHeight = false;
        notifyVLG.childForceExpandWidth = false;

        AddTMP(
            notifyStack.transform,
            "NotifyLabel",
            "<b>NOTIFY</b>",
            28
        );

        // ───────────────── TOGGLE CONTAINER ─────────────────
        GameObject toggleContainer =
            new GameObject("ToggleContainer");

        toggleContainer.transform.SetParent(
            notifyStack.transform,
            false);

        toggleContainer.transform.localScale =
            Vector3.one;

        HorizontalLayoutGroup toggleContainerLayout =
            toggleContainer.AddComponent<HorizontalLayoutGroup>();

        toggleContainerLayout.childAlignment =
            TextAnchor.MiddleCenter;

        toggleContainerLayout.childControlWidth = false;
        toggleContainerLayout.childControlHeight = false;

        LayoutElement containerLE =
            toggleContainer.AddComponent<LayoutElement>();

        containerLE.preferredWidth = 160;
        containerLE.preferredHeight = 120;

        // ───────────────── TOGGLE ─────────────────
        GameObject toggleGO =
            new GameObject("ToggleSwitch");

        toggleGO.transform.SetParent(
            toggleContainer.transform,
            false);

        toggleGO.transform.localScale =
            Vector3.one;

        LayoutElement toggleLE =
            toggleGO.AddComponent<LayoutElement>();

        toggleLE.preferredWidth = 130;
        toggleLE.preferredHeight = 70;

        Image toggleImg =
            toggleGO.AddComponent<Image>();

        toggleImg.color =
            entry.notifyOn ?
            COL_TOGGLE_ON :
            COL_TOGGLE_OFF;

        toggleImg.sprite =
            Resources.GetBuiltinResource<Sprite>(
                "UI/Skin/UISprite.psd");

        toggleImg.type =
            Image.Type.Sliced;

        // ───────────────── KNOB ─────────────────
        GameObject knob =
            new GameObject("Knob");

        knob.transform.SetParent(
            toggleGO.transform,
            false);

        knob.transform.localScale =
            Vector3.one;

        RectTransform knobRT =
            knob.AddComponent<RectTransform>();

        knobRT.sizeDelta =
            new Vector2(55, 55);

        knobRT.anchorMin =
            new Vector2(
                entry.notifyOn ? 1 : 0,
                0.5f);

        knobRT.anchorMax =
            new Vector2(
                entry.notifyOn ? 1 : 0,
                0.5f);

        knobRT.anchoredPosition =
            new Vector2(
                entry.notifyOn ? -30 : 30,
                0);

        Image knobImg =
            knob.AddComponent<Image>();

        knobImg.sprite =
            Resources.GetBuiltinResource<Sprite>(
                "UI/Skin/Knob.psd");

        Toggle toggle =
            toggleGO.AddComponent<Toggle>();

        toggle.isOn = entry.notifyOn;
        toggle.targetGraphic = toggleImg;
        toggle.graphic = knobImg;

        int capturedIdx = index;

        toggle.onValueChanged.AddListener(
            (isOn) =>
            {
                if (capturedIdx < contacts.Count)
                {
                    contacts[capturedIdx]
                        .notifyOn = isOn;

                    Save();
                }

                toggleImg.color =
                    isOn ?
                    COL_TOGGLE_ON :
                    COL_TOGGLE_OFF;

                knobRT.anchorMin =
                    new Vector2(
                        isOn ? 1 : 0,
                        0.5f);

                knobRT.anchorMax =
                    new Vector2(
                        isOn ? 1 : 0,
                        0.5f);

                knobRT.anchoredPosition =
                    new Vector2(
                        isOn ? -30 : 30,
                        0);
            });

        // ───────────────── DELETE BUTTON ─────────────────
        GameObject del =
            new GameObject("DeleteButton");

        del.transform.SetParent(
            card.transform,
            false);

        del.transform.localScale =
            Vector3.one;

        LayoutElement delLE =
            del.AddComponent<LayoutElement>();

        delLE.preferredWidth = 90;
        delLE.preferredHeight = 90;

        // THIS MAKES THE BUTTON SIT LOWER
        delLE.minHeight = 160;

        Image delImg =
            del.AddComponent<Image>();

        delImg.color = COL_RED;

        delImg.sprite =
            Resources.GetBuiltinResource<Sprite>(
                "UI/Skin/UISprite.psd");

        delImg.type =
            Image.Type.Sliced;

        Button delBtn =
            del.AddComponent<Button>();

        delBtn.targetGraphic = delImg;

        GameObject icon =
            new GameObject("Icon");

        icon.transform.SetParent(
            del.transform,
            false);

        icon.transform.localScale =
            Vector3.one;

        TextMeshProUGUI iconTMP =
            icon.AddComponent<TextMeshProUGUI>();

        iconTMP.text = "🗑";

        iconTMP.fontSize = 44;

        iconTMP.alignment =
            TextAlignmentOptions.Center;

        ApplyFont(iconTMP);

        int delIdx = index;

        delBtn.onClick.AddListener(() =>
        {
            DeleteContact(card, delIdx);
        });
    }

    void AddTMP(
        Transform parent,
        string objName,
        string text,
        float size)
    {
        GameObject go =
            new GameObject(objName);

        go.transform.SetParent(parent, false);

        go.transform.localScale =
            Vector3.one;

        LayoutElement layout =
            go.AddComponent<LayoutElement>();

        layout.minHeight = 60;

        TextMeshProUGUI tmp =
            go.AddComponent<TextMeshProUGUI>();

        tmp.text = text;

        tmp.fontSize = size;

        tmp.color = COL_DARK_BLUE;

        tmp.enableWordWrapping = true;

        tmp.overflowMode =
            TextOverflowModes.Overflow;

        tmp.lineSpacing = 8;

        ApplyFont(tmp);
    }

    void DeleteContact(
        GameObject card,
        int index)
    {
        if (index < 0 ||
            index >= contacts.Count)
            return;

        contacts.RemoveAt(index);

        Save();

        Destroy(card);

        LoadAndRebuild();
    }

    void Save()
    {
        SaveWrapper wrapper =
            new SaveWrapper();

        wrapper.list = contacts;

        PlayerPrefs.SetString(
            SAVE_KEY,
            JsonUtility.ToJson(wrapper));

        PlayerPrefs.Save();
    }

    void LoadAndRebuild()
    {
        string json =
            PlayerPrefs.GetString(
                SAVE_KEY,
                "");

        contacts =
            new List<ContactEntry>();

        if (!string.IsNullOrEmpty(json))
        {
            try
            {
                SaveWrapper wrapper =
                    JsonUtility.FromJson<SaveWrapper>(
                        json);

                if (wrapper != null &&
                    wrapper.list != null)
                {
                    contacts = wrapper.list;
                }
            }
            catch
            {
            }
        }

        if (contentScroll)
        {
            List<GameObject> kill =
                new List<GameObject>();

            foreach (Transform child in contentScroll)
            {
                if (child.name.StartsWith("ContactCard_"))
                {
                    kill.Add(child.gameObject);
                }
            }

            foreach (GameObject g in kill)
            {
                Destroy(g);
            }
        }

        for (int i = 0; i < contacts.Count; i++)
        {
            SpawnCard(i);
        }
    }

    void SetError(string msg)
    {
        if (errorText)
            errorText.text = msg;
    }

    void ApplyFont(TextMeshProUGUI tmp)
    {
#if UNITY_EDITOR
        const string GUID =
            "8f586378b4e144a9851e7b34d9b748ee";

        TMP_FontAsset font =
            UnityEditor.AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
                UnityEditor.AssetDatabase.GUIDToAssetPath(GUID));

        if (font)
        {
            tmp.font = font;
        }
#endif
    }
}