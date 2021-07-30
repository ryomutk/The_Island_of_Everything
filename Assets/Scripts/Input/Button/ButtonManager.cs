using UnityEngine;


public class ButtonManager:MonoBehaviour
{
    ButtonGroup<IDetailedButtonData> cardButtonGroup;
    [SerializeField] DetailedButtonHolder cardButtonHolder;
    [SerializeField] ButtonGroupType cardButtonType = ButtonGroupType.monoSelect;


    void Start()
    {
        var testMotion = ButtonMotionServer.GetButtonMotionData(0);
        cardButtonGroup = new ButtonGroup<IDetailedButtonData>(new ButtonObserver<IDetailedButtonData>(cardButtonHolder,cardButtonType),cardButtonHolder,new TestButtonView(testMotion,cardButtonHolder));
    }


    void Update()
    {
        var context = ButtonInputRecever.GetMouceContext();
        cardButtonGroup.Update(context);
    }
}