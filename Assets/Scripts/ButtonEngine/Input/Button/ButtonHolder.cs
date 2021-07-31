using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


//手札のデータを投げると、手札を配置してくれる
public abstract class ButtonHolder<T>:ButtonHolderBase
where T:IButtonData
{
    
    Dictionary<Button,IButtonAction<T>> _buttonActionDictionary = new Dictionary<Button, IButtonAction<T>>();
    Dictionary<Button,T> _buttonDataDictionary = new Dictionary<Button, T>();
    
    public IButtonAction<T> GetButtonAction(Button button)
    {
        return _buttonActionDictionary[button];
    }

    public T GetButtonData(Button button)
    {
        return _buttonDataDictionary[button];
    }

    [Sirenix.OdinInspector.Button]
    public void SetButton(IButtonAction<T> buttonAction)
    {
        var instance = GetInstance();
        var data = buttonAction.GetButtonData();

        RegisterButton(instance);
        LoadData(instance,buttonAction.GetButtonData());
        _buttonActionDictionary[instance] = buttonAction;
        _buttonDataDictionary[instance] = data;
    }

    [Sirenix.OdinInspector.Button]
    public void DisableButton(Button button)
    {
        button.gameObject.SetActive(false);
        button.ClearListeners();
        _buttonActionDictionary.Remove(button);
        _buttonDataDictionary.Remove(button);
        RemoveButton(button);
    }

    protected abstract void LoadData(Button instance,T data);
}