using System;
using System.IO;
using System.Linq;
using System.Windows;
using Model.Core;

namespace PetShelter
{
    public partial class MainWindow : Window
    {
        private readonly ShelterManager _shelterManager;
        private string _currentFormat = "JSON";

        public MainWindow()
        {
            _shelterManager = new ShelterManager();
            _shelterManager.LoadOrCreateShelters();

            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Populate shelters
            CboShelter.Items.Add("(Все приюты)");
            foreach (var shelter in _shelterManager.Shelters)
            {
                CboShelter.Items.Add(shelter.Name);
            }
            CboShelter.SelectedIndex = 0;

            // Populate pet types
            CboPetType.Items.Add("(Все виды)");
            CboPetType.Items.Add("Cat");
            CboPetType.Items.Add("Dog");
            CboPetType.Items.Add("Rabbit");
            CboPetType.SelectedIndex = 0;

            // Populate formats
            CboFormat.Items.Add("JSON");
            CboFormat.Items.Add("XML");
            CboFormat.SelectedIndex = 0;

            CboFormat.SelectionChanged += CboFormat_SelectionChanged;
        }

        private void CboFormat_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CboFormat.SelectedItem != null)
            {
                _currentFormat = CboFormat.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(_currentFormat))
                {
                    ConvertReportsToFormat(_currentFormat);
                }
            }
        }

        private void ConvertReportsToFormat(string targetFormat)
        {
            var reportsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            if (!Directory.Exists(reportsDir))
                return;

            string sourcePattern = targetFormat == "JSON" ? "*.xml" : "*.json";
            var sourceFiles = Directory.GetFiles(reportsDir, sourcePattern);

            foreach (var file in sourceFiles)
            {
                try
                {
                    string content = File.ReadAllText(file);
                    string newFileName = Path.GetFileNameWithoutExtension(file) + "." + targetFormat.ToLower();
                    string newFilePath = Path.Combine(reportsDir, newFileName);
                    File.WriteAllText(newFilePath, content);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при конвертации файла: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void BtnShowPets_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get selected values
                string? shelterName = CboShelter.SelectedItem?.ToString();
                if (shelterName == "(Все приюты)") shelterName = null;

                string? petTypeName = CboPetType.SelectedItem?.ToString();
                if (petTypeName == "(Все виды)") petTypeName = null;

                Type? petType = null;
                if (!string.IsNullOrEmpty(petTypeName))
                {
                    petType = petTypeName switch
                    {
                        "Cat" => typeof(Cat),
                        "Dog" => typeof(Dog),
                        "Rabbit" => typeof(Rabbit),
                        _ => null
                    };
                }

                bool openAreaOnly = ChkOpenAreaOnly.IsChecked ?? false;

                // Get filtered pets
                var filteredPets = _shelterManager.GetFilteredPets(shelterName, petType, openAreaOnly);

                // Get the selected shelter
                Shelter? selectedShelter = null;
                if (!string.IsNullOrEmpty(shelterName))
                {
                    selectedShelter = _shelterManager.GetShelterByName(shelterName);
                }

                // Open the pets window
                var petsWindow = new PetsWindow(filteredPets, selectedShelter, _shelterManager, _currentFormat);
                petsWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}