using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(CanvasGroup))]
public class ButtonCanvas : MonoBehaviour
{
    public List<Button> buttonList{get{return _buttonList;}}

    List<Button> _buttonList = new List<Button>();


    void Start()
    {
        _buttonList = GetComponentsInChildren<Button>().ToList();
    }

    public bool RegisterButton(Button button)
    {
        if (!_buttonList.Contains(button))
        {
            _buttonList.Add(button);
            button.transform.SetParent(transform);
            return true;
        }

        return false;
    }

    public bool RemoveButton(Button button)
    {
        if (_buttonList.Contains(button))
        {
            _buttonList.Remove(button);
            return true;
        }

        return false;
    }

    Vector3 clickPosition = new Vector3();

#if (UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE)
    MouceContext context = new MouceContext();
    KeyCode[] observeButtons = new KeyCode[] { KeyCode.Mouse0, KeyCode.Mouse1 };
    InputState lastState;
#endif

    public void UpdateButons()
    {
#if (UNITY_IOS || UNITY_ANDROID)
        clickPosition = Input.GetTouch(0).position;

        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].UpdateButton(Camera.current.ScreenToWorldPoint(clickPosition));
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



        for (int i = 0; i < _buttonList.Count; i++)
        {
            _buttonList[i].UpdateButton(context);
        }
#endif
    }
}