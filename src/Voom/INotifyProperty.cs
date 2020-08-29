namespace Voom
{
    public interface INotifyProperty<T>
    {
        internal ValueGet<T> ValueGet { get; set; }
        internal ValueSet<T> ValueSet { get; set; }
        internal ValueSet<T> BeforePropertyChanged { get; set; }
        internal CheckEquality<T> CheckConsiderEqual { get; set; }

        T Value { get; set; }
    }
}