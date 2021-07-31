using UnityEngine;
using System.Collections.Generic;

//ボタンviewの基底クラス。
public class TestButtonView : ButtonObserverBase
{

    public TestButtonView(IMotionData<Button> motion, ButtonHolderBase holderBase) : base(motion, holderBase)
    { }


    protected override void OnAppend(Button button)
    {
        var motion = defaultButtonMotion.GetUIMotion(button, ButtonMotionType.enter);
        UIMotionQueue.instance.AppendMotion(motion);
    }

    protected override void OnRemove(Button button)
    {
        var motion = defaultButtonMotion.GetUIMotion(button, ButtonMotionType.exit);
        UIMotionQueue.instance.AppendMotion(motion);
    }

    protected override void OnSelect(Button button)
    {
        var motion = defaultButtonMotion.GetUIMotion(button, ButtonMotionType.selected);
        UIMotionQueue.instance.AppendMotion(motion);
    }

    protected override void OnDisselect(Button button)
    {
        var motion = defaultButtonMotion.GetUIMotion(button, ButtonMotionType.def);
        UIMotionQueue.instance.AppendMotion(motion);
    }
}