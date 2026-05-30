using Model.Core;

namespace PetShelter;

public class MainForm : Form
{
    private readonly ShelterManager _shelterManager;
    private string? _currentFormat = "JSON";

    // Controls
    private readonly ComboBox _cboShelter = new();
    private readonly ComboBox _cboPetType = new();
    private readonly CheckBox _chkOpenAreaOnly = new();
    private readonly ComboBox _cboFormat = new();
    private readonly Button _btnShowPets = new();
    private readonly Label _lblShelter = new();
    private readonly Label _lblPetType = new();
    private readonly Label _lblFormat = new();
    private readonly GroupBox _grpFilters = new();
    private readonly GroupBox _grpReport = new();

    public MainForm()
    {
        _shelterManager = new ShelterManager();
        _shelterManager.LoadOrCreateShelters();

        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "PetShelter - Приют для животных";
        Size = new Size(500, 400);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        // GroupBox for filters
        _grpFilters.Location = new Point(20, 20);
        _grpFilters.Size = new Size(440, 200);
        _grpFilters.Text = "Фильтры";
        _grpFilters.Font = new Font(_grpFilters.Font, FontStyle.Bold);

        // Shelter label and combobox
        _lblShelter.Text = "Приют:";
        _lblShelter.Location = new Point(20, 30);
        _lblShelter.Size = new Size(100, 23);
        _lblShelter.Font = new Font(_lblShelter.Font, FontStyle.Regular);

        _cboShelter.Location = new Point(150, 27);
        _cboShelter.Size = new Size(250, 23);
        _cboShelter.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboShelter.Items.Add("(Все приюты)");
        foreach (var shelter in _shelterManager.Shelters)
        {
            _cboShelter.Items.Add(shelter.Name);
        }
        _cboShelter.SelectedIndex = 0;

        // Pet type label and combobox
        _lblPetType.Text = "Вид животного:";
        _lblPetType.Location = new Point(20, 70);
        _lblPetType.Size = new Size(120, 23);
        _lblPetType.Font = new Font(_lblPetType.Font, FontStyle.Regular);

        _cboPetType.Location = new Point(150, 67);
        _cboPetType.Size = new Size(250, 23);
        _cboPetType.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboPetType.Items.Add("(Все виды)");
        _cboPetType.Items.Add("Cat");
        _cboPetType.Items.Add("Dog");
        _cboPetType.Items.Add("Rabbit");
        _cboPetType.SelectedIndex = 0;

        // Open area only checkbox
        _chkOpenAreaOnly.Text = "Только приюты с открытой территорией";
        _chkOpenAreaOnly.Location = new Point(20, 110);
        _chkOpenAreaOnly.Size = new Size(380, 24);
        _chkOpenAreaOnly.Font = new Font(_chkOpenAreaOnly.Font, FontStyle.Regular);

        // GroupBox for report format
        _grpReport.Location = new Point(20, 230);
        _grpReport.Size = new Size(440, 70);
        _grpReport.Text = "Формат отчета";
        _grpReport.Font = new Font(_grpReport.Font, FontStyle.Bold);

        _lblFormat.Text = "Формат:";
        _lblFormat.Location = new Point(20, 30);
        _lblFormat.Size = new Size(100, 23);
        _lblFormat.Font = new Font(_lblFormat.Font, FontStyle.Regular);

        _cboFormat.Location = new Point(150, 27);
        _cboFormat.Size = new Size(250, 23);
        _cboFormat.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboFormat.Items.Add("JSON");
        _cboFormat.Items.Add("XML");
        _cboFormat.SelectedIndex = 0;
        _cboFormat.SelectedIndexChanged += CboFormat_SelectedIndexChanged;

        // Show pets button
        _btnShowPets.Text = "Показать питомцев";
        _btnShowPets.Location = new Point(150, 320);
        _btnShowPets.Size = new Size(180, 40);
        _btnShowPets.Font = new Font(_btnShowPets.Font, FontStyle.Bold);
        _btnShowPets.BackColor = Color.FromArgb(0, 120, 215);
        _btnShowPets.ForeColor = Color.White;
        _btnShowPets.FlatStyle = FlatStyle.Flat;
        _btnShowPets.Click += BtnShowPets_Click;

        // Add controls to form
        _grpFilters.Controls.AddRange(new Control[] {
            _lblShelter, _cboShelter,
            _lblPetType, _cboPetType,
            _chkOpenAreaOnly
        });

        _grpReport.Controls.AddRange(new Control[] {
            _lblFormat, _cboFormat
        });

        Controls.AddRange(new Control[] {
            _grpFilters, _grpReport, _btnShowPets
        });
    }

    private void CboFormat_SelectedIndexChanged(object? sender, EventArgs e)
    {
        _currentFormat = _cboFormat.SelectedItem?.ToString();

        if (!string.IsNullOrEmpty(_currentFormat))
        {
            ConvertReportsToFormat(_currentFormat);
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
                // Read the source file and create a copy with the new extension
                string content = File.ReadAllText(file);
                string newFileName = Path.GetFileNameWithoutExtension(file) + "." + targetFormat.ToLower();
                string newFilePath = Path.Combine(reportsDir, newFileName);
                File.WriteAllText(newFilePath, content);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при конвертации файла: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    private void BtnShowPets_Click(object? sender, EventArgs e)
    {
        try
        {
            // Get selected values
            string? shelterName = _cboShelter.SelectedItem?.ToString();
            if (shelterName == "(Все приюты)") shelterName = null;

            string? petTypeName = _cboPetType.SelectedItem?.ToString();
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

            bool openAreaOnly = _chkOpenAreaOnly.Checked;

            // Get filtered pets
            var filteredPets = _shelterManager.GetFilteredPets(shelterName, petType, openAreaOnly);

            // Get the selected shelter (for add/remove functionality)
            Shelter? selectedShelter = null;
            if (!string.IsNullOrEmpty(shelterName))
            {
                selectedShelter = _shelterManager.GetShelterByName(shelterName);
            }

            // Open the pets window
            var petsForm = new PetsForm(filteredPets, selectedShelter, _shelterManager, _currentFormat ?? "JSON");
            petsForm.ShowDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}