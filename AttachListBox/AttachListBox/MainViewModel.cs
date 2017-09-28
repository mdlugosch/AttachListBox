using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AttachListBox
{
    class MainViewModel : INotifyCollectionChanged
    {
        #region Commands
        SendListBoxCommand sendListBox;
        public ICommand SendListBox { get
            {
                if (sendListBox == null)
                {
                    sendListBox = new SendListBoxCommand(this);
                    return sendListBox;
                } else return sendListBox;
            }
        }

        SendTreeViewCommand sendTreeView;
        public ICommand SendTreeView
        {
            get
            {
                if (sendTreeView == null)
                {
                    sendTreeView = new SendTreeViewCommand(this);
                    return sendTreeView;
                }
                else return sendTreeView;
            }
        }
        #endregion

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #region Themen-Datenliste
        ObservableCollection<Thema> themen;
        public ObservableCollection<Thema> ThemenListe { get { return themen; } }
        #endregion

        ObservableCollection<SortedThemeList> sortedThemes = new ObservableCollection<SortedThemeList>();
        public ObservableCollection<SortedThemeList> SortedThemes { get { return sortedThemes; } }

        #region List mit ausgewählten Themen
        public ObservableCollection<Thema> themeResult = new ObservableCollection<Thema>();
        public ObservableCollection<Thema> ThemeResult { get { return themeResult; } }
        #endregion

        ObservableCollection<String> themen_grp { get; set; }

        #region Personen-Datenliste
        ObservableCollection<Person> persons;
        public ObservableCollection<Person> PersonList { get { return persons; } }
        #endregion
        #region Datenliste die als Checkliste erweitert ist.
        ObservableCollection<Person_CheckList> persons_Checklist;
        public ObservableCollection<Person_CheckList> Persons_Checklist { get { return persons_Checklist; } }      
        #endregion
        #region Filterliste für ListBox-Benutzerauswahl
        ObservableCollection<Person_CheckList> persons_Filterlist = new ObservableCollection<Person_CheckList>();
        public ObservableCollection<Person_CheckList> Persons_Filterlist { get { return persons_Filterlist; } }
        #endregion

        public MainViewModel()
        {
            persons = new ObservableCollection<Person>();
            persons_Checklist = new ObservableCollection<Person_CheckList>();

            themen = new ObservableCollection<Thema>();

            #region DummyDaten Personenliste erstellen
            for (int i=1; i<11; i++)
            persons.Add(new AttachListBox.Person { Id = i, Name = "Person" + i });
            #endregion

            #region DummyDaten ThemenListe erstellen
            string[] gruppen = new string[] { "GruppeA","GruppeB","GruppeC","GruppeD","GruppeE"};
            int j = 1;
            for (int i = 1; i < 21; i++)
            {
                themen.Add(new Thema { Ident = i, Bezeichnung = "Thema" + i, Gruppe = gruppen[j++] });
                if (j >= gruppen.Length) j = 0;
            }
            #endregion

            persons_Checklist = ComposeCheckList(persons);
            BuildThemeList();
        }

        public ObservableCollection<Person_CheckList> ComposeCheckList(ObservableCollection<Person> p)
        {
            ObservableCollection<Person_CheckList> checklist = new ObservableCollection<Person_CheckList>();

            foreach (Person element in p)
            {
                checklist.Add(new Person_CheckList() { IsChecked=false, PersonObj=element});
            }

            return checklist;
        }

        public void BuildThemeList()
        {
            var groupQuery = from q in themen
                             group q by q.Gruppe;

            foreach (var element in groupQuery)
            {
                var themeQuery = from q in themen
                                 where q.Gruppe == element.Key
                                 select new Thema_CheckList()
                                 {
                                    Bezeichnung = q.Bezeichnung,
                                    Gruppe = q.Gruppe,
                                    Ident = q.Ident,
                                    IsChecked = false
                                 };

                sortedThemes.Add(new SortedThemeList() { Gruppe = element.Key, ThemeList = new ObservableCollection<Thema_CheckList>(themeQuery) });
            }
        }

        public void GetThemeResultList()
        {
            ThemeResult.Clear();

            foreach (SortedThemeList tList in sortedThemes)
            {
                foreach (Thema_CheckList t in tList.ThemeList)
                {
                    if(t.IsChecked) ThemeResult.Add(t);
                }
            }
        }

        #region Command-Methode
        public void SendListBoxCommand()
        {
            persons_Filterlist.Clear();

            foreach (Person_CheckList element in persons_Checklist)
            {
                if (element.IsChecked) persons_Filterlist.Add(element);
            }
        }

        public void SendTreeViewCommand()
        {
            GetThemeResultList();
        }
            #endregion
        }
}
