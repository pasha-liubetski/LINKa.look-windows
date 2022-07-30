using System.Windows;

namespace LinkaWPF
{
    /// <summary>
    /// Логика взаимодействия для DescriptionWindow.xaml
    /// </summary>
    public partial class DescriptionWindow : Window
    {
        public DescriptionWindow(bool editor, string description)
        {
            InitializeComponent();
            Editor = editor;
            Description = description;
            EditField.Text = description;
            EditField.Focusable = editor;
            WrapButtons.Visibility = editor ? Visibility.Visible : Visibility.Hidden;
        }

        public bool Editor { get; }
        public string Description { get; private set; }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Description = EditField.Text;
            Close();
        }
    }
}
