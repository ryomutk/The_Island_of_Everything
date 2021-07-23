using UnityEngine;
using System.Collections.Generic;
using ObserverPattern;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public Image image{get{return _image;}}
    Image _image;
    Bounds _bounds;

    [SerializeField] bool noticeOnDown = false;
    [SerializeField] Sprite normal;
    [SerializeField] Sprite pushed;

    List<IButtonListener> buttonListeners = new List<IButtonListener>();

    //押されていた時間
    int _interactFrame = 0;

    void Start()
    {
        _image = GetComponent<Image>();




        _bounds = normal.bounds;


        _bounds.extents *= _image.rectTransform.lossyScale.x * 2;
    }

    public void CopyButton(Button button)
    {
        this.normal = button.normal;
        this.pushed = button.pushed;
        this.noticeOnDown = button.noticeOnDown;
    }

    public void ClearListeners()
    {
        buttonListeners.Clear();
    }

    public bool AddObserver(IButtonListener listener)
    {
        if (!buttonListeners.Contains(listener))
        {
            buttonListeners.Add(listener);
            return true;
        }

        return false;
    }

    public bool RemoveObserver(IButtonListener listener)
    {
        if (buttonListeners.Contains(listener))
        {
            buttonListeners.Remove(listener);
            return true;
        }

        return false;
    }

    void Notice(bool holded)
    {
        for (int i = 0; i < buttonListeners.Count; i++)
        {
            buttonListeners[i].OnNotice(holded);
        }
    }

    /// <summary>
    /// STANDALONE向けUPDATE
    /// </summary>
    /// <param name="position"></param>
    /// <param name="leftButton"></param>
    public void UpdateButton(MouceContext context)
    {
        _bounds.center = (Vector2)transform.position;

        if (context.state == InputState.none || !_bounds.Contains(context.position))
        {
            _interactFrame = 0;
            _image.sprite = normal;

            return;
        }
        else if (context.state != InputState.none && _bounds.Contains(context.position))
        {

            if (context.state == InputState.pressed || context.state == InputState.start)
            {
                if (context.state == InputState.start)
                {
                    _image.sprite = pushed;
                    if (noticeOnDown)
                    {
                        //マウスの時は右クリックでholdと等価
                        Notice(context.key == KeyCode.Mouse1);
                    }
                }

                _interactFrame++;
            }
            else if (context.state == InputState.cancel)
            {
                //クリックはstartさえしてれば押し下げてた時間は関係なく通す
                if (_interactFrame > 0 && !noticeOnDown)
                {
                    _interactFrame = 0;
                    Notice(context.key == KeyCode.Mouse1);
                }
            }

            return;
        }

        Debug.LogWarning("Something may went wrong");

    }


    /// <summary>
    /// MOBILE向けUPDATE
    /// </summary>
    /// <param name="position"></param>
    public void UpdateButton(Vector2 position)
    {
        _bounds.center = (Vector2)transform.position;


        if (position == null || !_bounds.Contains(position))
        {

            //Holdより長く押してたら放しても反応しない
            if (_interactFrame < InputConfig.holdFrame)
            {
                if (!noticeOnDown && _interactFrame > InputConfig.tapFrame)
                {
                    Notice(false);
                }
            }

            _interactFrame = 0;
            _image.sprite = normal;
        }
        else
        {
            _interactFrame++;
            _image.sprite = pushed;

            if (noticeOnDown && _interactFrame == InputConfig.tapFrame)
            {
                Notice(false);
            }
            //hold判定
            else if (_interactFrame == InputConfig.holdFrame)
            {
                Notice(true);
            }
        }
    }


}