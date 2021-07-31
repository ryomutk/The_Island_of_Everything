using System.Collections.Generic;
using UnityEngine;


public abstract class ButtonObserverBase
{
    protected IMotionData<Button> defaultButtonMotion { get; set; }
    protected ButtonHolderBase holderBase;

    List<Button> buttonsWatching = new List<Button>();
    List<Button> selectedButtons = new List<Button>();
    List<Button> hoveredButtons = new List<Button>();


    public ButtonObserverBase(IMotionData<Button> motion, ButtonHolderBase holder)
    {
        defaultButtonMotion = motion;
        holderBase = holder;
    }

    public void Update()
    {
        DetectState();
    }

    void DetectState()
    {
        for (int i = 0; i < holderBase.buttonList.Count; i++)
        {
            var button = holderBase.buttonList[i];
            if (!buttonsWatching.Contains(button))
            {
                OnAppend(button);
                buttonsWatching.Add(button);
            }
        }

        for (int i = 0; i < buttonsWatching.Count; i++)
        {
            var button = buttonsWatching[i];
            if (!holderBase.buttonList.Contains(button))
            {
                OnRemove(button);
                buttonsWatching.Remove(button);
                continue;
            }

            if (button.isSelected && !selectedButtons.Contains(button))
            {
                selectedButtons.Add(button);
                OnSelect(button);
            }
            else if (!button.isSelected && selectedButtons.Contains(button))
            {
                selectedButtons.Remove(button);
                OnDisselect(button);
            }

            if(button.isHovered && !hoveredButtons.Contains(button))
            {
                OnHover(button);
                hoveredButtons.Remove(button);
            }
            else if(!button.isHovered && selectedButtons.Contains(button))
            {
                OnDisHover(button);
                hoveredButtons.Remove(button);
            }
        }
    }

    //ボタンが追加されたときに呼ばれる
    protected virtual void OnAppend(Button button)
    { }

    protected virtual void OnRemove(Button button)
    { }

    protected virtual void OnSelect(Button button)
    { }

    protected virtual void OnDisselect(Button button)
    {}

    protected virtual void OnHover(Button button)
    { }

    protected virtual void OnDisHover(Button button)
    { }
}