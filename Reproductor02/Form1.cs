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
using TagLib;

namespace Reproductor02
{
    public partial class Form1 : Form
    {
        string directorioMP3 = "";
        bool estado = true;
        Cancion song = new Cancion();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog abrir = new OpenFileDialog();
            abrir.FileName = "Seleccione una canción";
            abrir.Filter = "Archivo mp3|*.mp3|Archivo wav|*.wav|Archivo MP4|*.MP4|Todos los Archivos|*.*";
            abrir.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            abrir.FilterIndex = 1;
            if (abrir.ShowDialog() == DialogResult.OK)
            {
                directorioMP3 = abrir.FileName;
                TagLib.File tags = TagLib.File.Create(directorioMP3);

                //Envia datos a la funcion cargar listado
                AddSong(tags.Tag.Title, tags.Tag.JoinedPerformers, directorioMP3);
                //Llina listbox con las canciones seleccionadas
                listBox1.Items.Add(tags.Tag.Title);

                label7.Text = Convert.ToString(listBox1.Items.Count);
                if (listBox1.Items.Count == 1)
                {
                    song = Datos.media.Inicio;
                    axWindowsMediaPlayer1.URL = song.Directorio;
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                }
            }


        }
        public void AddSong(string name, string artist, string url)
        {
            Datos.media.NextSong(name, artist, url);
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            if (song == null || axWindowsMediaPlayer1.URL == "")
            {
                MessageBox.Show("No ha ingresado una canción en la lista de reproducción");
            }
            else
            {
                cargarControles();
                switch (estado)
                {
                    case true:
                        axWindowsMediaPlayer1.Ctlcontrols.play();
                        bunifuImageButton4.Image = Properties.Resources.Pause_50px;
                        estado = false;
                        break;
                    case false:
                        axWindowsMediaPlayer1.Ctlcontrols.pause();
                        bunifuImageButton4.Image = Properties.Resources.Play_50px;
                        estado = true;
                        break;
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = trackBar1.Value;
        }

        private void bunifuSlider1_ValueChanged(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = bunifuSlider1.Value;
        }

        private void bunifuImageButton7_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.mute = true;
            trackBar1.Value = 0;
        }

        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {
            song = song.siguiente;
            if (song == null)
            {
                MessageBox.Show("Error");
            }
            else
            {
                axWindowsMediaPlayer1.URL = song.Directorio;
                //axWindowsMediaPlayer1.Ctlcontrols.pause();

                cargarControles();
            }
        }

        private void bunifuImageButton6_Click(object sender, EventArgs e)
        {
            song = song.anterior;
            if (song == null)
            {
                MessageBox.Show("Error");
            }
            else
            {
                axWindowsMediaPlayer1.URL = song.Directorio;
                //axWindowsMediaPlayer1.Ctlcontrols.pause();
                cargarControles();
            }
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            Guardar g = new Guardar();
            g.ShowDialog();
            GuardarLista();
            GuardaText(Datos.name);
        }

        public void GuardarLista()
        {
            FileStream archivo = new FileStream("Listas.txt", FileMode.Append, FileAccess.Write);
            StreamWriter Escribir = new StreamWriter(archivo);

            Escribir.WriteLine(Datos.name + ";" + Datos.imagen);
            Escribir.Close();
        }
        public void GuardaText(string name)
        {
            FileStream archivo = new FileStream(name + ".geo", FileMode.Append, FileAccess.Write);
            StreamWriter Escribir = new StreamWriter(archivo);
            String[] matriz = new String[listBox1.Items.Count];
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                matriz[i] = listBox1.Items[i].ToString();
                Escribir.WriteLine(matriz[i] + ";" + Datos.media.FindSong(matriz[i]) + ";" + Datos.name);
            }
            Escribir.Close();
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            LoadLista();
        }

        public void LoadLista()
        {
            string archivo = "";
            OpenFileDialog abrir = new OpenFileDialog();
            abrir.FileName = "Seleccione na lista";
            abrir.Filter = "Lista de Reproduccion|*.geo";
            abrir.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            abrir.FilterIndex = 1;
            if (abrir.ShowDialog() == DialogResult.OK)
            {
                archivo = abrir.FileName;
                FileStream lista = new FileStream(archivo, FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader LeerArchivo = new StreamReader(lista);

                axWindowsMediaPlayer1.Ctlcontrols.stop();

                Datos.media.delete();
                listBox1.Items.Clear();
                string linea;
                while ((linea = LeerArchivo.ReadLine()) != null)
                {
                    string[] cadena = linea.Split(';');
                    int cont = 0;
                    Cancion c = new Cancion();
                    foreach (string subcadena in cadena)
                    {
                        //Lleno un objeto de tipo contacto y le asigno sus propiedades
                        if (cont == 0) { listBox1.Items.Add(subcadena); c.Titulo = subcadena; }
                        if (cont == 1) { c.Directorio = subcadena; TagLib.File dt = TagLib.File.Create(subcadena); c.Artista = dt.Tag.JoinedPerformers; }
                        if (cont == 2) { label6.Text = subcadena; }
                        cont++; //aumento el contador para la ubicación de columnas
                    }
                    //ingreso ese objeto a la lista con sus respectivas propiedades
                    Datos.media.NextSong(c.Titulo, c.Artista, c.Directorio);
                }

                //cierro la lectura de archivo
                LeerArchivo.Close();

                CargarLista();

                label7.Text = Convert.ToString(listBox1.Items.Count);
                song = Datos.media.Inicio;
                axWindowsMediaPlayer1.URL = song.Directorio;
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                cargarControles();
            }
        }

        public void cargarControles()
        {
            TagLib.File dts = TagLib.File.Create(song.Directorio);
            label1.Text = dts.Tag.Title;
            label2.Text = dts.Tag.JoinedPerformers;

            if (dts.Tag.Pictures.Length >= 1)
            {

                MemoryStream pic = new MemoryStream(dts.Tag.Pictures[0].Data.Data);
                pictureBox2.Image = Image.FromStream(pic);
            }
            else
            {
                pictureBox2.Image = Properties.Resources.Music_96px;
            }
        }

        public void CargarLista()
        {
            string lt = "";
            string nombrearchivo = "Listas.txt";
            FileStream archivo = new FileStream(nombrearchivo, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader LeerArchivo = new StreamReader(archivo);

            string linea;
            while ((linea = LeerArchivo.ReadLine()) != null)
            {
                string[] cadena = linea.Split(';');
                int cont = 0;
                foreach (string subcadena in cadena)
                {
                    //Lleno un objeto de tipo contacto y le asigno sus propiedades
                    if (cont == 0) { lt = subcadena; }
                    if (cont == 1 && label6.Text == lt) { pictureBox3.Image =Image.FromFile(subcadena); }
                    cont++; //aumento el contador para la ubicación de columnas
                }
            }
            //cierro la lectura de archivo
            LeerArchivo.Close();
        }
    

        private void timer1_Tick(object sender, EventArgs e)
        {
            TrackSlide();
            bunifuSlider1.Value = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
            double t = Math.Floor(0 + axWindowsMediaPlayer1.Ctlcontrols.currentPosition);
            label3.Text = t.ToString();
            double t2 = Math.Floor(axWindowsMediaPlayer1.currentMedia.duration);
            label4.Text = t2.ToString();
        }
        public void TrackSlide()
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                bunifuSlider1.MaximumValue = (int)axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration;
                timer1.Start();
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                timer1.Stop();
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                timer1.Stop();
                bunifuSlider1.Value = 0;
            }
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            TrackSlide();
        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            Datos.media.delete();
            listBox1.Items.Clear();

            label1.Text = "¿Listo para escuchar música?";
            label2.Text = "Selecciona tu canción favorita";
            label6.Text = "Lista Actual";
            label7.Text = Convert.ToString(listBox1.Items.Count);
            pictureBox2.Image = Properties.Resources.Music_96px;
            pictureBox3.Image = Properties.Resources.Music_96px;
        }

        private void bunifuImageButton8_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }
        private void bunifuTextbox1_KeyPress(object sender, EventArgs e)
        {
        }

        private void bunifuImageButton9_Click(object sender, EventArgs e)
        {
            //string busqueda = "";
            //busqueda = Datos.media.Search(bunifuTextbox1.text);
            //if (busqueda == null)
            //{
            //    DialogResult result = MessageBox.Show("Que no se entere nadie, pero Justin Bieber acaba de robarse ésta canción ¿Desea buscar en otra lista?", "Busqueda", MessageBoxButtons.YesNoCancel);

            //    if (result == DialogResult.Yes)
            //    {
            //        LoadLista();
            //    }
            //    else if (result == DialogResult.No)
            //    {
            //    }
            //    else if (result == DialogResult.Cancel)
            //    {
            //    }
            //}
            //else
            //{
            //    TagLib.File tl = TagLib.File.Create(busqueda);
            //    DialogResult result = MessageBox.Show("Encontramos la canción que estabas buscando :D, Deseas repdorucurla en este instante?", "Salir", MessageBoxButtons.YesNoCancel);
            //    if (result == DialogResult.Yes)
            //    {
            //        axWindowsMediaPlayer1.URL = busqueda;
            //    }
            //    else if (result == DialogResult.No)
            //    {
            //    }
            //    else if (result == DialogResult.Cancel)
            //    {
            //    }
            //}
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
