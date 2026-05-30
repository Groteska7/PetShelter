using System;
using System.Windows;
using System.Windows.Controls;
using Model.Core;

namespace PetShelter
{
    public partial class AddPetWindow : Window
    {
        private readonly Shelter _shelter;
        public Pet? NewPet { get; private set; }

        public AddPetWindow(Shelter shelter)
        {
            _shelter = shelter;
            InitializeComponent();
        }

        private void CboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAdditionalFields();
        }

        private void UpdateAdditionalFields()
        {
            if (CboType.SelectedItem is ComboBoxItem item)
            {
                string type = item.Content?.ToString() ?? "Cat";

                CatFields.Visibility = type == "Cat" ? Visibility.Visible : Visibility.Collapsed;
                DogFields.Visibility = type == "Dog" ? Visibility.Visible : Visibility.Collapsed;
                RabbitFields.Visibility = type == "Rabbit" ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = TxtName.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Введите кличку питомца.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(TxtAge.Text, out int age) || age < 0 || age > 50)
                {
                    MessageBox.Show("Введите корректный возраст (0-50 лет).", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!double.TryParse(TxtWeight.Text, out double weight) || weight < 0.1 || weight > 200)
                {
                    MessageBox.Show("Введите корректный вес (0.1-200 кг).", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                bool hasClaustrophobia = ChkClaustrophobia.IsChecked ?? false;

                string selectedType = "Cat";
                if (CboType.SelectedItem is ComboBoxItem item)
                {
                    selectedType = item.Content?.ToString() ?? "Cat";
                }

                NewPet = selectedType switch
                {
                    "Cat" => new Cat(name, age, weight, TxtFurColor.Text.Trim(), ChkNeutered.IsChecked ?? false, hasClaustrophobia),
                    "Dog" => new Dog(name, age, weight, TxtBreed.Text.Trim(), ChkTrained.IsChecked ?? false, hasClaustrophobia),
                    "Rabbit" => CreateRabbit(name, age, weight, hasClaustrophobia),
                    _ => null
                };

                if (NewPet == null)
                {
                    MessageBox.Show("Ошибка при создании питомца.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Rabbit CreateRabbit(string name, int age, double weight, bool hasClaustrophobia)
        {
            double earLength = 10;
            if (!double.TryParse(TxtEarLength.Text, out earLength) || earLength < 1 || earLength > 30)
            {
                earLength = 10;
            }

            string furType = "Short";
            if (CboFurType.SelectedItem is ComboBoxItem item)
            {
                furType = item.Content?.ToString() ?? "Short";
            }

            return new Rabbit(name, age, weight, earLength, furType, hasClaustrophobia);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NewPet = null;
            DialogResult = false;
            Close();
        }
    }
}