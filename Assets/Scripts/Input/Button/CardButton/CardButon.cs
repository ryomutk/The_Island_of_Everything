using UnityEngine;
using UnityEngine.UI;
using TMPro;

//フレーバーテキストとイメージがセットできる
public class CardButton:Button
{
    public Image flavorImage{get{return _flavorImage;}}
    public TMP_Text detail{get{return _detal;}}

    Image _flavorImage;
    TMP_Text _detal;

    void Start()
    {
        _detal = GetComponentInChildren<TMP_Text>();
        //自分にもボタン用のimageがついているので応急処置。
        _flavorImage = GetComponentsInChildren<Image>()[1];
    }
}