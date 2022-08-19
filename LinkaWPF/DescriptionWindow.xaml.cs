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
        }

        public bool Editor { get; }
        public string Description { get; private set; }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
