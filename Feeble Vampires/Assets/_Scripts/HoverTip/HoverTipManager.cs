using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverTipManager : MonoBehaviour
{
    public TextMeshProUGUI tipText;
    public RectTransform tipWindow;

    public static Action<String, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;

    private void OnEnable()
    {
        OnMouseHover += showTip;
        OnMouseLoseFocus += hideTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= showTip;
        OnMouseLoseFocus -= hideTip;
    }

    // Start is called before the first frame update
    void Start()
    {
        hideTip();
    }

    void showTip(string tip, Vector2 mousePos)
    {
        tipText.text = tip;
        tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 200 ? 200 : tipText.preferredWidth, tipText.preferredHeight);

        tipWindow.gameObject.SetActive(true);
        tipWindow.transform.position = new Vector2(mousePos.x + tipWindow.sizeDelta.x * 2, mousePos.y);
    }

    void hideTip()
    {
        tipText.text = default;
        tipWindow.gameObject.SetActive(false);
    }
}
