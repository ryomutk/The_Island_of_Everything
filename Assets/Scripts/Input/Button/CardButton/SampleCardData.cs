using UnityEngine;

[CreateAssetMenu]
//テスト用に手打ちで入力できるButtonData
public class SanpleCardData : ScriptableObject, IDetailedButtonData
{
    public Sprite image { get{return _image;} }
    public string buttonName { get{return _buttonName;} }
    public string flavorText { get{return _flavorText;} }
    public string detail{get{return _detail;}}

    [SerializeField] Sprite _image;
    [SerializeField] string _buttonName;
    [SerializeField] string _detail;
    [SerializeField] string _flavorText;
}