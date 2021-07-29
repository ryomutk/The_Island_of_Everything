using UnityEngine;
using System.Collections.Generic;
using Utility.ObjPool;

[RequireComponent(typeof(ButtonGroup))]
public abstract class ButtonHolderBase : MonoBehaviour
{
    InstantPool<Button> instancePool;
    [SerializeField] Button buttonPref;
    [SerializeField] int initNum;
    ButtonGroup canvas;
    public List<Button> buttonList { get; private set; }

    void Start()
    {
        buttonList = new List<Button>();
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
        buttonList.Add(button);
        return canvas.RegisterButton(button);
    }

    protected bool RemoveFromCanvas(Button button)
    {
        buttonList.Remove(button);
        return canvas.RemoveButton(button);
    }

}