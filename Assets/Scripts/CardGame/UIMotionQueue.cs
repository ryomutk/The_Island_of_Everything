using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using DG.Tweening;

public class UIMotionQueue : Singleton<UIMotionQueue>
{
    List<IUIMotion> motionTween;

    public void AppendMotion(IUIMotion tween)
    {
        motionTween.Add(tween);
    }

    public void ExecuteQueue()
    {
        while (motionTween.Count != 0)
        {
            //DontDisturbの場合
            if (motionTween[0].dontDisturb)
            {
                //ゲームのアップデート処理を止める
                Time.timeScale = 0;
                //終わったらまた動かす
                motionTween[0].motion.onComplete += () => Time.timeScale = 1;
                motionTween[0].motion.Play();
                motionTween.RemoveAt(0);

                //UIのdontdisturbが二回続いたとき同時に起こるとゲームが動き始める時間がずれるのでコールバックで再開するようにする
                motionTween[0].motion.onComplete += () => ExecuteQueue();
                break;
            }
            else
            {
                motionTween[0].motion.Play();
                motionTween.RemoveAt(0);
            }
            
        }
    }
}


