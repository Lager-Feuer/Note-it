using System.Collections.ObjectModel;

namespace Note_it.Model
{
    class Notebook : ViewModelBase
    {
        public Notebook()
        {
            this.Notizen = new ObservableCollection<Note>();
        }

        private string _title;

        public ObservableCollection<Note> Notizen { get; set; }
        public string Title { get { return _title; } set { _title = value; NotifyPropertyChanged(); } }
    }
}
