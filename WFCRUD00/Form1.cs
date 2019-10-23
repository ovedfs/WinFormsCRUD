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

        private void ClearForm()
        {
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
                txtFirstName.Text = row.Cells[0].Value.ToString();
            }
        }
    }
}
