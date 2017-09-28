using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttachListBox
{
    class SortedThemeList
    {
        public string Gruppe { get; set; }
        public ObservableCollection<Thema_CheckList> ThemeList { get; set; }
    }
}
