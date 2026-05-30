using Model.Core;

namespace PetShelter;

public class AddPetForm : Form
{
    private readonly Shelter _shelter;
    public Pet? NewPet { get; private set; }

    // Controls
    private readonly TextBox _txtName = new();
    private readonly NumericUpDown _numAge = new();
    private readonly NumericUpDown _numWeight = new();
    private readonly ComboBox _cboType = new();
    private readonly CheckBox _chkClaustrophobia = new();
    private readonly Button _btnOk = new();
    private readonly Button _btnCancel = new();
    private readonly GroupBox _grpAdditional = new();

    // Additional controls (dynamically shown based on type)
    private readonly TextBox _txtFurColor = new();
    private readonly CheckBox _chkNeutered = new();
    private readonly TextBox _txtBreed = new();
    private readonly CheckBox _chkTrained = new();
    private readonly NumericUpDown _numEarLength = new();
    private readonly ComboBox _cboFurType = new();

    private readonly Label _lblName = new();
    private readonly Label _lblAge = new();
    private readonly Label _lblWeight = new();
    private readonly Label _lblType = new();
    private readonly Label _lblAdditional = new();

    public AddPetForm(Shelter shelter)
    {
        _shelter = shelter;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "Добавить питомца";
        Size = new Size(450, 500);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        // Name
        _lblName.Text = "Кличка:";
        _lblName.Location = new Point(20, 20);
        _lblName.Size = new Size(120, 23);

        _txtName.Location = new Point(160, 17);
        _txtName.Size = new Size(250, 23);

        // Age
        _lblAge.Text = "Возраст (лет):";
        _lblAge.Location = new Point(20, 55);
        _lblAge.Size = new Size(120, 23);

        _numAge.Location = new Point(160, 52);
        _numAge.Size = new Size(100, 23);
        _numAge.Minimum = 0;
        _numAge.Maximum = 50;
        _numAge.Value = 1;

        // Weight
        _lblWeight.Text = "Вес (кг):";
        _lblWeight.Location = new Point(20, 90);
        _lblWeight.Size = new Size(120, 23);

        _numWeight.Location = new Point(160, 87);
        _numWeight.Size = new Size(100, 23);
        _numWeight.Minimum = 0.1m;
        _numWeight.Maximum = 200m;
        _numWeight.DecimalPlaces = 1;
        _numWeight.Value = 5m;

        // Type
        _lblType.Text = "Вид животного:";
        _lblType.Location = new Point(20, 125);
        _lblType.Size = new Size(120, 23);

        _cboType.Location = new Point(160, 122);
        _cboType.Size = new Size(250, 23);
        _cboType.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboType.Items.Add("Cat");
        _cboType.Items.Add("Dog");
        _cboType.Items.Add("Rabbit");
        _cboType.SelectedIndex = 0;
        _cboType.SelectedIndexChanged += CboType_SelectedIndexChanged;

        // Claustrophobia
        _chkClaustrophobia.Text = "Клаустрофобия";
        _chkClaustrophobia.Location = new Point(20, 165);
        _chkClaustrophobia.Size = new Size(200, 24);

        // Additional info group
        _grpAdditional.Location = new Point(20, 200);
        _grpAdditional.Size = new Size(390, 180);
        _grpAdditional.Text = "Дополнительно";

        _lblAdditional.Location = new Point(10, 25);
        _lblAdditional.Size = new Size(370, 150);

        // Setup additional controls
        SetupAdditionalControls();

        // OK button
        _btnOk.Text = "Добавить";
        _btnOk.Location = new Point(100, 400);
        _btnOk.Size = new Size(100, 35);
        _btnOk.Click += BtnOk_Click;

        // Cancel button
        _btnCancel.Text = "Отмена";
        _btnCancel.Location = new Point(220, 400);
        _btnCancel.Size = new Size(100, 35);
        _btnCancel.Click += BtnCancel_Click;

        // Add controls
        Controls.AddRange(new Control[] {
            _lblName, _txtName,
            _lblAge, _numAge,
            _lblWeight, _numWeight,
            _lblType, _cboType,
            _chkClaustrophobia,
            _grpAdditional,
            _btnOk, _btnCancel
        });

        _grpAdditional.Controls.Add(_lblAdditional);
        UpdateAdditionalControls();
    }

    private void SetupAdditionalControls()
    {
        // Cat controls
        _txtFurColor.Location = new Point(10, 50);
        _txtFurColor.Size = new Size(150, 23);
        _txtFurColor.PlaceholderText = "Цвет шерсти";

        _chkNeutered.Text = "Стерилизован/а";
        _chkNeutered.Location = new Point(10, 80);
        _chkNeutered.Size = new Size(150, 24);

        // Dog controls
        _txtBreed.Location = new Point(10, 50);
        _txtBreed.Size = new Size(150, 23);
        _txtBreed.PlaceholderText = "Порода";

        _chkTrained.Text = "Дрессирован";
        _chkTrained.Location = new Point(10, 80);
        _chkTrained.Size = new Size(150, 24);

        // Rabbit controls
        _numEarLength.Location = new Point(10, 50);
        _numEarLength.Size = new Size(100, 23);
        _numEarLength.Minimum = 1m;
        _numEarLength.Maximum = 30m;
        _numEarLength.DecimalPlaces = 1;
        _numEarLength.Value = 10m;

        var lblEarLength = new Label
        {
            Text = "Длина ушей (см):",
            Location = new Point(120, 53),
            Size = new Size(100, 23)
        };

        _cboFurType.Location = new Point(10, 80);
        _cboFurType.Size = new Size(150, 23);
        _cboFurType.DropDownStyle = ComboBoxStyle.DropDownList;
        _cboFurType.Items.Add("Short");
        _cboFurType.Items.Add("Long");
        _cboFurType.Items.Add("Rex");
        _cboFurType.SelectedIndex = 0;

        var lblFurType = new Label
        {
            Text = "Тип шерсти:",
            Location = new Point(170, 83),
            Size = new Size(80, 23)
        };

        // Add all possible controls (they'll be shown/hidden as needed)
        _grpAdditional.Controls.AddRange(new Control[] {
            _txtFurColor, _chkNeutered,
            _txtBreed, _chkTrained,
            _numEarLength, lblEarLength,
            _cboFurType, lblFurType
        });
    }

    private void CboType_SelectedIndexChanged(object? sender, EventArgs e)
    {
        UpdateAdditionalControls();
    }

    private void UpdateAdditionalControls()
    {
        // Hide all additional controls first
        _txtFurColor.Visible = false;
        _chkNeutered.Visible = false;
        _txtBreed.Visible = false;
        _chkTrained.Visible = false;
        _numEarLength.Visible = false;
        _cboFurType.Visible = false;

        // Hide labels for rabbit
        foreach (Control ctrl in _grpAdditional.Controls)
        {
            if (ctrl is Label lbl && (lbl.Text == "Длина ушей (см):" || lbl.Text == "Тип шерсти:"))
            {
                lbl.Visible = false;
            }
        }

        string selectedType = _cboType.SelectedItem?.ToString() ?? "Cat";

        switch (selectedType)
        {
            case "Cat":
                _txtFurColor.Visible = true;
                _chkNeutered.Visible = true;
                _lblAdditional.Text = "Для кошек укажите цвет шерсти и статус стерилизации.";
                break;

            case "Dog":
                _txtBreed.Visible = true;
                _chkTrained.Visible = true;
                _lblAdditional.Text = "Для собак укажите породу и статус дрессировки.";
                break;

            case "Rabbit":
                _numEarLength.Visible = true;
                _cboFurType.Visible = true;
                foreach (Control ctrl in _grpAdditional.Controls)
                {
                    if (ctrl is Label label && label.Text == "Длина ушей (см):")
                        label.Visible = true;
                    if (ctrl is Label label2 && label2.Text == "Тип шерсти:")
                        label2.Visible = true;
                }
                _lblAdditional.Text = "Для кроликов укажите длину ушей и тип шерсти.";
                break;
        }
    }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        try
        {
            string name = _txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите кличку питомца.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int age = (int)_numAge.Value;
            double weight = (double)_numWeight.Value;
            bool hasClaustrophobia = _chkClaustrophobia.Checked;
            string selectedType = _cboType.SelectedItem?.ToString() ?? "Cat";

            // Create the appropriate pet type
            NewPet = selectedType switch
            {
                "Cat" => CreateCat(weight, hasClaustrophobia),
                "Dog" => CreateDog(weight, hasClaustrophobia),
                "Rabbit" => CreateRabbit(weight, hasClaustrophobia),
                _ => null
            };

            if (NewPet == null)
            {
                MessageBox.Show("Ошибка при создании питомца.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private Cat CreateCat(double weight, bool hasClaustrophobia)
    {
        return new Cat(
            _txtName.Text.Trim(),
            (int)_numAge.Value,
            weight,
            _txtFurColor.Text.Trim(),
            _chkNeutered.Checked,
            hasClaustrophobia
        );
    }

    private Dog CreateDog(double weight, bool hasClaustrophobia)
    {
        return new Dog(
            _txtName.Text.Trim(),
            (int)_numAge.Value,
            weight,
            _txtBreed.Text.Trim(),
            _chkTrained.Checked,
            hasClaustrophobia
        );
    }

    private Rabbit CreateRabbit(double weight, bool hasClaustrophobia)
    {
        return new Rabbit(
            _txtName.Text.Trim(),
            (int)_numAge.Value,
            weight,
            (double)_numEarLength.Value,
            _cboFurType.SelectedItem?.ToString() ?? "Short",
            hasClaustrophobia
        );
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        NewPet = null;
        DialogResult = DialogResult.Cancel;
        Close();
    }
}