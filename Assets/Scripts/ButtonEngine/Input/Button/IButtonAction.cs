public interface IButtonAction<T>
where T:IButtonData
{
    void Execute();
    T GetButtonData();
}