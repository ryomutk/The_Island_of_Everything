using UnityEngine;
using System.Collections.Generic;
using ObserverPattern;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public Image image { get { return _image; } }
    public KeyCode pressedCode = KeyCode.None;
    public bool isSelected { get { return _isSelected; } set { _isSelected = value; } }
    Image _image;
    Bounds _bounds;
    bool _isSelected = false;

    [SerializeField] ButtonType type;
    [SerializeField] bool noticeOnDown = false;
    [SerializeField] Sprite _normal;
    Sprite pushed
    {
        get
        {
            if (_pushed == null)
            {
                return _normal;
            }
            return _pushed;
        }

        set
        {
            _pushed = value;
        }
    }

    Sprite selected
    {
        get
        {
            if (_selected == null)
            {
                return _normal;
            }

            return _selected;
        }
    }

    [SerializeField] Sprite _pushed;
    [SerializeField] Sprite _selected;

    List<IButtonListener> buttonListeners = new List<IButtonListener>();

    //押されていた時間
    int _interactFrame = 0;

    void Start()
    {
        _image = GetComponent<Image>();
        InitBounds();
    }

    void InitBounds()
    {
        _bounds = _normal.bounds;
        _bounds.extents *= _image.rectTransform.lossyScale.x * 2;
    }

    public void InitButton(Button button)
    {
        this._normal = button._normal;
        this.pushed = button.pushed;
        this.noticeOnDown = button.noticeOnDown;

        InitBounds();
    }



    public void ClearListeners()
    {
        buttonListeners.Clear();
    }

    public bool AddListener(IButtonListener listener)
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
            buttonListeners[i].OnNotice(this);
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

        if (type == ButtonType.selectOnHover)
        {
            if (_bounds.Contains(context.position))
            {
                _isSelected = true;
                image.sprite = selected;
            }
            else
            {
                _isSelected = false;
                image.sprite = _normal;
            }
        }


        if (context.state == InputState.none || !_bounds.Contains(context.position))
        {
            _interactFrame = 0;
            _image.sprite = _normal;

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
            _image.sprite = _normal;
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

    void OnDisable()
    {
        _isSelected = false;
    }


}

public enum ButtonType
{
    //普通の一回押したら信号を発信するボタン
    normal,
    //hoverでselectedになるボタン
    selectOnHover
}