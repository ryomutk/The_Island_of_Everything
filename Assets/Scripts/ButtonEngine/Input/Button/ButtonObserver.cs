using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// もっとも簡単なボタンの入力処理機関
/// </summary>
/// 
public class ButtonModel<T> : IButtonListener
where T:IButtonData
{
    protected ButtonGroupType type;

    //二つ以上の同時入力を拒否。
    bool avoidMultiInput = true;

    //次のフレームに処理されるボタンたち
    List<Button> buttonQueue = new List<Button>();

    public ButtonModel(ButtonGroupType type)
    {
        this.type = type;
    }

    public void OnNotice(Button button)
    {
        buttonQueue.Add(button);
    }


    public void Update(ButtonHolder<T> holder)
    {
        CheckButtonObservility(holder);
        var result = SolveQueue(holder);
    }

    void CheckButtonObservility(ButtonHolder<T> holder)
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

    int SolveQueue(ButtonHolder<T> holder)
    {
        if (buttonQueue.Count <= 0)
        {
            return 0;
        }


        SolveNext(holder);

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
                    SolveNext(holder);
                }

                return 2; //多重入力を処理
            }
        }

        return 1; //通常終了
    }

    //Queue内の次をsolve
    void SolveNext(ButtonHolder<T> holder)
    {
        var button = buttonQueue[0];
        var action = holder.GetButtonAction(button);

        switch (type)
        {
            case ButtonGroupType.toggle:
                button.isSelected = !button.isSelected;
                break;

            case ButtonGroupType.monoSelect:
                if (!button.isSelected)
                {
                    DisSelectAll(holder);
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

    void DisSelectAll(ButtonHolder<T> holder)
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