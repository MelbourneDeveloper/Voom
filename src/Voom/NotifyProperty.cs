using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Voom
{
    public class NotifyProperty<T> : INotifyProperty<T>
    {
        #region Fields
        INotifyPropertyChanged _notifyPropertyChanged;
        private T _propertyValue;
        private string _propertyName;
        private ValueSet<T> _valueSetCallback { get; set; }
        private ValueSet<T> _valueSet;
        private ValueGet<T> _valueGet;
        private CheckEquality<T> _checkConsiderEqual;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructs a NotifyProperty
        /// </summary>
        /// <param name="notifyPropertyChanged">The event source</param>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="valueGet">Getter. Defaults to getting from the backing field</param>
        /// <param name="valueSet">Setter. Defaults to setting the backing field</param>
        /// <param name="valueSetCallback">Logic that occurs after valueSet but before raising PropertyChanged on the event source</param>
        /// <param name="checkConsiderEqual">Comparison check to determine whether or not PropertyChanged is raised. If true PropertyChanged is not raised. If false, PropertyChanged is raised</param>
        public NotifyProperty(
            INotifyPropertyChanged notifyPropertyChanged,
            string propertyName,
            ValueGet<T> valueGet = null,
            ValueSet<T> valueSet = null,
            ValueSet<T> valueSetCallback = null,
            CheckEquality<T> checkConsiderEqual = null
            )
        {
            _notifyPropertyChanged = notifyPropertyChanged ?? throw new ArgumentNullException(nameof(notifyPropertyChanged));
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));

            _valueSet = valueSet;
            _valueGet = valueGet;
            _valueSetCallback = valueSetCallback;
            _checkConsiderEqual = checkConsiderEqual;

            if (_valueSet == null)
            {
                _valueSet = new ValueSet<T>((value) => _propertyValue = value);
            }

            if (_valueGet == null)
            {
                _valueGet = new ValueGet<T>(() => _propertyValue);
            }

            if (_checkConsiderEqual == null)
            {
                _checkConsiderEqual = new CheckEquality<T>((o, n) => EqualityComparer<T>.Default.Equals(o, n));
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
                var isEqual = _checkConsiderEqual(oldValue, value);

                _valueSet(value);
                _valueSetCallback?.Invoke(value);

                if (!isEqual)
                {
                    _notifyPropertyChanged.RaisePropertyChanged(_propertyName);
                }
            }
        }

        public INotifyProperty<T> Get(ValueGet<T> valueGet)
        {
            _valueGet = valueGet;
            return this;
        }

        public INotifyProperty<T> Set(ValueSet<T> valueSet)
        {
            _valueSet = valueSet;
            return this;
        }

        public INotifyProperty<T> Callback(ValueSet<T> valueSetCallback)
        {
            _valueSetCallback = valueSetCallback;
            return this;
        }

        public INotifyProperty<T> ConsiderValueEqualWhen(CheckEquality<T> checkConsiderEqual)
        {
            _checkConsiderEqual = checkConsiderEqual;
            return this;
        }
        #endregion
    }
}
