using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Model.Core;
using Model.Data;

namespace PetShelter
{
    public partial class PetsWindow : Window
    {
        private readonly ObservableCollection<PetViewModel> _petsViewModel;
        private readonly Shelter? _selectedShelter;
        private readonly ShelterManager _shelterManager;
        private readonly string _reportFormat;
        private int _reportCounter = 0;

        public PetsWindow(System.Collections.Generic.List<Pet> pets, Shelter? selectedShelter, 
                         ShelterManager shelterManager, string reportFormat)
        {
            _selectedShelter = selectedShelter;
            _shelterManager = shelterManager;
            _reportFormat = reportFormat;

            InitializeComponent();

            // Initialize info label
            LblInfo.Content = selectedShelter != null 
                ? $"Приют: {selectedShelter.Name}" 
                : "Все приюты";

            // Enable/disable buttons based on shelter selection
            BtnAddPet.IsEnabled = selectedShelter != null;
            BtnRemovePet.IsEnabled = selectedShelter != null;

            // Create view models
            _petsViewModel = new ObservableCollection<PetViewModel>();
            foreach (var pet in pets)
            {
                var shelter = _shelterManager.FindShelterForPet(pet);
                _petsViewModel.Add(new PetViewModel(pet, shelter?.Name ?? "Неизвестно"));
            }

            DataGridPets.ItemsSource = _petsViewModel;
        }

        private void BtnAddPet_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedShelter == null) return;

            var addWindow = new AddPetWindow(_selectedShelter);
            if (addWindow.ShowDialog() == true && addWindow.NewPet != null)
            {
                if (_selectedShelter is IChangeable changeable)
                {
                    if (changeable.AddPet(addWindow.NewPet))
                    {
                        _shelterManager.SaveShelter(_selectedShelter);
                        
                        var shelter = _shelterManager.FindShelterForPet(addWindow.NewPet);
                        _petsViewModel.Add(new PetViewModel(addWindow.NewPet, shelter?.Name ?? "Неизвестно"));
                        
                        MessageBox.Show($"Питомец {addWindow.NewPet.Name} успешно добавлен!", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось добавить питомца. Проверьте параметры приюта.", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void BtnRemovePet_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedShelter == null || DataGridPets.SelectedItem == null) return;

            var result = MessageBox.Show("Вы уверены, что хотите удалить выбранного питомца?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var viewModel = (PetViewModel)DataGridPets.SelectedItem;
                var petToRemove = viewModel.Pet;

                if (_selectedShelter is IChangeable changeable)
                {
                    if (changeable.RemovePet(petToRemove))
                    {
                        _shelterManager.SaveShelter(_selectedShelter);
                        _petsViewModel.Remove(viewModel);
                        
                        MessageBox.Show($"Питомец {petToRemove.Name} успешно удален!", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить питомца.", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void BtnSaveReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _reportCounter++;
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string fileName = $"Подборка_№{_reportCounter}_от_{timestamp}";

                var reportsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
                if (!Directory.Exists(reportsDir))
                {
                    Directory.CreateDirectory(reportsDir);
                }

                string filePath = Path.Combine(reportsDir, $"{fileName}.{_reportFormat.ToLower()}");

                var pets = _petsViewModel.Select(vm => vm.Pet).ToList();

                if (_reportFormat == "JSON")
                {
                    var jsonSerializer = new JsonSerializer();
                    jsonSerializer.SaveListToFile(pets, filePath);
                }
                else if (_reportFormat == "XML")
                {
                    var xmlSerializer = new XmlSerializer();
                    xmlSerializer.SaveListToFile(pets, filePath);
                }

                MessageBox.Show($"Отчет сохранен в файл:\n{filePath}", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении отчета: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class PetViewModel
    {
        public Pet Pet { get; }
        public string Name => Pet.Name;
        public int Age => Pet.Age;
        public double Weight => Pet.Weight;
        public string TypeName => Pet.GetType().Name;
        public string ClaustrophobiaText => Pet.HasClaustrophobia ? "Да" : "Нет";
        public string ShelterName { get; }
        public string AdditionalInfo { get; }

        public PetViewModel(Pet pet, string shelterName)
        {
            Pet = pet;
            ShelterName = shelterName;

            AdditionalInfo = pet switch
            {
                Cat cat => $"Шерсть: {cat.FurColor}",
                Dog dog => $"Порода: {dog.Breed}",
                Rabbit rabbit => $"Уши: {rabbit.EarLength:F1}см",
                _ => string.Empty
            };
        }
    }
}