#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// WEMA - Build Family Alerts Scene
/// 
/// HOW TO USE:
/// 1. Drop this file into any folder named "Editor" inside your Assets folder
///    (e.g. Assets/Editor/BuildFamilyAlertsScene.cs)
/// 2. In Unity menu bar click: Tools -> WEMA -> Build Family Alerts Scene
/// 3. The scene will be created at Assets/Scenes/06_FamilyAlerts.unity
/// 4. Add it to your Build Settings (File -> Build Settings -> Add Open Scenes)
/// </summary>
public class BuildFamilyAlertsScene : EditorWindow
{
    // ── WEMA Colors ──────────────────────────────────────────────
    static readonly Color COL_DARK_BLUE   = HexColor("#003366");
    static readonly Color COL_BG          = HexColor("#F0F4F8");
    static readonly Color COL_WHITE       = Color.white;
    static readonly Color COL_AVATAR_BG   = HexColor("#D6E8FF");
    static readonly Color COL_GRAY_TEXT   = HexColor("#888888");
    static readonly Color COL_TOGGLE_OFF  = HexColor("#CCCCCC");
    static readonly Color COL_TOGGLE_ON   = HexColor("#4CAF50");
    static readonly Color COL_NAV_BG      = HexColor("#FFFFFF");
    static readonly Color COL_PANEL_BG    = HexColor("#F0F4F8");

    // Font GUID from your existing scenes
    const string FONT_GUID = "8f586378b4e144a9851e7b34d9b748ee";

    [MenuItem("Tools/WEMA/Build Family Alerts Scene")]
    public static void BuildScene()
    {
        // Create or open the scene
        string scenePath = "Assets/Scenes/06_FamilyAlerts.unity";
        var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // ── Root objects ─────────────────────────────────────────
        BuildCamera();
        BuildCanvas();
        BuildEventSystem();
        BuildNavigator();
        BuildApplication();

        // Save
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(newScene, scenePath);
        Debug.Log("✅ WEMA Family Alerts scene built at: " + scenePath);
    }

    // ─────────────────────────────────────────────────────────────
    static void BuildCamera()
    {
        var cam = new GameObject("Main Camera");
        cam.tag = "MainCamera";
        var c = cam.AddComponent<Camera>();
        c.clearFlags = CameraClearFlags.SolidColor;
        c.backgroundColor = COL_BG;
        c.orthographic = true;
        c.orthographicSize = 5;
        cam.AddComponent<AudioListener>();
    }

    static void BuildEventSystem()
    {
        var es = new GameObject("EventSystem");
        es.AddComponent<UnityEngine.EventSystems.EventSystem>();
        es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
    }

    static void BuildNavigator()
    {
        new GameObject("Navigator");
    }

    static void BuildApplication()
    {
        new GameObject("Application");
    }

    // ─────────────────────────────────────────────────────────────
    static void BuildCanvas()
    {
        // Canvas
        var canvasGO = new GameObject("Canvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 0;
        var cs = canvasGO.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1170, 2532); // iPhone-style
        cs.matchWidthOrHeight = 0.5f;
        canvasGO.AddComponent<GraphicRaycaster>();

        // Panel (full background)
        var panel = CreateUIObject("Panel", canvasGO.transform);
        SetStretchFull(panel);
        var panelImg = panel.AddComponent<Image>();
        panelImg.color = COL_PANEL_BG;

        // Header
        BuildHeader(panel.transform);

        // Title
        BuildTitle(panel.transform);

        // ScrollArea with cards
        BuildScrollArea(panel.transform);

        // Bottom Nav Bar
        BuildBottomNav(panel.transform);
    }

    // ─────────────────────────────────────────────────────────────
    static void BuildHeader(Transform parent)
    {
        var header = CreateUIObject("Header", parent);
        var rt = header.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 1f);
        rt.offsetMin = new Vector2(0, -160);
        rt.offsetMax = new Vector2(0, 0);

        var bg = header.AddComponent<Image>();
        bg.color = COL_WHITE;

        // Back button
        var backBtn = CreateUIObject("BackButton", header.transform);
        var backRT = backBtn.GetComponent<RectTransform>();
        backRT.anchorMin = new Vector2(0, 0.5f);
        backRT.anchorMax = new Vector2(0, 0.5f);
        backRT.pivot = new Vector2(0, 0.5f);
        backRT.anchoredPosition = new Vector2(40, 0);
        backRT.sizeDelta = new Vector2(200, 80);
        backBtn.AddComponent<Button>();
        var backImg = backBtn.AddComponent<Image>();
        backImg.color = Color.clear;

        var backText = CreateTMPText("BackText", backBtn.transform, "← Back", 50, COL_DARK_BLUE);
        SetStretchFull(backText);

        // Home icon (text placeholder)
        var homeBtn = CreateUIObject("HomeButton", header.transform);
        var homeRT = homeBtn.GetComponent<RectTransform>();
        homeRT.anchorMin = new Vector2(1, 0.5f);
        homeRT.anchorMax = new Vector2(1, 0.5f);
        homeRT.pivot = new Vector2(1, 0.5f);
        homeRT.anchoredPosition = new Vector2(-40, 0);
        homeRT.sizeDelta = new Vector2(80, 80);
        homeBtn.AddComponent<Button>();
        var homeImg = homeBtn.AddComponent<Image>();
        homeImg.color = Color.clear;
        CreateTMPText("HomeIcon", homeBtn.transform, "⌂", 60, COL_DARK_BLUE);
    }

    // ─────────────────────────────────────────────────────────────
    static void BuildTitle(Transform parent)
    {
        var title = CreateUIObject("TitleText", parent);
        var rt = title.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 1f);
        rt.offsetMin = new Vector2(40, -280);
        rt.offsetMax = new Vector2(-40, -160);

        var tmp = title.AddComponent<TextMeshProUGUI>();
        tmp.text = "Family Alerts";
        tmp.fontSize = 90;
        tmp.fontStyle = FontStyles.Bold;
        tmp.color = COL_DARK_BLUE;
        tmp.alignment = TextAlignmentOptions.Left;
        ApplyFont(tmp);
    }

    // ─────────────────────────────────────────────────────────────
    static void BuildScrollArea(Transform parent)
    {
        // ScrollArea
        var scrollArea = CreateUIObject("ScrollArea", parent);
        var saRT = scrollArea.GetComponent<RectTransform>();
        saRT.anchorMin = new Vector2(0, 0);
        saRT.anchorMax = new Vector2(1, 1);
        saRT.pivot = new Vector2(0.5f, 0.5f);
        saRT.offsetMin = new Vector2(0, 160);   // above bottom nav
        saRT.offsetMax = new Vector2(0, -280);  // below title

        var scrollRect = scrollArea.AddComponent<ScrollRect>();
        scrollRect.horizontal = false;
        scrollRect.vertical = true;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;
        scrollRect.inertia = false;
        scrollRect.scrollSensitivity = 30;

        var mask = scrollArea.AddComponent<RectMask2D>();

        // Viewport (same as ScrollArea for simplicity)
        scrollRect.viewport = saRT;

        // ContentScroll
        var content = CreateUIObject("ContentScroll", scrollArea.transform);
        var cRT = content.GetComponent<RectTransform>();
        cRT.anchorMin = new Vector2(0, 1);
        cRT.anchorMax = new Vector2(1, 1);
        cRT.pivot = new Vector2(0.5f, 1f);
        cRT.anchoredPosition = new Vector2(0, 0);
        cRT.sizeDelta = new Vector2(0, 1800);

        var vlg = content.AddComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset(40, 40, 30, 30);
        vlg.spacing = 30;
        vlg.childAlignment = TextAnchor.UpperCenter;
        vlg.childControlWidth = true;
        vlg.childControlHeight = false;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        scrollRect.content = cRT;

        // Contact data
        var contacts = new (string name, string relation, bool notifyOn)[]
        {
            ("Sarah Johnson",   "Daughter",        false),
            ("Michael Smith",   "Son",              false),
            ("Emily Davis",     "Granddaughter",    true),
            ("Dr. Robert Lee",  "Caregiver",        true),
        };

        foreach (var c in contacts)
            BuildContactCard(content.transform, c.name, c.relation, c.notifyOn);
    }

    // ─────────────────────────────────────────────────────────────
    static void BuildContactCard(Transform parent, string name, string relation, bool toggleOn)
    {
        var card = CreateUIObject("ContactCard_" + name.Replace(" ", ""), parent);
        var le = card.AddComponent<LayoutElement>();
        le.minHeight = 180;
        le.preferredHeight = 180;

        var cardImg = card.AddComponent<Image>();
        cardImg.color = COL_WHITE;

        var hlg = card.AddComponent<HorizontalLayoutGroup>();
        hlg.padding = new RectOffset(30, 30, 20, 20);
        hlg.spacing = 30;
        hlg.childAlignment = TextAnchor.MiddleLeft;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = false;

        // Avatar circle
        var avatar = CreateUIObject("AvatarCircle", card.transform);
        var avatarRT = avatar.GetComponent<RectTransform>();
        avatarRT.sizeDelta = new Vector2(120, 120);
        var avatarImg = avatar.AddComponent<Image>();
        avatarImg.color = COL_AVATAR_BG;
        // Person icon text inside
        var iconText = CreateTMPText("AvatarIcon", avatar.transform, "👤", 60, COL_DARK_BLUE);
        SetStretchFull(iconText);
        iconText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        // Name + Relation vertical stack
        var textStack = CreateUIObject("TextStack", card.transform);
        var tsRT = textStack.GetComponent<RectTransform>();
        tsRT.sizeDelta = new Vector2(600, 140);
        var tsVLG = textStack.AddComponent<VerticalLayoutGroup>();
        tsVLG.childAlignment = TextAnchor.MiddleLeft;
        tsVLG.childControlWidth = true;
        tsVLG.childForceExpandWidth = true;
        tsVLG.spacing = 5;

        var nameText = CreateTMPText("NameText", textStack.transform, name, 60, COL_DARK_BLUE);
        nameText.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        var nameLE = nameText.AddComponent<LayoutElement>();
        nameLE.preferredHeight = 75;

        var relText = CreateTMPText("RelationText", textStack.transform, relation, 45, COL_GRAY_TEXT);
        var relLE = relText.AddComponent<LayoutElement>();
        relLE.preferredHeight = 55;

        // Spacer
        var spacer = CreateUIObject("Spacer", card.transform);
        var spacerLE = spacer.AddComponent<LayoutElement>();
        spacerLE.flexibleWidth = 1;

        // NOTIFY label + Toggle stack
        var notifyStack = CreateUIObject("NotifyStack", card.transform);
        var nsRT = notifyStack.GetComponent<RectTransform>();
        nsRT.sizeDelta = new Vector2(160, 140);
        var nsVLG = notifyStack.AddComponent<VerticalLayoutGroup>();
        nsVLG.childAlignment = TextAnchor.MiddleCenter;
        nsVLG.childControlWidth = true;
        nsVLG.childForceExpandWidth = true;
        nsVLG.spacing = 8;

        var notifyLabel = CreateTMPText("NotifyLabel", notifyStack.transform, "NOTIFY", 36, COL_DARK_BLUE);
        notifyLabel.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        notifyLabel.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        var nlLE = notifyLabel.AddComponent<LayoutElement>();
        nlLE.preferredHeight = 50;

        // Toggle
        var toggleGO = CreateUIObject("ToggleSwitch", notifyStack.transform);
        var toggleRT = toggleGO.GetComponent<RectTransform>();
        var tLE = toggleGO.AddComponent<LayoutElement>();
        tLE.preferredWidth = 120;
        tLE.preferredHeight = 60;

        var toggleBG = toggleGO.AddComponent<Image>();
        toggleBG.color = toggleOn ? COL_TOGGLE_ON : COL_TOGGLE_OFF;

        var toggle = toggleGO.AddComponent<Toggle>();
        toggle.isOn = toggleOn;

        // Knob
        var knob = CreateUIObject("Knob", toggleGO.transform);
        var knobRT = knob.GetComponent<RectTransform>();
        knobRT.sizeDelta = new Vector2(52, 52);
        knobRT.anchorMin = new Vector2(toggleOn ? 1 : 0, 0.5f);
        knobRT.anchorMax = new Vector2(toggleOn ? 1 : 0, 0.5f);
        knobRT.pivot = new Vector2(0.5f, 0.5f);
        knobRT.anchoredPosition = new Vector2(toggleOn ? -30 : 30, 0);
        var knobImg = knob.AddComponent<Image>();
        knobImg.color = COL_WHITE;
        toggle.graphic = knobImg;
        toggle.targetGraphic = toggleBG;

        // Wire up color change via script note in name
        // (ToggleColorSwitch script must be added manually or via separate script)
    }

    // ─────────────────────────────────────────────────────────────
    static void BuildBottomNav(Transform parent)
    {
        var nav = CreateUIObject("Bottom_Nav_Bar", parent);
        var rt = nav.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 0);
        rt.pivot = new Vector2(0.5f, 0f);
        rt.offsetMin = new Vector2(0, 0);
        rt.offsetMax = new Vector2(0, 160);

        var navImg = nav.AddComponent<Image>();
        navImg.color = COL_WHITE;

        var hlg = nav.AddComponent<HorizontalLayoutGroup>();
        hlg.padding = new RectOffset(20, 20, 10, 10);
        hlg.spacing = 0;
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.childControlWidth = false;
        hlg.childControlHeight = false;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = false;

        string[] labels = { "Home", "Profile", "Contacts" };
        string[] icons  = { "⌂", "👤", "📋" };

        foreach (var (label, icon) in System.Linq.Enumerable.Zip(labels, icons, (a, b) => (a, b)))
        {
            var btn = CreateUIObject("NavBtn_" + label, nav.transform);
            var btnRT = btn.GetComponent<RectTransform>();
            btnRT.sizeDelta = new Vector2(200, 140);
            btn.AddComponent<Button>();
            var btnImg = btn.AddComponent<Image>();
            btnImg.color = Color.clear;

            var btnVLG = btn.AddComponent<VerticalLayoutGroup>();
            btnVLG.childAlignment = TextAnchor.MiddleCenter;
            btnVLG.childControlWidth = true;
            btnVLG.childForceExpandWidth = true;
            btnVLG.spacing = 5;

            var iconGO = CreateTMPText("Icon", btn.transform, icon, 60, COL_DARK_BLUE);
            iconGO.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            var iconLE = iconGO.AddComponent<LayoutElement>();
            iconLE.preferredHeight = 70;

            var labelGO = CreateTMPText("Label", btn.transform, label, 36, COL_DARK_BLUE);
            labelGO.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            var labelLE = labelGO.AddComponent<LayoutElement>();
            labelLE.preferredHeight = 50;
        }
    }

    // ─────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────
    static GameObject CreateUIObject(string name, Transform parent)
    {
        var go = new GameObject(name);
        go.AddComponent<RectTransform>();
        go.transform.SetParent(parent, false);
        return go;
    }

    static GameObject CreateTMPText(string name, Transform parent, string text, float size, Color color)
    {
        var go = CreateUIObject(name, parent);
        go.AddComponent<CanvasRenderer>();
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.color = color;
        ApplyFont(tmp);
        return go;
    }

    static void SetStretchFull(GameObject go)
    {
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    static void ApplyFont(TextMeshProUGUI tmp)
    {
        var fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
            AssetDatabase.GUIDToAssetPath(FONT_GUID));
        if (fontAsset != null)
            tmp.font = fontAsset;
    }

    static Color HexColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color c);
        return c;
    }
}
#endif
