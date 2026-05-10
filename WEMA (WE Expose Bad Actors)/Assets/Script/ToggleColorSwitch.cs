using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach this to each ToggleSwitch GameObject in the Family Alerts scene.
/// It handles the green/gray color change and knob position when toggled.
/// </summary>
public class ToggleColorSwitch : MonoBehaviour
{
    [Header("References")]
    public Toggle toggle;
    public Image  toggleBackground;
    public RectTransform knob;

    [Header("Colors")]
    public Color onColor  = new Color(0.298f, 0.686f, 0.314f); // #4CAF50
    public Color offColor = new Color(0.800f, 0.800f, 0.800f); // #CCCCCC

    [Header("Knob Positions")]
    public float knobOnX  =  30f;
    public float knobOffX = -30f;

    void Start()
    {
        // Auto-find references if not assigned
        if (toggle == null)
            toggle = GetComponent<Toggle>();
        if (toggleBackground == null)
            toggleBackground = GetComponent<Image>();
        if (knob == null && transform.childCount > 0)
            knob = transform.GetChild(0).GetComponent<RectTransform>();

        toggle.onValueChanged.AddListener(OnToggleChanged);
        
        // Set initial state without animation
        ApplyState(toggle.isOn, instant: true);
    }

    void OnToggleChanged(bool isOn)
    {
        ApplyState(isOn, instant: false);
    }

    void ApplyState(bool isOn, bool instant)
    {
        if (toggleBackground != null)
            toggleBackground.color = isOn ? onColor : offColor;

        if (knob != null)
        {
            float targetX = isOn ? knobOnX : knobOffX;
            if (instant)
            {
                var pos = knob.anchoredPosition;
                pos.x = targetX;
                knob.anchoredPosition = pos;
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(SlideKnob(targetX));
            }
        }
    }

    System.Collections.IEnumerator SlideKnob(float targetX)
    {
        float duration = 0.15f;
        float elapsed  = 0f;
        float startX   = knob.anchoredPosition.x;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            t = t * t * (3f - 2f * t); // smoothstep

            var pos = knob.anchoredPosition;
            pos.x = Mathf.Lerp(startX, targetX, t);
            knob.anchoredPosition = pos;
            yield return null;
        }

        var finalPos = knob.anchoredPosition;
        finalPos.x = targetX;
        knob.anchoredPosition = finalPos;
    }
}
