using UnityEngine;
using System.Collections.Generic;


public static class ButtonInputRecever
{
    
#if (UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE)
    static MouceContext context = new MouceContext();
    static KeyCode[] observeButtons = new KeyCode[] { KeyCode.Mouse0, KeyCode.Mouse1 };
    static InputState lastState;
#endif

    public static MouceContext GetMouceContext()
    {
#if (UNITY_IOS || UNITY_ANDROID)
        clickPosition = Input.GetTouch(0).position;

        for (int i = 0; i < buttonList.Count; i++)
        {
            throw new System.NotImplementedException();
        }
#endif



#if (UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE)

        lastState = context.state;
        context.state = InputState.none;


        for (int i = 0; i < observeButtons.Length; i++)
        {
            var nowButton = observeButtons[i];
            if (Input.GetKey(nowButton))
            {
                context.key = nowButton;

                switch (lastState)
                {
                    case InputState.none:
                        context.state = InputState.start;
                        break;

                    default:
                        context.state = InputState.pressed;
                        break;
                }
            }
        }

        if (context.state == InputState.none && lastState == InputState.pressed)
        {
            context.state = InputState.cancel;
        }

        if (context.state != InputState.none)
        {
            context.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        return context;
#endif
    }
}