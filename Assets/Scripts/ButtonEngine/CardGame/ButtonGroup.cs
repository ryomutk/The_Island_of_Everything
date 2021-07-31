public class ButtonGroup<T>
where T : IButtonData
{
    ButtonObserverBase view;
    ButtonModel<T> observer;
    ButtonHolder<T> holder;

    public ButtonGroup(ButtonModel<T> observer, ButtonHolder<T> holder, ButtonObserverBase view = null)
    {
        this.view = view;
        this.observer = observer;
        this.holder = holder;
    }

    public virtual void Update(MouceContext context)
    {
        ButtonUpdate(context);
        observer.Update(holder);
        view.Update();
    }

    void ButtonUpdate(MouceContext context)
    {
        for(int i = 0;i < holder.buttonList.Count;i++)
        {
            holder.buttonList[i].UpdateButton(context);
        }
    }
}