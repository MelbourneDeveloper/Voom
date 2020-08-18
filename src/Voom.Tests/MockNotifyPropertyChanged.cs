using System.ComponentModel;

namespace UnitTests
{
    public class MockNotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
