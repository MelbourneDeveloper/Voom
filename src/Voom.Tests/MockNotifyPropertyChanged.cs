using System.ComponentModel;

namespace Voom.Tests
{
    public class MockNotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
