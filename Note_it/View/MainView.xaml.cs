using Note_it.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Note_it.View
{
    public partial class MainView : SWWindow
    {
        public static MainView MainViewInstance { get; private set; }
        public MainView()
        {
            //DataContext = new MainViewModel();
            MainViewInstance = this;
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.CloseButton != null)
            {
                this.CloseButton.Click -= CloseButton_Click;
                this.CloseButton.Click += CloseButtonNew_Click;
            }
        }

        protected void CloseButtonNew_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationView confirmationView = new ConfirmationView();
            confirmationView.messageLabel.Text = "Möchten Sie das Programm wirklich beenden?";
            confirmationView.cancelBtn.Visibility = Visibility.Visible;
            confirmationView.ShowDialog();
            confirmationView.Close();
            if (confirmationView.isActionConfirmed)
                Application.Current.Shutdown();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            MainViewInstance = null;
            ((MainViewModel)DataContext).saveNotebooks();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView notesView = sender as ListView;
            ((MainViewModel)DataContext).ListViewSelectionChanged((Model.Note)notesView.SelectedItem);
        }

        private void NotebookView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView notebookView = sender as ListView;
            ((MainViewModel)DataContext).NotebookViewSelectionChanged((Model.Notebook)notebookView.SelectedItem);
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Model.Note noteItem = ((MainViewModel)DataContext).DynamicNotesCollection[0] as Model.Note;
                TextBox searchField = sender as TextBox;
                if (!noteItem.Content.Contains(searchField.Text)) return;

                ContentPresenter c = (ContentPresenter)DynamicContent.ItemContainerGenerator.ContainerFromIndex(0);
                TextBox notesTextBox = c.ContentTemplate.FindName("NotesTextBox", c) as TextBox;
                notesTextBox.Focus();
                notesTextBox.Select(noteItem.Content.IndexOf(searchField.Text), searchField.Text.Length);
            }
            //((MainViewModel)DataContext).TextBoxTextChanged(searchField.Text);
        }
    }
}
