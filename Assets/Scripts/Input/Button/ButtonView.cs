using UnityEngine;
using System.Collections.Generic;

//ボタンviewの基底クラス。
public class TestButtonView
{
    protected IButtonMotionData defaultButtonMotion { get; set; }
    protected ButtonHolderBase holderBase;
    protected List<Button> buttonsWatching = new List<Button>();

    public TestButtonView(IButtonMotionData motion, ButtonHolderBase holder)
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
                var motion = defaultButtonMotion.GetUIMotion(button, ButtonMotionType.enter);
                UIMotionQueue.instance.AppendMotion(motion);
                buttonsWatching.Add(button);
            }
        }

        for (int i = 0; i < buttonsWatching.Count; i++)
        {
            var button = buttonsWatching[i];
            if (!holderBase.buttonList.Contains(button))
            {
                var motion = defaultButtonMotion.GetUIMotion(button, ButtonMotionType.exit);
                UIMotionQueue.instance.AppendMotion(motion);
                buttonsWatching.Remove(button);
            }
        }

    }
}