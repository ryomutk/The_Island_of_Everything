using DG.Tweening;
using UnityEngine;

public interface IMotionData<T>
where T:MonoBehaviour
{
    IUIMotion GetUIMotion(T target,ButtonMotionType type);
}


public enum ButtonMotionType
{
    def,
    enter,
    exit,
    selected,
    hover
}
