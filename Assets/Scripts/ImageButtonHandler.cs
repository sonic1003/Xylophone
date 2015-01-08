using UnityEngine;
using System.Collections;

public class ImageButtonHandler : MonoBehaviour
{

    public GameObject toggleButtonObj;
    private UIButton button;

    void Start()
    {
        button = toggleButtonObj.GetComponent<UIButton>();
    }

    protected virtual void OnClick()
    {
        if (button.normalSprite == "Xylo_Play")
        {
            button.normalSprite = "Xylo_Stop";
            button.pressedSprite = "Xylo_Stop_active";
        } else
        {
            button.normalSprite = "Xylo_Play";
            button.pressedSprite = "Xylo_Play_active";
        }
    }
		
}
