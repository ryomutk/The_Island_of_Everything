using UnityEngine;

[CreateAssetMenu]
public class SampleButtonAction:ScriptableObject,IButtonAction<IDetailedButtonData>
{
    [SerializeField]SampleCardData cardData;
    [SerializeField]string sampleMSG;

    public void Execute()
    {
        UnityEngine.Debug.Log(sampleMSG);   
    }

    public IDetailedButtonData GetButtonData()
    {
        return cardData;
    }
}