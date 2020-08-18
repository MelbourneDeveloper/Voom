using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Voom
{
    public static class NotifyPropertyChangedExtensions
    {
        private static MethodInfo _invokeMethodInfo = typeof(PropertyChangedEventHandler).GetMethod(nameof(PropertyChangedEventHandler.Invoke));
        private static MemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions { });

        public static void RaisePropertyChanged(this INotifyPropertyChanged notifyPropertyChanged, string propertyName)
        {
            var type = notifyPropertyChanged.GetType();

            var propertyChangedField = _memoryCache.GetOrCreate(type, (cacheEntry) => type.GetField("PropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic));

            var propertyChangedEventHandler = (PropertyChangedEventHandler)propertyChangedField.GetValue(notifyPropertyChanged);

            _invokeMethodInfo.Invoke(propertyChangedEventHandler, new object[] { notifyPropertyChanged, new PropertyChangedEventArgs(propertyName) });
        }

        public static void SetValue<T>(this INotifyPropertyChanged notifyPropertyChanged, ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            property = value;
            notifyPropertyChanged.RaisePropertyChanged(propertyName);
        }

        public static void SetValue(this INotifyPropertyChanged notifyPropertyChanged, BlankDelegate setter,  [CallerMemberName] string propertyName = null)
        {
            setter();
            notifyPropertyChanged.RaisePropertyChanged(propertyName);
        }
    }
}
