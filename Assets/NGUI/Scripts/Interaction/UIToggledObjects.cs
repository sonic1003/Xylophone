//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Example script showing how to activate or deactivate a game object when a toggle's state changes.
/// OnActivate event is sent out by the UIToggle script.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Toggled Objects")]
public class UIToggledObjects : MonoBehaviour
{
    public List<GameObject> activate;
    public List<GameObject> deactivate;

    Color orange = new Color(251f / 255, 147f / 255, 73f / 255);
    Color purple = new Color(154f / 255, 170f / 255f, 239f / 255);
    public bool isStore = false;

    [HideInInspector]
    [SerializeField]
    GameObject
        target;
    [HideInInspector]
    [SerializeField]
    bool
        inverse = false;

    void Awake()
    {
        // Legacy functionality -- auto-upgrade
        if (target != null)
        {
            if (activate.Count == 0 && deactivate.Count == 0)
            {
                if (inverse)
                    deactivate.Add(target);
                else
                    activate.Add(target);
            } else
                target = null;

#if UNITY_EDITOR
            NGUITools.SetDirty(this);
#endif
        }

#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif
        UIToggle toggle = GetComponent<UIToggle>();
        EventDelegate.Add(toggle.onChange, Toggle);
    }

    public void Toggle()
    {

        bool val = UIToggle.current.value;

        if (enabled)
        {
            for (int i = 0; i < activate.Count; ++i)
            {
                Set(activate [i], val);
            }

            for (int i = 0; i < deactivate.Count; ++i)
            {

                Set(deactivate [i], !val);
            }
        }

        if (isStore)
        {
            ToggleSprite(UIToggle.current.name, UIToggle.current.value);
        }

    }

    void ToggleSprite(string tname, bool tval)
    {                  
        if (tval)
            GameObject.Find(tname).GetComponent<UISprite>().color = purple;
        else
            GameObject.Find(tname).GetComponent<UISprite>().color = orange;
    }

    void Set(GameObject go, bool state)
    {
        if (go != null)
        {
            //NGUITools.SetActive (go, state);
            //UIPanel panel = NGUITools.FindInParents<UIPanel>(target);
            //if (panel != null) panel.Refresh();
/*
            if (state)
            {
                TweenAlpha.Begin(go, 0.8f, 1f);
            }
            else
            {
                TweenAlpha.Begin(go, 0.2f, 0f);
            }*/

            NGUITools.SetActive(go, state);
        }
    }
}