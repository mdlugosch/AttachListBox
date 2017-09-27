using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AttachListBox
{
    class SendListBoxCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        MainViewModel vm;

        public SendListBoxCommand(MainViewModel vm)
        {
            this.vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            vm.SendListBoxCommand();
        }
    }
}
