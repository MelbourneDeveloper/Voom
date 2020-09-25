using System;
using System.ComponentModel;

namespace Voom.Tests
{
    public class MockNotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _Id;

        public string Id
        {
            get => _Id;
            set 
            { 
                _Id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
            }
        }

        public MockNotifyPropertyChanged(IObservable<string> idStream = null)
        {
            if (idStream != null)
            {
                idStream.Subscribe((id) => Id = id);
            }
        }
    }
}
