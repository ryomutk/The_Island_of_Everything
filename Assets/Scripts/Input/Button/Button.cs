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
    Sprite normal
    {
        get
        {
            return _normal;
        }

        set
        {
            _normal = value;
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

        set{
            _selected = value;
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

    public bool CheckIfListening(IButtonListener listener)
    {
        return buttonListeners.Contains(listener);
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



    void Notice()
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
        pressedCode = KeyCode.None;

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
                        pressedCode = context.key;
                        Notice();
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
                    pressedCode = context.key;
                    Notice();
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
        pressedCode = KeyCode.None;

        _bounds.center = (Vector2)transform.position;


        if (position == null || !_bounds.Contains(position))
        {

            //Holdより長く押してたら放しても反応しない
            if (_interactFrame < InputConfig.holdFrame)
            {
                if (!noticeOnDown && _interactFrame > InputConfig.tapFrame)
                {
                    //スマホは二種類しかないことが保証されているので、mouse0,1で分別する。とりあえず。
                    pressedCode = KeyCode.Mouse0;
                    Notice();
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
                pressedCode = KeyCode.Mouse0;
                Notice();
            }
            //hold判定
            else if (_interactFrame == InputConfig.holdFrame)
            {
                pressedCode = KeyCode.Mouse1;
                Notice();
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