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

        [TestMethod]
        public void TestMethod1()
        {
            var notifyPropertyChanged = new NotifyPropertyChanged();
            notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
            notifyPropertyChanged.RaisePropertyChanged(PropertyName);
            Assert.IsTrue(_propertyChangedWasRaised);
        }


        [TestMethod]
        public void TestMethod2()
        {
            var notifyPropertyChanged = new NotifyPropertyChanged();
            var notifyProperty = new NotifyProperty<string>(notifyPropertyChanged, PropertyName);
            notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
            notifyPropertyChanged.RaisePropertyChanged(PropertyName);
            Assert.IsTrue(_propertyChangedWasRaised);
        }

        private void NotifyPropertyChanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _propertyChangedWasRaised = e.PropertyName == PropertyName;
        }
    }

    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
