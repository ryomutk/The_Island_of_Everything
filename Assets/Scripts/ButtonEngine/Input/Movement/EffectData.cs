using UnityEngine;
using System;
using DG.Tweening;

public abstract class EffectData<T>:ScriptableObject
{
    public Func<T,Sequence> effector{get;protected set;}

}