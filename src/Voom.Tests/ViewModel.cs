using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Voom.Tests
{
    public class ViewModel : INotifyPropertyChanged, IDisposable
    {
        public IReadOnlyCollection<IDevice> Devices { get; private set; } = new ReadOnlyCollection<IDevice>(new List<IDevice>());
        public event PropertyChangedEventHandler? PropertyChanged;
        private bool _isConnected;
        private IDevice? _selectedDevice;
        ISubscribable _subscribable;


        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                _isConnected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConnected)));
            }
        }

        public IDevice? SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                _selectedDevice = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedDevice)));
            }
        }

        public ViewModel(ISubscribable subscribable)
        {
            _subscribable = subscribable;
            subscribable.Subscribe(this, new Action<IReadOnlyCollection<IDevice>>(DeviceListUpdated));
        }

        private void DeviceListUpdated(IReadOnlyCollection<IDevice> devices)
        {
            Devices = devices;

            if (!Devices.Contains(SelectedDevice))
            {
                SelectedDevice = null;
            }

            if (SelectedDevice == null)
            {
                SelectedDevice = Devices.FirstOrDefault();
            }
        }

        public void Dispose()
        {
            _subscribable.Unsubscribe(this);
        }
    }

    public interface IDevice { }
    public class Device : IDevice { }
}

