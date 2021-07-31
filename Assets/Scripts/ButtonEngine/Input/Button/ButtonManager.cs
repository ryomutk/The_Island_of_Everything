using UnityEngine;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour
{
    ButtonGroup<IDetailedButtonData> cardButtonGroup;
    [SerializeField]  DetailedButtonHolder cardButtonHolder;
    [SerializeField] ButtonGroupType cardButtonType = ButtonGroupType.monoSelect;
    [SerializeField] bool active = true;


    void Start()
    {
        var testMotion = MotionServer.GetButtonMotionData(0);
        cardButtonGroup = new ButtonGroup<IDetailedButtonData>(new ButtonModel<IDetailedButtonData>(cardButtonType), cardButtonHolder, new TestButtonView(testMotion, cardButtonHolder));
    }


    void Update()
    {

        if (active)
        {
            var context = ButtonInputRecever.GetMouceContext();

            cardButtonGroup.Update(context);
            UIMotionQueue.instance.ExecuteQueue();
        }
    }
}