using System;
using System.ComponentModel;

namespace Voom
{
    public class NotifyProperty<T> : IConfigurePropertyChangedNotification<T>
    {
        #region Fields
        INotifyPropertyChanged _notifyPropertyChanged;
        private T _propertyValue;
        private string _propertyName;
        private ValueSet<T> _valueSetCallback { get; set; }
        private ValueSet<T> _valueSet;
        private ValueGet<T> _valueGet;
        #endregion

        #region Constructor
        public NotifyProperty(
            INotifyPropertyChanged notifyPropertyChanged,
            string propertyName,
            ValueGet<T> valueGet = null,
            ValueSet<T> valueSet = null,
            ValueSet<T> valueSetCallback = null)
        {
            _notifyPropertyChanged = notifyPropertyChanged ?? throw new ArgumentNullException(nameof(notifyPropertyChanged));
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));

            _valueSet = valueSet;
            _valueGet = valueGet;
            _valueSetCallback = valueSetCallback;

            if (_valueSet == null)
            {
                _valueSet = new ValueSet<T>((value) => _propertyValue = value);
            }

            if (_valueGet == null)
            {
                _valueGet = new ValueGet<T>(() => _propertyValue);
            }
        }
        #endregion

        #region Implicit Operators
        public static implicit operator T(NotifyProperty<T> p) => p.Value;
        #endregion

        #region Properties
        public T Value
        {
            get
            {
                return _valueGet();
            }
            set
            {
                _valueSet(value);
                _valueSetCallback?.Invoke(value);
                _notifyPropertyChanged.RaisePropertyChanged(_propertyName);
            }
        }

        public IConfigurePropertyChangedNotification<T> Get(ValueGet<T> valueGet)
        {
            _valueGet = valueGet;
            return this;
        }

        public IConfigurePropertyChangedNotification<T> Set(ValueSet<T> valueSet)
        {
            _valueSet = valueSet;
            return this;
        }

        public IConfigurePropertyChangedNotification<T> Callback(ValueSet<T> valueSetCallback)
        {
            _valueSetCallback = valueSetCallback;
            return this;
        }
        #endregion
    }
}
