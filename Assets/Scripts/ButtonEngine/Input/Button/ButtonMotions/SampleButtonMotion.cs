using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class ButtonMotionBase : IMotionData<Button>
{
    float appearDuration = 0.3f;
    Ease ease = Ease.InQuart;
    float exitDuration = 1f;
    float exitRaiseAmount = 0.5f;
    Animator nullAnimator = new Animator();
    Dictionary<Button, Animator> storedAnimator = new Dictionary<Button, Animator>();


    public virtual IUIMotion GetUIMotion(Button target, ButtonMotionType type)
    {
        var animator = GetAnimator(target);
        IUIMotion mo;

        if (type == ButtonMotionType.enter)
        {
            mo = ApperenceTween(target);
            mo.motion.onComplete += () => animator.SetTrigger("Normal");
        }
        else if (type == ButtonMotionType.exit)
        {
            mo = ExitTween(target);
            mo.motion.onPlay += () => animator.SetTrigger("Disabled");
        }
        else if(type == ButtonMotionType.def)
        {
            mo = DefaultTween(target);
            mo.motion.onPlay += () => animator.SetTrigger("Normal");
        }
        else if(type == ButtonMotionType.selected)
        {
            mo = SelectedTween(target);
            mo.motion.onPlay += () => animator.SetTrigger("Selected");
        }
        else
        {
            mo = new UIMotion(false,GetNullTween());
        }
        

        return mo;
    }

    UIMotion DefaultTween(Button target)
    {
        Tween a = GetNullTween();

        return new UIMotion(false,a);
    }

    protected virtual UIMotion ApperenceTween(Button target)
    {
        var defaultScale = target.transform.localScale.y;
        var defPosition = target.transform.position;
        var s = DOTween.Sequence();

        
        s.Append(target.image.DOFade(0, 0));
        s.Append(target.transform.DOScaleY(0, 0));
        s.Append(target.transform.DOScaleY(defaultScale, appearDuration)).SetEase(ease);
        s.Join(target.image.DOFade(1, appearDuration)).SetEase(ease);

        return new UIMotion(false, s);
    }

    protected virtual UIMotion ExitTween(Button target)
    {
        var defaultScale = target.transform.lossyScale.y;
        var defPosition = target.transform.position;
        var s = DOTween.Sequence();

        s.Join(target.image.DOFade(0, exitDuration))
        .Join(target.transform.DOLocalMoveX(exitRaiseAmount, exitDuration)).SetRelative().SetEase(ease);

        return new UIMotion(false, s);
    }

    protected virtual UIMotion SelectedTween(Button target)
    {
        var tw = GetNullTween();

        return new UIMotion(false,tw);
    }

    protected Animator GetAnimator(Button button)
    {
        if (storedAnimator.ContainsKey(button))
        {
            return storedAnimator[button];
        }
        else
        {
            var animator = button.GetComponentInChildren<Animator>();

            if (animator != null)
            {
                storedAnimator[button] = animator;
            }
            else
            {
                animator = nullAnimator;
                storedAnimator[button] = nullAnimator;
            }

            return animator;
        }
    }

    protected Tween GetNullTween()
    {
        return DOTween.To(()=>0,(x)=>{},0,0);
    }
}