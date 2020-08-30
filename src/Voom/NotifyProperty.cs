using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Voom
{
    public class NotifyProperty<T> : INotifyProperty<T>
    {
        #region Fields
        private readonly INotifyPropertyChanged _notifyPropertyChanged;
        private T _propertyValue;
        private readonly string _propertyName;
        private ValueSet<T> _beforePropertyChanged { get; set; }
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
        internal NotifyProperty(
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
            _beforePropertyChanged = valueSetCallback;
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

        #region Hidden Members
        ValueGet<T> INotifyProperty<T>.ValueGet { get => _valueGet; set => _valueGet = value; }
        ValueSet<T> INotifyProperty<T>.ValueSet { get => _valueSet; set => _valueSet = value; }
        ValueSet<T> INotifyProperty<T>.BeforePropertyChanged { get => _beforePropertyChanged; set => _beforePropertyChanged = value; }
        CheckEquality<T> INotifyProperty<T>.CheckConsiderEqual { get => _checkConsiderEqual; set => _checkConsiderEqual = value; }

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

                //TODO: This is comparing the old value to the new value but what if the new value is not relevant? What if the getter gets a completely different value?
                var isEqual = _checkConsiderEqual(oldValue, value);

                _valueSet(value);
                _beforePropertyChanged?.Invoke(value);

                if (!isEqual)
                {
                    _notifyPropertyChanged.RaisePropertyChanged(_propertyName);
                }
            }
        }
        #endregion
    }
}
