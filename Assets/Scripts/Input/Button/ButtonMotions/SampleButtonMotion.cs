using UnityEngine;
using DG.Tweening;

public class SampleButtonMotion:IButtonMotionData
{
     float appearDuration = 0.3f;
    Ease ease = Ease.InQuart;
    float exitDuration = 1f;
    float exitRaiseAmount = 0.5f;

    public IUIMotion GetUIMotion(Button target,ButtonMotionType type)
    {
        if(type == ButtonMotionType.enter)
        {
            return ApperenceTween(target);
        }
        else if(type == ButtonMotionType.exit)
        {
            return ExitTween(target);
        }


        return null;
    }

    UIMotion ApperenceTween(Button target)
    {
        var defaultScale = target.transform.lossyScale.y;
        var defPosition = target.transform.position;
        var s = DOTween.Sequence();

        s.Append(target.image.DOFade(0,0));
        s.Append(target.transform.DOScaleY(0,0));
        s.Append(target.transform.DOMove(GeometricPlace.deckPosition,0));
        s.Append(target.transform.DOScaleY(defaultScale,appearDuration)).SetEase(ease);
        s.Join(target.image.DOFade(1,appearDuration)).SetEase(ease);
        s.Join(target.transform.DOMove(defPosition,appearDuration)).SetEase(ease);

        return new UIMotion(false,s);
    }

    UIMotion ExitTween(Button target)
    {
        var defaultScale = target.transform.lossyScale.y;
        var defPosition = target.transform.position;
        var s = DOTween.Sequence();

        s.Join(target.image.DOFade(0,exitDuration))
        .Join(target.transform.DOLocalMoveX(exitRaiseAmount,exitDuration)).SetRelative().SetEase(ease);

        return new UIMotion(false,s);
    }
    

}