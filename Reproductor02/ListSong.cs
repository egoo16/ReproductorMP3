using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reproductor02
{
    class ListSong
    {
        private Cancion inicio;
        public Cancion Inicio
        {
            get { return inicio; }
        }
        public ListSong()
        {
            inicio = null;
        }
        private void FirstSong(string nombre, string artista, string directorio) //Se crea la Primera persona que se ingresa
        {
            Cancion nuevo = new Cancion();
            nuevo.Titulo = nombre;
            nuevo.Artista = artista;
            nuevo.Directorio = directorio;

            nuevo.siguiente = nuevo; //lista circular 
            nuevo.anterior = nuevo;
            inicio = nuevo;
        }

        public void NextSong(string nombre, string artista, string directorio) //se define siguiente persona para que avance
        {
            if (inicio == null)
            {
                FirstSong(nombre, artista, directorio); //si es igual a vacio se miestra lo que tenia de ultimo
            }
            else
            {
                Cancion nuevo = new Cancion();
                nuevo.Titulo = nombre;
                nuevo.Artista = artista;
                nuevo.Directorio = directorio;

                Cancion temp = new Cancion();
                temp = inicio.anterior;
                nuevo.siguiente = inicio;
                nuevo.anterior = temp;
                inicio.anterior = nuevo;
                temp.siguiente = nuevo;
            }
        }

        public string FindSong(string title) //Busca los contatos que tiene cada nodo 
        {
            Cancion temporal = new Cancion();   //se cra un temporal solo para que recorra los nodo 
            temporal = inicio;
            while (temporal != null) //mientras que temporal sea diferente de vacio debera mostrar lo que le pidan
            {
                if (temporal.Titulo == title)
                {
                    return temporal.Directorio;  //retorna el valor que encuentre en el temporal
                }
                else
                {
                    temporal = temporal.siguiente; //para que continue movinendose el temporal 
                }
            }
            return null;
        }

        public string Search(string title) //Busca los contatos que tiene cada nodo 
        {
            Cancion temporal = new Cancion();   //se cra un temporal solo para que recorra los nodo 
            temporal = inicio;
            while (temporal != null) //mientras que temporal sea diferente de vacio debera mostrar lo que le pidan
            {
                if (temporal.Titulo.Equals(title))
                {
                    return temporal.Directorio;  //retorna el valor que encuentre en el temporal
                }
                else
                {
                    temporal = temporal.siguiente; //para que continue movinendose el temporal 
                }
            }
            return null;
        }

        public void delete() //Busca los contatos que tiene cada nodo 
        {
            Cancion actual = new Cancion();   //se cra un temporal solo para que recorra los nodo 
            Cancion temp = new Cancion();
            actual = inicio;
            while (actual != null) //mientras que temporal sea diferente de vacio debera mostrar lo que le pidan
            {
                inicio = null;
                temp = actual.siguiente;
                actual = null;
            }
        }
    }
}
