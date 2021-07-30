using System.Collections.Generic;


public abstract class ButtonViewBase
{
    protected IButtonMotionData defaultButtonMotion { get; set; }
    protected ButtonHolderBase holderBase;

    
    public ButtonViewBase(IButtonMotionData motion, ButtonHolderBase holder)
    {
        defaultButtonMotion = motion;
        holderBase = holder;
    }

    public abstract void Update();

}