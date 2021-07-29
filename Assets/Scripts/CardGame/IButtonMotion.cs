using DG.Tweening;


public interface IButtonMotionData
{
    IUIMotion GetUIMotion(Button target,ButtonMotionType type);
}


public enum ButtonMotionType
{
    enter,
    exit,
    selected,

}
