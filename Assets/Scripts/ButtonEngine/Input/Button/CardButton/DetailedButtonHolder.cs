

public class DetailedButtonHolder:ButtonHolder<IDetailedButtonData>
{
    protected override void LoadData(Button instance, IDetailedButtonData data)
    {
        var button = instance as CardButton;

        button.cardName.text = data.buttonName;
        button.flavorImage.sprite = data.image;
        button.detail.text = data.detail;
    }
}