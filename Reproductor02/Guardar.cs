using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Reproductor02
{
    public partial class Guardar : Form
    {
        string direccion = "";
        public Guardar()
        {
            InitializeComponent();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            if (bunifuTextbox1.text == "")
            {
                MessageBox.Show("Debe ingresar un nombre para la lista de reproducción");
            }
            else
            {
                Datos.name = bunifuTextbox1.text;
                Datos.imagen = direccion;
                this.Close();
            }
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog buscar = new OpenFileDialog();
            buscar.Filter = "*.bmp; *.gif;*.jpg;*.png|*.bmp;*.gif;*.jpg;*.png";
            buscar.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            buscar.Title = "Seleccionar la imagen para el contacto";
            buscar.RestoreDirectory = true;

            if (buscar.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(buscar.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                direccion = buscar.FileName;
            }
            else
            {
                direccion = "";
            }
        }

        private void Guardar_Load(object sender, EventArgs e)
        {
            direccion = "";
        }
    }
}
