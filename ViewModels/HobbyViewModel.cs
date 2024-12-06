using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Inlämning.Models;

namespace WPF_Inlämning.ViewModels
{
    class HobbyViewModel : INotifyPropertyChanged
    {
        private string _newHobbyName;
        public string NewHobbyName
        {
            get => _newHobbyName;
            set
            {
                _newHobbyName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Hobby> Hobbies { get; set; }

        public ICommand AddHobbyCommand { get; }

        public HobbyViewModel()
        {
            
            Hobbies = new ObservableCollection<Hobby>
    {
        new Hobby { Name = "Bird watching" },
        new Hobby { Name = "Knitting" },
        new Hobby { Name = "Blacksmithing" }
    };

            
            var relayCommand = new RelayCommand(AddHobby, CanAddHobby);
            AddHobbyCommand = relayCommand;

           
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(NewHobbyName))
                {
                    relayCommand.RaiseCanExecuteChanged();
                }
            };
        }

        private void AddHobby()
        {
            if (!string.IsNullOrWhiteSpace(NewHobbyName))
            {
                Hobbies.Add(new Hobby { Name = NewHobbyName });
                NewHobbyName = string.Empty; 
            }
        }

        private bool CanAddHobby()
        {
            return !string.IsNullOrWhiteSpace(NewHobbyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

   
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();
        public void Execute(object parameter) => _execute();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}

