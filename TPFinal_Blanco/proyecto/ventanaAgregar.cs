using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Globalization;


namespace proyecto
{
    public partial class ventanaAgregar : Form
    {
        private producto producto = null;
        private OpenFileDialog imagen = null;
        public ventanaAgregar()
        {
            InitializeComponent();
        }
        public ventanaAgregar(producto producto)
        {
            InitializeComponent();
            this.producto = producto;
            Text = "Modificar producto";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //producto producto = new producto();
            productoNegocio negocio = new productoNegocio();
            string esNumero = tboxPrecio.Text;
            float i = 0;

            //validacion para que se ingresen los campos y solo numeros en el precio
            if (tboxCodigo.Text == "" || tboxNombre.Text == "" || tboxPrecio.Text == "")
            {
                MessageBox.Show("Por favor complete los campos obligatorios: Codigo, Nombre y Precio");

            }
            else if (!(float.TryParse(esNumero, out i))) 
            {
                MessageBox.Show("Solo numeros para el precio.");
            }
            else
            {

                try
                {
                    if (producto == null)
                        producto = new producto();

                    producto.Categoria = new categoria();
                    producto.Marca = new marca();

                    producto.Codigo = tboxCodigo.Text;
                    producto.Nombre = tboxNombre.Text;
                    producto.Descripcion = tboxDescripcion.Text;
                    producto.ImagenUrl = tboxUrl.Text;
                    producto.Categoria.Id = (int)cboxCategoria.SelectedValue;
                    producto.Marca.Id = (int)cboxMarca.SelectedValue;
                    //producto.Categoria = (categoria)cboxCategoria.SelectedItem;
                    //producto.Marca = (marca)cboxMarca.SelectedItem;
                    producto.Precio = decimal.Parse(tboxPrecio.Text);

                    if (producto.Id != 0)
                    {
                        negocio.modificar(producto);
                        MessageBox.Show("Modificado con exito!");
                    }
                    else
                    {
                        negocio.agregar(producto);
                        MessageBox.Show("Agregado con exito!");
                    }
                    if (imagen != null && !(tboxUrl.Text.ToUpper().Contains("HTTP")))
                    {
                        File.Copy(imagen.FileName, ConfigurationManager.AppSettings["carpeta-imagenes"] + imagen.SafeFileName);
                    }

                    Close();


                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }


        }

        private void ventanaAgregar_Load(object sender, EventArgs e)
        {     

            marcaNegocio marca = new marcaNegocio();
            categoriaNegocio categoria = new categoriaNegocio();

            try
            {
                cboxMarca.DataSource = marca.listar();
                cboxMarca.ValueMember = "Id";
                cboxMarca.DisplayMember = "Descripcion";
                cboxCategoria.DataSource = categoria.listar();
                cboxCategoria.ValueMember = "Id";
                cboxCategoria.DisplayMember = "Descripcion";

                if (producto != null)
                {
                    tboxCodigo.Text = producto.Codigo;
                    tboxNombre.Text = producto.Nombre;
                    tboxDescripcion.Text = producto.Descripcion;
                    tboxUrl.Text = producto.ImagenUrl;
                    cargarImagen(producto.ImagenUrl);
                    tboxPrecio.Text = producto.Precio.ToString();
                    cboxCategoria.SelectedValue = producto.Categoria.Id;
                    cboxMarca.SelectedValue = producto.Marca.Id;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    

        private void tboxUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(tboxUrl.Text);
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pboxAgregar.Load(imagen);
            }
            catch (Exception)
            {
                pboxAgregar.Load("https://media.istockphoto.com/id/1396814518/es/vector/imagen-pr%C3%B3ximamente-sin-foto-sin-imagen-en-miniatura-disponible-ilustraci%C3%B3n-vectorial.jpg?s=612x612&w=0&k=20&c=aA0kj2K7ir8xAey-SaPc44r5f-MATKGN0X0ybu_A774=");

            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
        }

        private void btnAgregarImagen_Click_1(object sender, EventArgs e)
        {
            imagen = new OpenFileDialog();
            imagen.Filter = "jpg|*.jpg;|png|*.png";//tipo de archivos que acepta
            if (imagen.ShowDialog() == DialogResult.OK)
            {
                tboxUrl.Text = imagen.FileName;
                cargarImagen(imagen.FileName);

                //File.Copy(imagen.FileName, ConfigurationManager.AppSettings["carpeta-imagenes"] + imagen.SafeFileName);
            }
        }










     
        private void tboxPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
   
        }
    }
} 
