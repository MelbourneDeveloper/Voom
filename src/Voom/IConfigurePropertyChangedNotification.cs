namespace Voom
{
    public interface IConfigurePropertyChangedNotification<T>
    {
        T Value { get; set; }

        IConfigurePropertyChangedNotification<T> Callback(ValueSet<T> valueSetCallback);
        IConfigurePropertyChangedNotification<T> Get(ValueGet<T> valueGet);
        IConfigurePropertyChangedNotification<T> Set(ValueSet<T> valueSet);
    }
}