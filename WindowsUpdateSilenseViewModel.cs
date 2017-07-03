using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WindowsUpdateSilence
{
    class Command : ICommand
    {
        private Action<object> _action;

        public Command(Action<object> action)
        {
            _action = action;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;// parameter != null && parameter.ToString() != "";
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (parameter != null)
            {
                _action(parameter);
            }
        }

        #endregion
    }

    class WindowsUpdateSilenseViewModel : INotifyPropertyChanged
    {         
        private ICommand _disableCommand;
        public ICommand DisableCommand
        {
            get
            {
                return _disableCommand;
            }
            set
            {
                _disableCommand = value;
            }
        }

        private ICommand _enableCommand;
        public ICommand EnableCommand
        {
            get
            {
                return _enableCommand;
            }
            set
            {
                _enableCommand = value;
            }
        }

        private ICommand _stopCommand;
        public ICommand StopCommand
        {
            get
            {
                return _stopCommand;
            }
            set
            {
                _stopCommand = value;
            }
        }

        private ICommand _startCommand;
        public ICommand StartCommand
        {
            get
            {
                return _startCommand;
            }
            set
            {
                _startCommand = value;
            }
        }

        private ICommand _statusCommand;
        public ICommand StatusCommand
        {
            get
            {
                return _statusCommand;
            }
            set
            {
                _statusCommand = value;
            }
        }

        private string _serviceName;
        public string ServiceName
        {
            get
            {
                return _serviceName;
            }
            set
            {
                
                if(ReplaceService(value))
                 {
                    _serviceName = value;
                    OnPropertyChanged("ServiceName");

                }
            }
        }

        private WindowsServiceMonitor _windowsServiceMonitor;

        public WindowsUpdateSilenseViewModel()
        {
            ServiceName = "Windows Update";

            DisableCommand = new Command(new Action<object>(DisableService));
            EnableCommand = new Command(new Action<object>(EnableService));
            StartCommand = new Command(new Action<object>(StartService));
            StopCommand = new Command(new Action<object>(StopService));
            StatusCommand = new Command(new Action<object>(StatusService));
        }

        private void StatusService(object obj)
        {
            try
            {
                string status = "IsDisabled ? " + (_windowsServiceMonitor.IsDisabled ? "V" : "X") + "\n" +
                            "IsStopped ? " + (_windowsServiceMonitor.IsStopped ? "V" : "X") + "\n" +
                            "IsRunning ? " + (_windowsServiceMonitor.IsRunning ? "V" : "X");
                MessageBox.Show(status);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!" + ex.Message + ", Inner:" + ex.InnerException);
            }        
        }

        private void DisableService(object obj)
        {
            try
            {
                _windowsServiceMonitor.Disable();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!" + ex.Message + ", Inner:" + ex.InnerException);
            }

        }

        private void EnableService(object obj)
        {
            try
            {
                _windowsServiceMonitor.Enable();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!" + ex.Message + ", Inner:" + ex.InnerException);
            }

        }

        private void StartService(object obj)
        {
            try
            {
                _windowsServiceMonitor.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!" + ex.Message + ", Inner:" + ex.InnerException);
            }

        }

        private void StopService(object obj)
        {
            try
            {
                _windowsServiceMonitor.Stop();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!" + ex.Message + ", Inner:" + ex.InnerException);
            }
        }

        private bool ReplaceService(string newServiceName)
        {
            if (!WindowsServiceMonitor.IsExsist(newServiceName))
               
            {
                MessageBox.Show("Error! Service not exsist");
                return false;
            }

            try
            {
                _windowsServiceMonitor = new WindowsServiceMonitor(newServiceName);
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!" + ex.Message + ", Inner:" + ex.InnerException);
                return false;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
