using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
//一個以上作っても意味ないんだからね！
public class InputConfig : ScriptableObject
{
    public static int tapFrame{get;private set;}
    public static int holdFrame{get;private set;}

    //タップと認識されるために必要な時間
    [SerializeField] int _tapFrame;
    
    //長押しと認識されるために必要な時間
    [SerializeField] int _holdFrame;

    void OnValidate()
    {
        tapFrame = _tapFrame;
        holdFrame = _holdFrame;
    }
}
