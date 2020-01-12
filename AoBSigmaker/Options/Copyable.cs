using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AoBSigmaker.Options
{
    public class Copyable<T> : INotifyPropertyChanged
    {
        public void CopyTo(Copyable<T> target)
        {
            foreach (PropertyInfo prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                object? value = prop.GetValue(this);
                prop.SetValue(target, value);
            }
        }

        protected void NotifOfPropertyChange([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
