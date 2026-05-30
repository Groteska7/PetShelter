using Model.Core;
using Model.Data;

namespace PetShelter;

public class PetsForm : Form
{
    private readonly List<Pet> _pets;
    private readonly Shelter? _selectedShelter;
    private readonly ShelterManager _shelterManager;
    private readonly string _reportFormat;
    private int _reportCounter = 0;

    // Controls
    private readonly DataGridView _dataGridView = new();
    private readonly Button _btnAddPet = new();
    private readonly Button _btnRemovePet = new();
    private readonly Button _btnSaveReport = new();
    private readonly Button _btnClose = new();
    private readonly Label _lblInfo = new();

    public PetsForm(List<Pet> pets, Shelter? selectedShelter, ShelterManager shelterManager, string reportFormat)
    {
        _pets = pets;
        _selectedShelter = selectedShelter;
        _shelterManager = shelterManager;
        _reportFormat = reportFormat;

        InitializeComponent();
        PopulateGrid();
    }

    private void InitializeComponent()
    {
        Text = "PetShelter - Список питомцев";
        Size = new Size(900, 600);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.Sizable;

        // Info label
        _lblInfo.Text = _selectedShelter != null
            ? $"Приют: {_selectedShelter.Name}"
            : "Все приюты";
        _lblInfo.Location = new Point(20, 15);
        _lblInfo.Size = new Size(850, 23);
        _lblInfo.Font = new Font(_lblInfo.Font, FontStyle.Bold);

        // DataGridView for pets
        _dataGridView.Location = new Point(20, 50);
        _dataGridView.Size = new Size(850, 420);
        _dataGridView.AutoGenerateColumns = false;
        _dataGridView.ReadOnly = true;
        _dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _dataGridView.MultiSelect = false;
        _dataGridView.AllowUserToAddRows = false;
        _dataGridView.BackgroundColor = Color.White;

        // Setup columns
        SetupDataGridColumns();

        // Add Pet button
        _btnAddPet.Text = "Добавить питомца";
        _btnAddPet.Location = new Point(20, 485);
        _btnAddPet.Size = new Size(150, 35);
        _btnAddPet.Enabled = _selectedShelter != null;
        _btnAddPet.Click += BtnAddPet_Click;

        // Remove Pet button
        _btnRemovePet.Text = "Удалить питомца";
        _btnRemovePet.Location = new Point(190, 485);
        _btnRemovePet.Size = new Size(150, 35);
        _btnRemovePet.Enabled = _selectedShelter != null;
        _btnRemovePet.Click += BtnRemovePet_Click;

        // Save Report button
        _btnSaveReport.Text = "Сохранить отчет";
        _btnSaveReport.Location = new Point(360, 485);
        _btnSaveReport.Size = new Size(150, 35);
        _btnSaveReport.Click += BtnSaveReport_Click;

        // Close button
        _btnClose.Text = "Закрыть";
        _btnClose.Location = new Point(530, 485);
        _btnClose.Size = new Size(150, 35);
        _btnClose.Click += BtnClose_Click;

        // Add controls
        Controls.AddRange(new Control[] {
            _lblInfo, _dataGridView, _btnAddPet, _btnRemovePet, _btnSaveReport, _btnClose
        });
    }

    private void SetupDataGridColumns()
    {
        _dataGridView.Columns.Clear();

        _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Кличка",
            Name = "colName",
            DataPropertyName = "Name",
            Width = 120
        });

        _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Возраст",
            Name = "colAge",
            DataPropertyName = "Age",
            Width = 70
        });

        _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Вес (кг)",
            Name = "colWeight",
            DataPropertyName = "Weight",
            Width = 80
        });

        _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Вид",
            Name = "colType",
            Width = 80
        });

        _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Клаустрофобия",
            Name = "colClaustrophobia",
            Width = 120
        });

        _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Приют",
            Name = "colShelter",
            Width = 150
        });

        // Additional info column based on pet type
        _dataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Доп. информация",
            Name = "colAdditional",
            Width = 150
        });
    }

    private void PopulateGrid()
    {
        _dataGridView.Rows.Clear();

        foreach (var pet in _pets)
        {
            // Cast to base class Pet and use polymorphism
            var basePet = (Pet)pet;

            // Get shelter name using ShelterManager
            var shelter = _shelterManager.FindShelterForPet(basePet);
            string shelterName = shelter?.Name ?? "Неизвестно";

            // Get additional info based on pet type using interface casting
            string additionalInfo = GetAdditionalInfo(pet);

            _dataGridView.Rows.Add(
                basePet.Name,
                basePet.Age,
                basePet.Weight.ToString("F1"),
                pet.GetType().Name,
                basePet.HasClaustrophobia ? "Да" : "Нет",
                shelterName,
                additionalInfo
            );
        }
    }

    private string GetAdditionalInfo(Pet pet)
    {
        // Use pattern matching for type checking
        return pet switch
        {
            Cat cat => $"Шерсть: {cat.FurColor}",
            Dog dog => $"Порода: {dog.Breed}",
            Rabbit rabbit => $"Уши: {rabbit.EarLength:F1}см",
            _ => string.Empty
        };
    }

    private void BtnAddPet_Click(object? sender, EventArgs e)
    {
        if (_selectedShelter == null)
        {
            MessageBox.Show("Выберите приют для добавления питомца.", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var addForm = new AddPetForm(_selectedShelter);
        if (addForm.ShowDialog() == DialogResult.OK)
        {
            var newPet = addForm.NewPet;
            if (newPet != null)
            {
                // Cast to IChangeable interface to add pet
                if (_selectedShelter is IChangeable changeable)
                {
                    if (changeable.AddPet(newPet))
                    {
                        _shelterManager.SaveShelter(_selectedShelter);
                        _pets.Add(newPet);
                        PopulateGrid();
                        MessageBox.Show($"Питомец {newPet.Name} успешно добавлен!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось добавить питомца. Проверьте параметры приюта.", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }

    private void BtnRemovePet_Click(object? sender, EventArgs e)
    {
        if (_selectedShelter == null || _dataGridView.SelectedRows.Count == 0)
        {
            MessageBox.Show("Выберите питомца для удаления.", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var result = MessageBox.Show("Вы уверены, что хотите удалить выбранного питомца?",
            "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            int selectedIndex = _dataGridView.SelectedRows[0].Index;
            if (selectedIndex >= 0 && selectedIndex < _pets.Count)
            {
                var petToRemove = _pets[selectedIndex];

                // Cast to IChangeable interface to remove pet
                if (_selectedShelter is IChangeable changeable)
                {
                    if (changeable.RemovePet(petToRemove))
                    {
                        _shelterManager.SaveShelter(_selectedShelter);
                        _pets.RemoveAt(selectedIndex);
                        PopulateGrid();
                        MessageBox.Show($"Питомец {petToRemove.Name} успешно удален!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить питомца.", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }

    private void BtnSaveReport_Click(object? sender, EventArgs e)
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

            // Use the appropriate serializer based on format
            if (_reportFormat == "JSON")
            {
                var jsonSerializer = new JsonSerializer();
                jsonSerializer.SaveListToFile(_pets, filePath);
            }
            else if (_reportFormat == "XML")
            {
                var xmlSerializer = new XmlSerializer();
                xmlSerializer.SaveListToFile(_pets, filePath);
            }

            MessageBox.Show($"Отчет сохранен в файл:\n{filePath}", "Успех",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при сохранении отчета: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }
}