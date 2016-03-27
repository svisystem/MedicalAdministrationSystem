using System;
using System.ComponentModel;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{

    public abstract class NotifyPropertyChanged : IChangeTracking, INotifyPropertyChanged
    {
        protected internal NotifyPropertyChanged()
        {
            this.PropertyChanged += new PropertyChangedEventHandler(OnNotifiedOfPropertyChanged);
        }
        private void OnNotifiedOfPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e != null && !string.Equals(e.PropertyName, "IsChanged", StringComparison.Ordinal))
            {
                this.IsChanged = true;
            }
        }
        
        public bool IsChanged
        {
            get
            {
                lock (_notifyingObjectIsChangedSyncRoot)
                {
                    return _notifyingObjectIsChanged;
                }
            }

            protected set
            {
                lock (_notifyingObjectIsChangedSyncRoot)
                {
                    if (!Equals(_notifyingObjectIsChanged, value))
                    {
                        _notifyingObjectIsChanged = value;

                        this.OnPropertyChanged("IsChanged");
                    }
                }
            }
        }

        private bool _notifyingObjectIsChanged;

        private readonly object _notifyingObjectIsChangedSyncRoot = new object();
        public void AcceptChanges()
        {
            this.IsChanged = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
