using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//データがここに集結！
public static class ButtonMotionServer
{
    [SerializeField] static List<IButtonMotionData> datas = new List<IButtonMotionData>()
    {
        new SampleButtonMotion()
    };

    public static IButtonMotionData GetButtonMotionData(int index)
    {
        return datas[index];
    }
}

public struct UIMotion:IUIMotion
{
    public bool dontDisturb{get;set;}
    public Tween motion{get;set;}

    public UIMotion(bool dontdisturb,Tween motion)
    {
        dontDisturb = dontdisturb;
        this.motion = motion;
    }
}
