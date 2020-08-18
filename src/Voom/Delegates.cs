namespace Voom
{
    public delegate void ValueSet<T>(T value);
    public delegate T ValueGet<T>();
    public delegate void BlankDelegate();
    public delegate void RaisePropertyChanged(string propertyName);
    public delegate bool CheckEquality<T>(T oldValue, T newValue);
}
