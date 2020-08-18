namespace Voom
{
    public interface INotifyProperty<T>
    {
        T Value { get; set; }

        INotifyProperty<T> Callback(ValueSet<T> valueSetCallback);
        INotifyProperty<T> Get(ValueGet<T> valueGet);
        INotifyProperty<T> Set(ValueSet<T> valueSet);
        INotifyProperty<T> ConsiderValueEqualWhen(CheckEquality<T> checkConsiderEqual);
    }
}