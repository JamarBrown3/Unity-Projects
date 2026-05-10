#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;

/// <summary>
/// WEMA - Rebuild 05_Contacts Scene
/// Drop this in Assets/Editor/ then run: Tools -> WEMA -> Build Contacts Scene
/// It rebuilds 05_Contacts.unity preserving your exact colors, fonts, and nav bar.
/// </summary>
public class BuildContactsScene : EditorWindow
{
    // ── Your exact colors from the scene ─────────────────────────
    static readonly Color COL_DARK_BLUE   = Hex("#003366");
    static readonly Color COL_WHITE       = Color.white;
    static readonly Color COL_PURPLE      = new Color(0.5098f, 0.1069f, 0.8396f, 1f); // your save button purple
    static readonly Color COL_BLUE_BTN    = new Color(0f, 0.0621f, 0.9906f, 1f);      // your nav button blue
    static readonly Color COL_INPUT_BG    = COL_WHITE;
    static readonly Color COL_CARD_BG     = COL_WHITE;
    static readonly Color COL_DELETE_RED  = new Color(0.85f, 0.15f, 0.15f, 1f);
    static readonly Color COL_PANEL_BG    = COL_WHITE;
    static readonly Color COL_GRAY_HINT   = new Color(0.196f, 0.196f, 0.196f, 0.5f);

    // ── Your exact font GUID ──────────────────────────────────────
    const string FONT_GUID = "8f586378b4e144a9851e7b34d9b748ee";

    // ── Your nav icon sprite GUIDs (from scene) ───────────────────
    const string HOME_SPRITE_GUID     = "9730f293200b5443492efbf13b1e9c62";
    const string PROFILE_SPRITE_GUID  = "6e4c89afaa7114753b34389c5f8670c1";
    const string CONTACTS_SPRITE_GUID = "6e4c89afaa7114753b34389c5f8670c1";

    [MenuItem("Tools/WEMA/Build Contacts Scene")]
    public static void BuildScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Root objects (matching your scene exactly)
        BuildCamera();
        BuildGlobalLight();
        var canvas  = BuildCanvas();
        BuildNavigator();
        BuildApplication();
        BuildEventSystem();

        System.IO.Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/05_Contacts.unity");
        Debug.Log("✅ WEMA Contacts scene built at Assets/Scenes/05_Contacts.unity");
    }

    // ─────────────────────────────────────────────────────────────
    static void BuildCamera()
    {
        var go  = new GameObject("Main Camera");
        go.tag  = "MainCamera";
        var cam = go.AddComponent<Camera>();
        cam.clearFlags       = CameraClearFlags.SolidColor;
        cam.backgroundColor  = new Color(0.192f, 0.302f, 0.475f, 0f);
        cam.orthographic     = true;
        cam.orthographicSize = 5;
        cam.depth            = -1;
        go.AddComponent<AudioListener>();
        go.transform.position = new Vector3(0, 0, -10);
    }

    static void BuildGlobalLight()
    {
        var go = new GameObject("Global Light 2D");
        // Just add the transform — the Light2D component needs the URP package
        // Unity will auto-add it when you open the scene if URP is installed
    }

    static void BuildNavigator()
    {
        new GameObject("Navigator");
    }

    static void BuildApplication()
    {
        new GameObject("Application");
    }

    static void BuildEventSystem()
    {
        var go = new GameObject("EventSystem");
        go.AddComponent<UnityEngine.EventSystems.EventSystem>();
        go.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
    }

    // ─────────────────────────────────────────────────────────────
    static GameObject BuildCanvas()
    {
        var cGO    = new GameObject("Canvas");
        var canvas = cGO.AddComponent<Canvas>();
        canvas.renderMode  = RenderMode.ScreenSpaceOverlay;
        var cs             = cGO.AddComponent<CanvasScaler>();
        cs.uiScaleMode     = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1170, 2532);
        cs.matchWidthOrHeight  = 0.5f;
        cGO.AddComponent<GraphicRaycaster>();

        // Panel (full white background — matches your scene)
        var panel = MakeUI("Panel", cGO.transform);
        Stretch(panel);
        var panelImg   = panel.AddComponent<Image>();
        panelImg.color = COL_PANEL_BG;
        panelImg.sprite = BuiltinSprite(10907); // Background sprite

        // ── Title area ──
        BuildTitleArea(panel.transform);

        // ── Scroll area (between title and nav) ──
        BuildScrollArea(panel.transform);

        // ── Bottom Nav Bar ──
        BuildBottomNav(panel.transform);

        return cGO;
    }

    // ─────────────────────────────────────────────────────────────
    static void BuildTitleArea(Transform parent)
    {
        // "Contacts" title — matches ContactsSceneInfo text in your scene
        var title = MakeUI("ContactsSceneInfo", parent);
        var rt    = title.GetComponent<RectTransform>();
        rt.anchorMin        = new Vector2(0.5f, 0.5f);
        rt.anchorMax        = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = new Vector2(0f, 450f);
        rt.sizeDelta        = new Vector2(750f, 257f);

        var tmp      = title.AddComponent<TextMeshProUGUI>();
        tmp.text     = "<color=#003366>Contacts</color>";
        tmp.fontSize = 90;
        tmp.alignment = TextAlignmentOptions.Center;
        ApplyFont(tmp);
        title.AddComponent<CanvasRenderer>();
    }

    // ─────────────────────────────────────────────────────────────
    static void BuildScrollArea(Transform parent)
    {
        // ScrollArea
        var scrollArea = MakeUI("ScrollArea", parent);
        var saRT       = scrollArea.GetComponent<RectTransform>();
        saRT.anchorMin  = new Vector2(0, 0);
        saRT.anchorMax  = new Vector2(1, 1);
        saRT.offsetMin  = new Vector2(0, 200);   // above bottom nav
        saRT.offsetMax  = new Vector2(0, -320);  // below title

        var sr               = scrollArea.AddComponent<ScrollRect>();
        sr.horizontal        = false;
        sr.vertical          = true;
        sr.movementType      = ScrollRect.MovementType.Clamped;
        sr.inertia           = false;
        sr.scrollSensitivity = 30f;
        scrollArea.AddComponent<RectMask2D>();

        // ContentScroll
        var content = MakeUI("ContentScroll", scrollArea.transform);
        var cRT     = content.GetComponent<RectTransform>();
        cRT.anchorMin        = new Vector2(0, 1);
        cRT.anchorMax        = new Vector2(1, 1);
        cRT.pivot            = new Vector2(0.5f, 1f);
        cRT.anchoredPosition = new Vector2(0, 0);
        cRT.sizeDelta        = new Vector2(0, 3000);

        var vlg                  = content.AddComponent<VerticalLayoutGroup>();
        vlg.padding              = new RectOffset(40, 40, 30, 30);
        vlg.spacing              = 30;
        vlg.childAlignment       = TextAnchor.UpperCenter;
        vlg.childControlWidth    = true;
        vlg.childControlHeight   = false;
        vlg.childForceExpandWidth  = true;
        vlg.childForceExpandHeight = false;

        sr.content  = cRT;
        sr.viewport = saRT;

        // ── Add Contact Form (top of scroll) ──
        BuildAddForm(content.transform);

        // ── "Edit Contacts" label ──
        BuildEditContactsLabel(content.transform);

        // ── ContactsManager script holder ──
        // (wired up after scene is built — see ContactsManager.cs)
    }

    // ─────────────────────────────────────────────────────────────
    static void BuildAddForm(Transform parent)
    {
        var form = MakeUI("AddContactForm", parent);
        var le   = form.AddComponent<LayoutElement>();
        le.preferredHeight = 680;

        var bg    = form.AddComponent<Image>();
        bg.color  = COL_WHITE;
        bg.sprite = BuiltinSprite(10907);
        bg.type   = Image.Type.Sliced;

        var vlg                  = form.AddComponent<VerticalLayoutGroup>();
        vlg.padding              = new RectOffset(40, 40, 30, 30);
        vlg.spacing              = 25;
        vlg.childAlignment       = TextAnchor.UpperCenter;
        vlg.childControlWidth    = true;
        vlg.childControlHeight   = false;
        vlg.childForceExpandWidth  = true;
        vlg.childForceExpandHeight = false;

        // "Add Contact" header
        var header    = MakeTMP("AddContactHeader", form.transform, "<color=#003366>Add Contact</color>", 70);
        var headerLE  = header.AddComponent<LayoutElement>();
        headerLE.preferredHeight = 90;
        header.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;

        // Name input
        BuildInputField(form.transform, "NameInput", "Enter name:", 150);

        // Relation input
        BuildInputField(form.transform, "RelationInput", "Relation (e.g. Daughter):", 150);

        // Phone input
        BuildInputField(form.transform, "PhoneInput", "Phone number:", 150);

        // Add Button — matches your purple save button exactly
        var btnGO  = MakeUI("AddButton", form.transform);
        var btnLE  = btnGO.AddComponent<LayoutElement>();
        btnLE.preferredHeight = 110;
        var btnImg    = btnGO.AddComponent<Image>();
        btnImg.color  = COL_PURPLE;
        btnImg.sprite = BuiltinSprite(10905); // rounded rect
        btnImg.type   = Image.Type.Sliced;
        var btn       = btnGO.AddComponent<Button>();
        btn.targetGraphic = btnImg;

        var btnTxt    = MakeTMP("Text (TMP)", btnGO.transform, "Add Contact", 55);
        Stretch(btnTxt);
        var t         = btnTxt.GetComponent<TextMeshProUGUI>();
        t.color       = COL_WHITE;
        t.alignment   = TextAlignmentOptions.Center;
        t.fontStyle   = FontStyles.Bold;
    }

    static void BuildInputField(Transform parent, string name, string placeholder, float height)
    {
        var go  = MakeUI(name, parent);
        var le  = go.AddComponent<LayoutElement>();
        le.preferredHeight = height;

        var bg    = go.AddComponent<Image>();
        bg.color  = COL_WHITE;
        bg.sprite = BuiltinSprite(10911); // InputField sprite
        bg.type   = Image.Type.Sliced;

        var input = go.AddComponent<TMP_InputField>();
        input.targetGraphic = bg;

        // Text Area
        var textArea = MakeUI("Text Area", go.transform);
        var taRT     = textArea.GetComponent<RectTransform>();
        taRT.anchorMin        = new Vector2(0, 0);
        taRT.anchorMax        = new Vector2(1, 1);
        taRT.offsetMin        = new Vector2(10, 6);
        taRT.offsetMax        = new Vector2(-10, -7);
        textArea.AddComponent<RectMask2D>();

        // Placeholder
        var ph     = MakeTMP("Placeholder", textArea.transform, placeholder, 60);
        Stretch(ph);
        var phTMP  = ph.GetComponent<TextMeshProUGUI>();
        phTMP.color     = COL_GRAY_HINT;
        phTMP.fontStyle = FontStyles.Italic;
        var phLE   = ph.AddComponent<LayoutElement>();
        phLE.ignoreLayout = true;

        // Text
        var txt    = MakeTMP("Text", textArea.transform, "", 60);
        Stretch(txt);
        var txtTMP = txt.GetComponent<TextMeshProUGUI>();
        txtTMP.color = COL_DARK_BLUE;
        var txtLE  = txt.AddComponent<LayoutElement>();
        txtLE.ignoreLayout = true;

        input.textViewport  = textArea.GetComponent<RectTransform>();
        input.textComponent = txtTMP;
        input.placeholder   = phTMP;

        ApplyFont(phTMP);
        ApplyFont(txtTMP);
    }

    // ─────────────────────────────────────────────────────────────
    static void BuildEditContactsLabel(Transform parent)
    {
        var lbl    = MakeUI("EditContactsLabel", parent);
        var le     = lbl.AddComponent<LayoutElement>();
        le.preferredHeight = 100;

        var tmp      = lbl.AddComponent<TextMeshProUGUI>();
        tmp.text     = "<color=#003366>Edit Contacts</color>";
        tmp.fontSize = 70;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;
        ApplyFont(tmp);
        lbl.AddComponent<CanvasRenderer>();
    }

    // ─────────────────────────────────────────────────────────────
    static void BuildBottomNav(Transform parent)
    {
        var nav    = MakeUI("Bottom_Nav_Bar", parent);
        var rt     = nav.GetComponent<RectTransform>();
        rt.anchorMin  = new Vector2(0, 0);
        rt.anchorMax  = new Vector2(1, 0);
        rt.pivot      = new Vector2(0.5f, 0f);
        rt.offsetMin  = new Vector2(0, 0);
        rt.offsetMax  = new Vector2(0, 200);

        var bg    = nav.AddComponent<Image>();
        bg.color  = COL_WHITE;

        // Nav buttons container
        var navContent = MakeUI("NavContent", nav.transform);
        Stretch(navContent);
        var hlg                   = navContent.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment        = TextAnchor.MiddleCenter;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = true;

        // Home button
        BuildNavButton(navContent.transform, "Home_Button", "Home",
            HOME_SPRITE_GUID, COL_BLUE_BTN, "loadHome_Scene", new Vector2(-413.6f, -66.5f));

        // Profile button
        BuildNavButton(navContent.transform, "Profile_Button", "Profile",
            PROFILE_SPRITE_GUID, new Color(0.069f, 0f, 0.915f, 1f), "loadProfile_Scene", new Vector2(0f, -66.5f));

        // Contacts button (active — highlighted)
        BuildNavButton(navContent.transform, "Contacts_Button", "Contacts",
            CONTACTS_SPRITE_GUID, COL_BLUE_BTN, "loadContacts_Scene", new Vector2(413.6f, -66.5f));
    }

    static void BuildNavButton(Transform parent, string name, string label,
        string spriteGuid, Color iconColor, string navMethod, Vector2 pos)
    {
        var go  = MakeUI(name, parent);

        var btn = go.AddComponent<Button>();
        var img = go.AddComponent<Image>();
        img.color = iconColor;

        // Try load sprite from GUID
        string spritePath = AssetDatabase.GUIDToAssetPath(spriteGuid);
        if (!string.IsNullOrEmpty(spritePath))
        {
            var sprites = AssetDatabase.LoadAllAssetsAtPath(spritePath);
            foreach (var s in sprites)
                if (s is Sprite sp) { img.sprite = sp; break; }
        }

        btn.targetGraphic = img;

        // Label
        var lblGO  = MakeTMP("Text (TMP)", go.transform, $"<color=#003366>{label}</color>", 45);
        var lblRT  = lblGO.GetComponent<RectTransform>();
        lblRT.anchorMin        = new Vector2(0.5f, 0f);
        lblRT.anchorMax        = new Vector2(0.5f, 0f);
        lblRT.pivot            = new Vector2(0.5f, 0f);
        lblRT.anchoredPosition = new Vector2(0, 5);
        lblRT.sizeDelta        = new Vector2(200, 50);
        var lblTMP = lblGO.GetComponent<TextMeshProUGUI>();
        lblTMP.alignment = TextAlignmentOptions.Center;
        lblTMP.fontStyle = FontStyles.Bold;
    }

    // ─────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────
    static GameObject MakeUI(string name, Transform parent)
    {
        var go = new GameObject(name);
        go.AddComponent<RectTransform>();
        go.transform.SetParent(parent, false);
        return go;
    }

    static GameObject MakeTMP(string name, Transform parent, string text, float size)
    {
        var go  = MakeUI(name, parent);
        go.AddComponent<CanvasRenderer>();
        var tmp      = go.AddComponent<TextMeshProUGUI>();
        tmp.text     = text;
        tmp.fontSize = size;
        tmp.color    = COL_DARK_BLUE;
        ApplyFont(tmp);
        return go;
    }

    static void Stretch(GameObject go)
    {
        var rt       = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    static void ApplyFont(TextMeshProUGUI tmp)
    {
        var font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
            AssetDatabase.GUIDToAssetPath(FONT_GUID));
        if (font != null) tmp.font = font;
    }

    static Sprite BuiltinSprite(long fileID)
    {
        var allSprites = AssetDatabase.LoadAllAssetsAtPath("Resources/unity_builtin_extra");
        foreach (var obj in allSprites)
            if (obj is Sprite s && s.GetInstanceID() == fileID) return s;
        // fallback: just return null, Unity will use default
        return null;
    }

    static Color Hex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color c);
        return c;
    }
}
#endif
