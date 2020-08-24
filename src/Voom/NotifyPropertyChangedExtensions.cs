using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace Voom
{
    public static class NotifyPropertyChangedExtensions
    {
        #region Static Fields
        private static readonly MethodInfo _invokeMethodInfo = typeof(PropertyChangedEventHandler).GetMethod(nameof(PropertyChangedEventHandler.Invoke));
        private static readonly ConcurrentDictionary<Type, FieldInfo> _memoryCache = new ConcurrentDictionary<Type, FieldInfo>();
        #endregion

        #region Extensions
        /// <summary>
        /// Raises PropertyChanged on any object with a type that implements INotifyPropertyChanged
        /// </summary>
        /// <param name="notifyPropertyChanged">Put your ViewModel here</param>
        /// <param name="propertyName">Which property changed?</param>
        public static void RaisePropertyChanged(this INotifyPropertyChanged notifyPropertyChanged, string propertyName)
        {
            var type = notifyPropertyChanged.GetType();

            var propertyChangedField = _memoryCache.GetOrAdd(type, (cacheEntry) => type.GetField(nameof(INotifyPropertyChanged.PropertyChanged), BindingFlags.Instance | BindingFlags.NonPublic));

            var propertyChangedEventHandler = (PropertyChangedEventHandler)propertyChangedField.GetValue(notifyPropertyChanged);

            if (propertyChangedEventHandler == null) throw new PropertyChangedUnhandledException();

            _invokeMethodInfo.Invoke(propertyChangedEventHandler, new object[] { notifyPropertyChanged, new PropertyChangedEventArgs(propertyName) });
        }

        //These methods may be added in future but to keep things simple, they will not be used for now
        /*
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
        */
        #endregion
    }
}
