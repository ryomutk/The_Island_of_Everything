

public interface IButtonListener
{
    //とりあえず引数はboolこれ以上にすると面倒なので。buttonそのものを投げるように設計を変えてね
    void OnNotice(bool arg);
}