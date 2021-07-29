using UnityEngine;
using System.Collections.Generic;
using Utility.ObjPool;
using System.Linq;

[RequireComponent(typeof(CanvasGroup))]
public abstract class ButtonHolderBase : MonoBehaviour
{
    InstantPool<Button> instancePool;
    [SerializeField] Button buttonPref;
    [SerializeField] int initNum;
    ButtonGroup canvas;

    public List<Button> buttonList { get { return _buttonList; } }
    List<Button> _buttonList = new List<Button>();

    void Start()
    {
        _buttonList =  GetComponentsInChildren<Button>().ToList();
        instancePool = new InstantPool<Button>(transform);
        instancePool.CreatePool(buttonPref, initNum);
        canvas = GetComponent<ButtonGroup>();
    }



    protected Button GetInstance()
    {
        return instancePool.GetObj();
    }

    protected bool RegisterButton(Button button)
    {
        if (!_buttonList.Contains(button))
        {
            _buttonList.Add(button);
            button.transform.SetParent(transform);
            return true;
        }

        return false;
    }


    protected bool RemoveButton(Button button)
    {
        if (_buttonList.Contains(button))
        {
            _buttonList.Remove(button);
            return true;
        }

        return false;
    }


}