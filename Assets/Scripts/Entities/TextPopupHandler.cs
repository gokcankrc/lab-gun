using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPopupHandler : MonoBehaviour
{
    public List<TextSetting> texts;
    public TextMode mode;
    public TextCondition condition;
    public TextMeshProUGUI text;
    public Transform container;
    private bool active;
    private float timer;
    private int index;
    private TextSetting current => texts[index % texts.Count];

    private void Start()
    {
        switch (condition)
        {
            case TextCondition.BeforeContainment:
                GameManager.I.onContainmentWallBroken += Deactivate;
                Activate();
                break;
            case TextCondition.AfterContainment:
                GameManager.I.onContainmentWallBroken += Activate;
                Deactivate();
                break;
            case TextCondition.Always:
                Activate();
                break;
        }
    }

    public void Init(TextPopup dep)
    {
        texts = dep.texts;
        mode = dep.mode;
        condition = dep.condition;
    }

    private void Activate(int _ = 0)
    {
        active = true;
        text.text = current.text;
        container.gameObject.SetActive(true);
        timer = current.duration;
    }

    private void Deactivate(int _ = 0)
    {
        active = false;
        container.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!active) return;
        switch (mode)
        {
            case TextMode.Looping:
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    index += 1;
                    timer = current.duration;
                    text.text = current.text;
                }

                break;
            case TextMode.AlwaysActive:
                break;
            case TextMode.Once:
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    Deactivate();
                    enabled = false;
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}