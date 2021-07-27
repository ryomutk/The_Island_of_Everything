using UnityEngine;
using Utility.ObjPool;

[RequireComponent(typeof(ButtonCanvas))]
public abstract class ButtonUtility:MonoBehaviour
{
    InstantPool<Button> buttonPool;
    [SerializeField] Button buttonPref;
    [SerializeField] int initNum;
    ButtonCanvas holder;

    void Start()
    {
        buttonPool = new InstantPool<Button>(transform);
        buttonPool.CreatePool(buttonPref,initNum);
        holder  = GetComponent<ButtonCanvas>();
    }


    protected Button GetInstance(params IButtonListener[] listeners)
    {
        var instance = buttonPool.GetObj();
        
        if(listeners != null)
        {
            instance.ClearListeners();
            instance.isSelected = false;
            for(int i = 0;i < listeners.Length;i++)
            {
                instance.AddListener(listeners[i]);
            }
        }

        return instance;
    }



    protected Button GetInstance()
    {
        return buttonPool.GetObj();
    }

    protected bool RegisterButton(Button button)
    {
        return holder.RegisterButton(button);
    }

    protected bool RemoveButton(Button button)
    {
        return holder.RemoveButton(button);
    }

}