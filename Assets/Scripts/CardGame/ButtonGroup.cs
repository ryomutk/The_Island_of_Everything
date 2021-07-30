public class ButtonGroup<T>
where T : IButtonData
{
    ButtonViewBase view;
    ButtonObserver<T> observer;
    ButtonHolder<T> holder;

    public ButtonGroup(ButtonObserver<T> observer, ButtonHolder<T> holder, ButtonViewBase view = null)
    {
        this.view = view;
        this.observer = observer;
        this.holder = holder;
    }

    public virtual void Update(MouceContext context)
    {
        ButtonUpdate(context);
        observer.Update();
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