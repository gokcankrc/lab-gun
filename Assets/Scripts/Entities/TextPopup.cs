using System;
using System.Collections.Generic;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    public List<TextSetting> texts;
    public TextPopupHandler prefab;
    public TextMode mode;
    public TextCondition condition;

    private void Start()
    {
        TextPopupManager.I.SetUp(this);
    }

    [Serializable]
    public class Dep
    {
        public List<TextSetting> texts;
        public TextMode mode;
        public TextCondition condition;
    }
}

[Serializable]
public class TextSetting
{
    public string text;
    public float duration = 3f;
}

public enum TextMode
{
    Looping,
    AlwaysActive,
    Once
}

public enum TextCondition
{
    BeforeContainment,
    AfterContainment,
    Always,
}