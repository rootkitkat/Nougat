using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace UnRealProject
{
    //подписать поля на событие Leave и вызвать AutoSave
    //error provider
    public partial class Form1 : Form
    {
        List<Student> _students = new List<Student>();
        Student _currentStudent; //студент данные которого сейчас отображаются на экране
        int _currentIndex = 0; 

        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSaveFolder_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtBxPathSave.Text = saveFileDialog.FileName;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateObject();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
         saveFileDialog.Filter = "Text files(*.txt)|*.txt|XML files(*.xml)|*.xml|Data files(*.dat)|*.dat|Bin files(*.bin)|*.bin";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileManager fileManager = new FileManager(_students, saveFileDialog.FileName);
                fileManager.SaveAll();
                //fileManager.SaveAll();

                //FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create);

                //BinaryFormatter bf = new BinaryFormatter();

                //bf.Serialize(fs, _students);

                //fs.Close();
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.txt)|*.txt|XML files(*.xml)|*.xml|Data files(*.dat)|*.dat|Bin files(*.bin)|*.bin|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open);

                //BinaryFormatter bf = new BinaryFormatter();

                //_students = (List<Student>)bf.Deserialize(fs);

                //fs.Close();

                FileManager fileManager = new FileManager(_students, openFileDialog.FileName);
                fileManager.Load();

                _currentStudent = _students[0];

                SetEnabled(true);
                InitForm();

                txtBxPathSave.Text = openFileDialog.FileName;
            }
        }

        //перенос данных из компонентов в объект
        private void UpdateObject()
        {
            if (_currentStudent != null)
            {
                _currentStudent.Otch = txtBxOtch.Text;
                _currentStudent.Family = txtBxFam.Text;
                _currentStudent.Name = txtBxName.Text;
                _currentStudent.Course = cmBxCourse.SelectedItem != null ? cmBxCourse.SelectedItem.ToString() : "";
                _currentStudent.Faculty = cmBxFaculty.SelectedItem != null ? cmBxFaculty.SelectedItem.ToString() : "";
                _currentStudent.Group = (int)numericUpDownGroup.Value;

                if (_currentStudent.Dict.ContainsKey("Математика"))
                {
                    _currentStudent.Dict["Математика"] = int.Parse(txtBxMath.Text);
                }
                else
                {
                    _currentStudent.Dict.Add("Математика", int.Parse(txtBxMath.Text));
                }

                if (_currentStudent.Dict.ContainsKey("ООП"))
                {
                    _currentStudent.Dict["ООП"] = int.Parse(txtBxOOP.Text);
                }
                else
                {
                    _currentStudent.Dict.Add("ООП", int.Parse(txtBxOOP.Text));
                }

                if (_currentStudent.Dict.ContainsKey("Физика"))
                {
                    _currentStudent.Dict["Физика"] = int.Parse(txtBxPhisics.Text);
                }
                else
                {
                    _currentStudent.Dict.Add("Физика", int.Parse(txtBxPhisics.Text));
                }
            }
        }

        private void InitCounter()
        {
            string text = string.Format("{0}/{1}", _currentIndex + 1, _students.Count);

            label11.Text = text;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            UpdateObject();

            if (_currentIndex != _students.Count - 1)
            {
                _currentIndex++;

                _currentStudent = _students[_currentIndex];

                InitForm();
            }
            else
            {
                if (chckBxRepeat.Checked)
                {
                    _currentIndex = 0;

                    _currentStudent = _students[_currentIndex];

                    InitForm();
                }
            }
        }

        private void SetEnabled(bool enabled)
        {
            foreach (Control control in Controls)
            {
                control.Enabled = enabled;
            }
        }

        //перенос данных на форму
        private void InitForm()
        {
            txtBxName.Text = _currentStudent.Name;
            txtBxFam.Text = _currentStudent.Family;
            txtBxOtch.Text = _currentStudent.Otch;
            txtBxMath.Text = _currentStudent.Dict.ContainsKey("Математика") ? _currentStudent.Dict["Математика"].ToString() : "";
            txtBxOOP.Text = _currentStudent.Dict.ContainsKey("ООП") ? _currentStudent.Dict["ООП"].ToString() : "";
            txtBxPhisics.Text = _currentStudent.Dict.ContainsKey("Физика") ? _currentStudent.Dict["Физика"].ToString() : "";
            cmBxCourse.SelectedItem = _currentStudent.Course;
            cmBxFaculty.SelectedItem = _currentStudent.Faculty;
            numericUpDownGroup.Value = _currentStudent.Group;

            InitCounter();
        }

        private void btnSaveObject_Click(object sender, EventArgs e)
        {
            UpdateObject();
        }

        private void CreateNew()
        {
            _currentStudent = new Student();
            _students.Add(_currentStudent);

            _currentIndex = _students.Count - 1;

            InitForm();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            SetEnabled(true);

            UpdateObject();

            CreateNew();
        }

        private void btnPred_Click(object sender, EventArgs e)
        {
            UpdateObject();

            if (_currentIndex != 0)
            {
                _currentIndex--;

                _currentStudent = _students[_currentIndex];

                InitForm();
            }
            else
            {
                if (chckBxRepeat.Checked)
                {
                    _currentIndex = _students.Count - 1;

                    _currentStudent = _students[_currentIndex];

                    InitForm();
                }
            }
        }

        private void chckBxAutoSave_CheckedChanged(object sender, EventArgs e)
        {
            if (chckBxAutoSave.Checked && txtBxPathSave.Text == "")
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtBxPathSave.Text = openFileDialog.FileName;
                }
                else
                {
                    chckBxAutoSave.Checked = false;
                }
            }
        }

        private void AutoSave()
        {
            if (chckBxAutoSave.Checked)
            {
                UpdateObject();

                FileManager fileManager = new FileManager(_students, txtBxPathSave.Text);
                fileManager.SaveAll();
            }
        }

        private void txtBxFam_Leave(object sender, EventArgs e)
        {
            // errorProvider1.Icon = Properties.Resources.
            if (string.IsNullOrEmpty(txtBxFam.Text))
            {
                errorProvider1.SetError(txtBxFam, "Это поле должно быть заполнено");
            }

            else
            {
                errorProvider1.SetError(txtBxFam, null);
            }

            AutoSave();
        }

        private void txtBxName_Leave(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtBxName.Text))
            {
                errorProvider2.SetError(txtBxName, "Это поле должно быть заполнено");
            }

            else
            {
                errorProvider2.SetError(txtBxName, null);
            }

            AutoSave();
        }

        private void txtBxOtch_Leave(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtBxOtch.Text))
            {
                errorProvider3.SetError(txtBxOtch, "Это поле должно быть заполнено");
            }

            else
            {
                errorProvider3.SetError(txtBxOtch, null);
            }

            AutoSave();
        }

        private void cmBxCourse_Leave(object sender, EventArgs e)
        {
            AutoSave();
        }

        private void cmBxFaculty_Leave(object sender, EventArgs e)
        {
            AutoSave();
        }

        private void numericUpDownGroup_Leave(object sender, EventArgs e)
        {
            AutoSave();
        }

        private void groupBox1_Leave(object sender, EventArgs e)
        {
            AutoSave();
        }

        private void errorProvider1_RightToLeftChanged(object sender, EventArgs e)
        {

        }
    }
}
