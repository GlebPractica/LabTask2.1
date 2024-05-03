using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Task1
{
    public partial class Form1 : Form
    {
        private List<Discipline> disciplines;
        private Lecturer lecturer;
        private Discipline discipline;

        public Form1()
        {
            InitializeComponent();
            disciplines = new List<Discipline>();
            lecturer = new Lecturer();
        }

        private void ClearTxtBoxes()
        {
            textBox1.Text = "";
            richTextBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            trackBar1.Value = 1;
        }

        private void FillListBox(List<Discipline> disciplines)
        {
            listBox1.Items.Clear();

            foreach (var disc in disciplines)
            {
                listBox1.Items.Add($"Название: {disc.Name}");
                listBox1.Items.Add($"Семестр: {disc.Semester}");
                listBox1.Items.Add($"Курс: {disc.Course}");
                listBox1.Items.Add($"Специальность: {disc.Speciality}");
                listBox1.Items.Add($"Количество лекций: {disc.LectureCount}");
                listBox1.Items.Add($"Количество лабораторных: {disc.LabCount}");
                listBox1.Items.Add($"Тип контроля: {disc.ControlType}");
                listBox1.Items.Add("Лектор:");
                listBox1.Items.Add($"ФИО: {disc.Lecture.FullName}");
                listBox1.Items.Add($"Кафедра: {disc.Lecture.Department}");
                listBox1.Items.Add($"Аудитория: {disc.Lecture.Room}");
                listBox1.Items.Add("\n");
            }
        }

        private string GetFromRadioButton()
        {
            if (radioButton1.Checked)
                return radioButton1.Text;
            else if (radioButton2.Checked)
                return radioButton2.Text;
            else if (radioButton3.Checked)
                return radioButton3.Text;
            else if (radioButton4.Checked)
                return radioButton4.Text;

            return radioButton1.Text;
        }

        private void BttnAddLector_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Вы не ввели ФИО лектора", "Ошибка");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Вы не ввели кафедру лектора", "Ошибка");
                return;
            }

            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Вы не ввели аудитория лектора", "Ошибка");
                return;
            }

            string name = textBox2.Text;
            string depart = textBox3.Text;
            string room = textBox4.Text;

            lecturer.FullName = name;
            lecturer.Department = depart;
            lecturer.Room = room;

            MessageBox.Show($"Добавлен лектор:\nФИО: {lecturer.FullName}\nКафедра: {lecturer.Department}\nАудитория: {lecturer.Room}", "Результат");
        }

        private void BttnAddOtdel_Click(object sender, EventArgs e)
        {
            string nameDisc;
            int semest;
            int cours;
            string spec;
            int countLabs;
            int countLect;
            string controlType;

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Вы не ввели название дисциплины", "Ошибка");
                return;
            }

            if (!int.TryParse(richTextBox1.Text, out countLabs) && countLabs > 0)
            {
                MessageBox.Show("Вы ввели неверное число");
                return;
            }

            if (lecturer == null)
            {
                MessageBox.Show("Вы не добавили лектора.", "Ошибка");
                return;
            }

            nameDisc = textBox1.Text;
            semest = Convert.ToInt32(numericUpDown1.Value);
            cours = Convert.ToInt32(comboBox1.SelectedItem);
            spec = GetFromRadioButton();
            countLect = trackBar1.Value;
            controlType = comboBox2.Text;

            discipline = new Discipline() { 
                Name = nameDisc, 
                Semester = semest, 
                Course = cours, 
                Speciality = spec, 
                LabCount = countLabs, 
                LectureCount = countLect, 
                Lecture = lecturer 
            };

            disciplines.Add(discipline);

            FillListBox(disciplines);
            ClearTxtBoxes();
        }

        private void BttnSaveJSON_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Filter = "JSON File (*.json)|*.json";

                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;

                string filePath = saveFileDialog1.FileName;

                string json = JsonConvert.SerializeObject(disciplines);
                File.WriteAllText(filePath, json);

                MessageBox.Show($"Успешно сохранено\nПуть: {filePath}", "Результат");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void BttnLoadJSON_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "JSON File (*.json)|*.json";

                if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;

                string filePath = openFileDialog1.FileName;

                string json = File.ReadAllText(filePath);
                disciplines = JsonConvert.DeserializeObject<List<Discipline>>(json);
                FillListBox(disciplines);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при чтении из JSON файла: " + ex.Message, "Ошибка");
            }
        }

        private void BttnSaveXML_Click(object sender, EventArgs e)
        {

            try
            {
                saveFileDialog1.Filter = "XML File (*.xml)|*.xml";

                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;

                string filePath = saveFileDialog1.FileName;

                var serializer = new XmlSerializer(typeof(List<Discipline>));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    serializer.Serialize(stream, disciplines);
                }

                MessageBox.Show($"Успешно сохранено\nПуть: {filePath}", "Результат");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void BttnXMLLoad_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "XML File (*.xml)|*.xml";

                if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;

                string filePath = openFileDialog1.FileName;

                XmlSerializer serializer = new XmlSerializer(typeof(List<Discipline>));
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    disciplines = (List<Discipline>)serializer.Deserialize(fs);
                }

                FillListBox(disciplines);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при чтении из XML файла: " + ex.Message, "Ошибка");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }
    }
}
