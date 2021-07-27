using UnityEngine;
using Utility.ObjPool;

[RequireComponent(typeof(ButtonCanvas))]
public abstract class ButtonPool:MonoBehaviour
{
    InstantPool<Button> instancePool;
    [SerializeField] Button buttonPref;
    [SerializeField] int initNum;
    ButtonCanvas canvas;

    void Start()
    {
        instancePool = new InstantPool<Button>(transform);
        instancePool.CreatePool(buttonPref,initNum);
        canvas  = GetComponent<ButtonCanvas>();
    }



    protected Button GetInstance()
    {
        return instancePool.GetObj();
    }

    protected bool RegisterButton(Button button)
    {
        return canvas.RegisterButton(button);
    }

    protected bool RemoveButton(Button button)
    {
        return canvas.RemoveButton(button);
    }

}