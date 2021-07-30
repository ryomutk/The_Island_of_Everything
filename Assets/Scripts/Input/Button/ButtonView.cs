using UnityEngine;
using System.Collections.Generic;

//ボタンviewの基底クラス。
public class TestButtonView:ButtonViewBase
{
    public TestButtonView(IButtonMotionData motion,ButtonHolderBase holderBase):base(motion,holderBase)
    {}

    protected List<Button> buttonsWatching = new List<Button>();

    public override void Update()
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