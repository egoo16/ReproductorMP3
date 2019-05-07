using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reproductor02
{
    class Cancion
    {
        private string titulo;
        private string artista;
        private string directorio;
        public Cancion siguiente;
        public Cancion anterior;

        public string Titulo
        {
            get
            {
                return titulo;
            }

            set
            {
                titulo = value;
            }
        }

        public string Artista
        {
            get
            {
                return artista;
            }

            set
            {
                artista = value;
            }
        }

        public string Directorio
        {
            get
            {
                return directorio;
            }

            set
            {
                directorio = value;
            }
        }
    }
}
