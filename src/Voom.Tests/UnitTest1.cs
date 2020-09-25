using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Voom;
using System.Reactive;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace Voom.Tests
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
            var notifyProperty = notifyPropertyChanged.CreateNotifyProperty<string>(PropertyName);
            notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
            notifyPropertyChanged.RaisePropertyChanged(PropertyName);
            Assert.IsTrue(_propertyChangedWasRaised);
        }

        [TestMethod]
        public void TestCallbackIsCalled()
        {
            var notifyPropertyChanged = new MockNotifyPropertyChanged();
            var notifyProperty = notifyPropertyChanged.CreateNotifyProperty<string>(PropertyName).BeforePropertyChanged((a) => _callbackValue = true);
            notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
            notifyProperty.Value = "test";
            Assert.IsTrue(_callbackValue);
        }

        [TestMethod]
        public void TestNeverConsiderValueEqual()
        {
            var notifyPropertyChanged = new MockNotifyPropertyChanged();
            var notifyProperty = notifyPropertyChanged.CreateNotifyProperty<string>(PropertyName).ConsiderValueEqualWhen((o, n) => false);
            notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
            notifyProperty.Value = null;
            Assert.IsTrue(_propertyChangedWasRaised);
        }

        [TestMethod]
        public void TestAlwaysConsiderValueEqual()
        {
            var notifyPropertyChanged = new MockNotifyPropertyChanged();
            var notifyProperty = notifyPropertyChanged.CreateNotifyProperty<string>(PropertyName).ConsiderValueEqualWhen((o, n) => true);
            notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
            notifyProperty.Value = null;
            Assert.IsFalse(_propertyChangedWasRaised);
        }

        [TestMethod]
        public void TestEqualValueRaisePropertyChangedDoesntFire()
        {
            var notifyPropertyChanged = new MockNotifyPropertyChanged();
            var notifyProperty = notifyPropertyChanged.CreateNotifyProperty<string>(PropertyName);
            notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
            notifyProperty.Value = null;
            Assert.IsFalse(_propertyChangedWasRaised);
        }

        [TestMethod]
        public void TestNotEqualValueRaisePropertyChangedDoesFire()
        {
            var notifyPropertyChanged = new MockNotifyPropertyChanged();
            var notifyProperty = notifyPropertyChanged.CreateNotifyProperty<string>(PropertyName);
            notifyPropertyChanged.PropertyChanged += NotifyPropertyChanged_PropertyChanged;
            notifyProperty.Value = "";
            Assert.IsTrue(_propertyChangedWasRaised);
        }

        [TestMethod]
        public void TestDoesntEventHandlerNotFound()
        {
            var notifyProperty = new MockNotifyPropertyChanged().CreateNotifyProperty<string>(PropertyName);

            notifyProperty.Value = "test";
        }


        [TestMethod]
        public void TestCallChainingSetsValue()
        {
            var value = 1;
            var expectedValue = 2;
            var callbackMade = false;

            var notifyPropertyChanged = new MockNotifyPropertyChanged();
            var notifyProperty =
                notifyPropertyChanged.CreateNotifyProperty<int>(PropertyName)
                .BeforePropertyChanged(a => callbackMade = true)
                .ConsiderValueEqualWhen((o, n) => o != n)
                .Get(() => value)
                .Set((v) => value = v);


            notifyProperty.Value = expectedValue;

            Assert.AreEqual(expectedValue, value);
            Assert.IsTrue(callbackMade);
        }

        [TestMethod]
        public async Task TestPublish()
        {
            var devices = new List<IDevice> { new Device() };
            var publisher = new PublishingHub();
            using (var viewModel = new ViewModel(publisher))
            {
                publisher.Publish<IReadOnlyCollection<IDevice>>(new ReadOnlyCollection<IDevice>(devices));
                Assert.AreEqual(devices.First(), viewModel.Devices.First());
            }
        }


        [TestMethod]
        public async Task SampleNotTestRxUI1()
        {
            var notifyPropertyChanged = new MockNotifyPropertyChanged();

            var subject = new Subject<string>();

            subject.Subscribe((id) => notifyPropertyChanged.Id = id);

            subject.OnNext("123");
        }

        [TestMethod]
        public async Task SampleNotTestRxUI2()
        {
            var subject = new Subject<string>();

            new MockNotifyPropertyChanged(subject);

            subject.OnNext("123");
        }

        [TestMethod]
        public async Task SampleNotTestRxUI3()
        {
            var subject = new Subject<string>();

            var notifyPropertyChanged = new MockNotifyPropertyChanged();

            notifyPropertyChanged.PropertyChanged += (s, e) => subject.OnNext(notifyPropertyChanged.Id);
        }

        [TestMethod]
        public async Task SampleNotTestRxUI4()
        {
            var notifyPropertyChanged = new MockNotifyPropertyChanged();

            var observable = Observable.FromEventPattern(notifyPropertyChanged, nameof(MockNotifyPropertyChanged.PropertyChanged));

            observable.Where((a) =>
            {
                var eventArgs = (PropertyChangedEventArgs)a.EventArgs;
                return eventArgs.PropertyName == nameof(MockNotifyPropertyChanged.Id) ? true : false;
            }).Subscribe((a) =>
            {
                var eventArgs = (PropertyChangedEventArgs)a.EventArgs;
                Console.WriteLine($"Property: { eventArgs.PropertyName} Value: {notifyPropertyChanged.Id}");
            });

            notifyPropertyChanged.Id = "test";
        }

        private void NotifyPropertyChanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _propertyChangedWasRaised = e.PropertyName == PropertyName;
        }
    }
}
