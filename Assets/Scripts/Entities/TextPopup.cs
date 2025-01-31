using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    public List<TextSetting> texts;
    public TextPopupHandler prefab;
    public TextMode mode;
    public TextCondition condition;
    [SerializeField, HideInInspector] private TextPopupHandler debug;

    private void Start()
    {
        TextPopupManager.I.SetUp(this);
    }

    [Button]
    private void ShowDebug()
    {
        debug = Instantiate(prefab, transform.position + Vector3.back, Quaternion.identity, transform);
        debug.gameObject.AddComponent<DestroyOnStart>();
    }

    private void HideDebug()
    {
        if (debug)
        {
            Destroy(debug.gameObject);
        }
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