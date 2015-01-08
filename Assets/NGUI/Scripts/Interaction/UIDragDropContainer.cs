//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag and Drop Container")]
public class UIDragDropContainer : MonoBehaviour
{
    public Transform reparentTarget;
    public bool scaleToOne;
    public Vector3 bottomLine;
    public bool resizeChild;
    public bool noRestriction;
}
