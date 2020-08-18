namespace Voom
{
    public static class NotifyPropertyExtensions
    {
        public static INotifyProperty<T> Get<T>(this INotifyProperty<T> notifyProperty, ValueGet<T> valueGet)
        {
            notifyProperty.ValueGet = valueGet;
            return notifyProperty;
        }

        public static INotifyProperty<T> Set<T>(this INotifyProperty<T> notifyProperty, ValueSet<T> valueSet)
        {
            notifyProperty.ValueSet = valueSet;
            return notifyProperty;
        }

        public static INotifyProperty<T> Callback<T>(this INotifyProperty<T> notifyProperty, ValueSet<T> valueSetCallback)
        {
            notifyProperty.ValueSetCallback = valueSetCallback;
            return notifyProperty;
        }

        public static INotifyProperty<T> ConsiderValueEqualWhen<T>(this INotifyProperty<T> notifyProperty, CheckEquality<T> checkConsiderEqual)
        {
            notifyProperty.CheckConsiderEqual = checkConsiderEqual;
            return notifyProperty;
        }
    }
}
