using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//動きのデータがここに集結！
public static class MotionServer
{
    static List<IMotionData<Button>> datas = new List<IMotionData<Button>>()
    {
        new ButtonMotionBase()
    };

    public static IMotionData<Button> GetButtonMotionData(int index)
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
