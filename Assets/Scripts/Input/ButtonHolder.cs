using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


//手札のデータを投げると、手札を配置してくれる
public abstract class ButtonHolder<T>:ButtonPool
where T:IButtonData
{
    //Listenerをlistにすべきかは考えて
    public Dictionary<Button,IButtonAction<T>> buttonList{get{return _buttonList;}}
    Dictionary<Button,IButtonAction<T>> _buttonList = new Dictionary<Button, IButtonAction<T>>();
    

    public void SetCard(IButtonAction<T> buttonAction)
    {
        var instance = GetInstance();
        
        RegisterButton(instance);
        LoadData(instance,buttonAction.GetCardData());
        buttonList[instance] = buttonAction;
    }

    public void RemoveCard(Button button)
    {
        button.ClearListeners();
        buttonList.Remove(button);
        RemoveButton(button);
    }

    protected abstract void LoadData(Button instance,T data);
}