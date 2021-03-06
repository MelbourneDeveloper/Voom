﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Voom
{
    public static class Extensions
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

        public static INotifyProperty<T> BeforePropertyChanged<T>(this INotifyProperty<T> notifyProperty, ValueSet<T> valueSetCallback)
        {
            notifyProperty.BeforePropertyChanged = valueSetCallback;
            return notifyProperty;
        }

        public static INotifyProperty<T> ConsiderValueEqualWhen<T>(this INotifyProperty<T> notifyProperty, CheckEquality<T> checkConsiderEqual)
        {
            notifyProperty.CheckConsiderEqual = checkConsiderEqual;
            return notifyProperty;
        }

        public static INotifyProperty<T> CreateNotifyProperty<T>(this INotifyPropertyChanged notifyPropertyChanged, string propertyName)
        {
            return new NotifyProperty<T>(notifyPropertyChanged, propertyName);
        }
    }
}
