using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using Voom;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private const string PropertyName = "test";
        private bool _propertyChangedWasRaised = false;
        private bool _callbackValue = false;

        [TestMethod]
        public void TestCanRaisePropertyChanged()
        {
            var notifyPropertyChanged = new MockNotifyPropertyChanged();
            notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
            notifyPropertyChanged.RaisePropertyChanged(PropertyName);
            Assert.IsTrue(_propertyChangedWasRaised);
        }


        [TestMethod]
        public void TestNotifyPropertyRaisesPropertyChanged()
        {
            var notifyPropertyChanged = new MockNotifyPropertyChanged();
            var notifyProperty = new NotifyProperty<string>(notifyPropertyChanged, PropertyName);
            notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
            notifyPropertyChanged.RaisePropertyChanged(PropertyName);
            Assert.IsTrue(_propertyChangedWasRaised);
        }

        [TestMethod]
        public void TestCallbackIsCalled()
        {
            var notifyPropertyChanged = new MockNotifyPropertyChanged();
            var notifyProperty = new NotifyProperty<string>(notifyPropertyChanged, PropertyName).Callback((a) => _callbackValue = true);
            notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
            notifyProperty.Value = "test";
            Assert.IsTrue(_callbackValue);
        }

        [TestMethod]
        public void TestNeverConsiderValueEqual()
        {
            var notifyPropertyChanged = new MockNotifyPropertyChanged();
            var notifyProperty = new NotifyProperty<string>(notifyPropertyChanged, PropertyName).ConsiderValueEqualWhen((o, n) => false);
            notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
            notifyProperty.Value = null;
            Assert.IsTrue(_propertyChangedWasRaised);
        }

        [TestMethod]
        public void TestAlwaysConsiderValueEqual()
        {
            var notifyPropertyChanged = new MockNotifyPropertyChanged();
            var notifyProperty = new NotifyProperty<string>(notifyPropertyChanged, PropertyName).ConsiderValueEqualWhen((o, n) => true);
            notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
            notifyProperty.Value = null;
            Assert.IsFalse(_propertyChangedWasRaised);
        }

        [TestMethod]
        public void TestEqualValueRaisePropertyChangedDoesntFire()
        {
            var notifyPropertyChanged = new MockNotifyPropertyChanged();
            var notifyProperty = new NotifyProperty<string>(notifyPropertyChanged, PropertyName);
            notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
            notifyProperty.Value = null;
            Assert.IsFalse(_propertyChangedWasRaised);
        }

        [TestMethod]
        public void TestNotEqualValueRaisePropertyChangedDoesFire()
        {
            var notifyPropertyChanged = new MockNotifyPropertyChanged();
            var notifyProperty = new NotifyProperty<string>(notifyPropertyChanged, PropertyName);
            notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
            notifyProperty.Value = "";
            Assert.IsTrue(_propertyChangedWasRaised);
        }

        [TestMethod]
        public void TestThrowsEventHandlerNotFound()
        {
            var notifyProperty = new NotifyProperty<string>(new MockNotifyPropertyChanged(), PropertyName);
            
            Assert.ThrowsException<PropertyChangedUnhandledException>(()=> notifyProperty.Value = "test");
        }

        private void NotifyPropertyChanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _propertyChangedWasRaised = e.PropertyName == PropertyName;
        }
    }
}
