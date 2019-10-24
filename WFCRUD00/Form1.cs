using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFCRUD00
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(txtFirstName.Text) || String.IsNullOrEmpty(txtLastName.Text) || String.IsNullOrEmpty(txtEmail.Text) || String.IsNullOrEmpty(txtPhone.Text))
            {
                MessageBox.Show("Todos los datos son obligatorios", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (! String.IsNullOrEmpty(lblID.Text))
            {
                UpdatePerson();
            }
            else
            {
                InsertPerson();
            }
        }

        private void InsertPerson()
        {
            Person person = new Person(txtFirstName.Text, txtLastName.Text, txtEmail.Text, txtPhone.Text);

            if (DbData.Insert(person) != -1)
            {
                MessageBox.Show("Registro agregado correctamente", "Nuevo registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshGrid();
                ClearForm();
                return;
            }

            MessageBox.Show("Hubo un error al registrar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void UpdatePerson()
        {
            Person person = new Person(txtFirstName.Text, txtLastName.Text, txtEmail.Text, txtPhone.Text);
            int id = Int32.Parse(lblID.Text);

            try
            {
                if (DbData.Update(person, id) != -1)
                {
                    MessageBox.Show("Registro actualizado correctamente", "Actualizar registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshGrid();
                    ClearForm();
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Hubo un error al actualizar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void ClearForm()
        {
            lblID.Text = String.Empty;
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
        }

        private void RefreshGrid()
        {
            dataGridView.DataSource = DbData.GetData();
        }

        private void dataGridView_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView.Rows[e.RowIndex];

                if (int.TryParse(row.Cells[0].Value.ToString(), out int id))
                {
                    Person person = DbData.Find(id);
                    FillForm(person);
                }
            }
        }

        private void FillForm(Person person)
        {
            lblID.Text = person.Id.ToString();
            txtFirstName.Text = person.FirstName;
            txtLastName.Text = person.LastName;
            txtEmail.Text = person.Email;
            txtPhone.Text = person.Phone;
        }
    }
}
