using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


//手札のデータを投げると、手札を配置してくれる
public abstract class ButtonHolder<T>:ButtonHolderBase
where T:IButtonData
{
    
    //Listenerをlistにすべきかは考えて
    public Dictionary<Button,IButtonAction<T>> buttonDictionary{get{return _buttonDictionary;}}
    Dictionary<Button,IButtonAction<T>> _buttonDictionary = new Dictionary<Button, IButtonAction<T>>();
    


    public void SetButton(IButtonAction<T> buttonAction)
    {
        var instance = GetInstance();
        
        RegisterButton(instance);
        LoadData(instance,buttonAction.GetCardData());
        buttonDictionary[instance] = buttonAction;
    }

    public void DisableButton(Button button)
    {
        button.gameObject.SetActive(false);
        button.ClearListeners();
        buttonDictionary.Remove(button);
        RemoveFromCanvas(button);
    }

    protected abstract void LoadData(Button instance,T data);
}