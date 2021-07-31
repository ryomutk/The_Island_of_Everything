using UnityEngine;
using UnityEngine.UI;
using TMPro;

//フレーバーテキストとイメージがセットできる
public class CardButton:Button
{
    public Image flavorImage{get{return _flavorImage;}}
    public TMP_Text detail{get{return _detal;}}
    public TMP_Text cardName{get{return _name;}}

    [SerializeField]Image _flavorImage;
    [SerializeField]TMP_Text _name;
    [SerializeField]TMP_Text _detal;


}