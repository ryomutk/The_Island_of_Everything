using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// もっとも簡単なボタンの入力処理機関
/// </summary>
/// <typeparam name="T"></typeparam>
public class ButtonObserver<T> : IButtonListener
where T : IButtonData
{
    [SerializeField] protected ButtonGroupType type;
    [SerializeField] protected ButtonHolder<T> holder;

    //二つ以上の同時入力を拒否。
    [SerializeField] bool avoidMultiInput = true;

    //次のフレームに処理されるボタンたち
    List<Button> buttonQueue = new List<Button>();

    public ButtonObserver(ButtonHolder<T> holder)
    {
        this.holder = holder;
    }

    public void OnNotice(Button button)
    {
        buttonQueue.Add(button);
    }


    public void Update()
    {
        CheckButtonObservility();
        SolveQueue();
    }

    void CheckButtonObservility()
    {
        for (int i = 0; i < holder.buttonList.Count; i++)
        {
            var button = holder.buttonList[i];
            if (!button.CheckIfListening(this))
            {
                button.AddListener(this);
            }
        }
    }

    int SolveQueue()
    {
        if (buttonQueue.Count < 0)
        {
            return 0;
        }


        SolveNext();

        //一つやってもまだ残ってたら
        if (buttonQueue.Count > 0)
        {

            if (avoidMultiInput)
            {
                buttonQueue.Clear();
                return -1; //多重入力をキャンセル
            }
            else
            {
                while (buttonQueue.Count > 0)
                {
                    SolveNext();
                }

                return 2; //多重入力を処理
            }
        }

        return 1; //通常終了
    }

    //Queue内の次をsolve
    void SolveNext()
    {
        var button = buttonQueue[0];
        var action = holder.buttonDictionary[button];

        switch (type)
        {
            case ButtonGroupType.toggle:
                button.isSelected = !button.isSelected;
                break;

            case ButtonGroupType.monoSelect:
                if (!button.isSelected)
                {
                    DisSelectAll();
                    button.isSelected = true;
                }
                else
                {
                    action.Execute();
                }
                break;

            case ButtonGroupType.pushOnly:
                action.Execute();
                break;
        }

        buttonQueue.RemoveAt(0);
    }

    void DisSelectAll()
    {
        for (int i = 0; i < holder.buttonList.Count; i++)
        {
            holder.buttonList[i].isSelected = false;
        }
    }

}


public enum ButtonGroupType
{
    pushOnly,     //普通に押したら実行
    toggle,       //実行はせず、selectedのオンオフだけ行う
    monoSelect　　//こいつがselectを設定するタイプのうち、複数選択を禁止するタイプ
    //multiSelect　いや、これはtoggleでしょ
}