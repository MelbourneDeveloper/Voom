using System;
using System.Collections.Generic;
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
        private CheckEquality<T> _raisePropertyChangedCheck;
        #endregion

        #region Constructor
        public NotifyProperty(
            INotifyPropertyChanged notifyPropertyChanged,
            string propertyName,
            ValueGet<T> valueGet = null,
            ValueSet<T> valueSet = null,
            ValueSet<T> valueSetCallback = null,
            CheckEquality<T> raisePropertyChangedCheck = null
            )
        {
            _notifyPropertyChanged = notifyPropertyChanged ?? throw new ArgumentNullException(nameof(notifyPropertyChanged));
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));

            _valueSet = valueSet;
            _valueGet = valueGet;
            _valueSetCallback = valueSetCallback;
            _raisePropertyChangedCheck = raisePropertyChangedCheck;

            if (_valueSet == null)
            {
                _valueSet = new ValueSet<T>((value) => _propertyValue = value);
            }

            if (_valueGet == null)
            {
                _valueGet = new ValueGet<T>(() => _propertyValue);
            }

            if (_raisePropertyChangedCheck == null)
            {
                _raisePropertyChangedCheck = new CheckEquality<T>((o, n) =>
                {
                    bool v = EqualityComparer<T>.Default.Equals(o, n);
                    return v;
                });
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
                var oldValue = _valueGet();
                var isEqual = _raisePropertyChangedCheck(oldValue, value);

                _valueSet(value);
                _valueSetCallback?.Invoke(value);

                if (!isEqual)
                {
                    _notifyPropertyChanged.RaisePropertyChanged(_propertyName);
                }
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
