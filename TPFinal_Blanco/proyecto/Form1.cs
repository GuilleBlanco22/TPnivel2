using System;
using System.CodeDom;
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

namespace proyecto
{
    public partial class Form1 : Form
    {
        private List<producto> listaProducto;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
            cboxCampo.Items.Add("Precio");
            cboxCampo.Items.Add("Nombre");
            cboxCampo.Items.Add("Descripcion");
        }
           
        private void cargar()
        {
            productoNegocio negocio = new productoNegocio();

            try
            {
                listaProducto = negocio.listar();
                dgvProductos.DataSource = listaProducto;
                ocultarColumnas();
                cargarImagen(listaProducto[0].ImagenUrl);
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            
        }
        private void ocultarColumnas()
        {
            dgvProductos.Columns["ImagenUrl"].Visible = false;
            dgvProductos.Columns["Id"].Visible = false;
        }
        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow != null)
            {
                producto seleccionado = (producto)dgvProductos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
            }
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pboxProducto.Load(imagen);
            }
            catch (Exception)
            {
                pboxProducto.Load("https://media.istockphoto.com/id/1396814518/es/vector/imagen-pr%C3%B3ximamente-sin-foto-sin-imagen-en-miniatura-disponible-ilustraci%C3%B3n-vectorial.jpg?s=612x612&w=0&k=20&c=aA0kj2K7ir8xAey-SaPc44r5f-MATKGN0X0ybu_A774=");
                
            }
        }

        

        /* DISEÑO DE CAMBIAR DE CLARO A MODO OSCURO
        
        private void btnModoOscuro_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.Black;
        }

        private void btnModoClaro_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }*/

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult cerrar = MessageBox.Show("¿Desea salir de la app?", "Cerrar app", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if(cerrar == DialogResult.Yes)
            this.Close();
        }

        private void agregarProductoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ventanaAgregar agregar = new ventanaAgregar();//abrir otra ventana desde la principal
            agregar.ShowDialog();
        }

        private void toolStripAgregar_Click_1(object sender, EventArgs e)
        {
            ventanaAgregar agregar = new ventanaAgregar();
            agregar.ShowDialog();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            ventanaAgregar agregar = new ventanaAgregar();
            agregar.ShowDialog();
            cargar();
        }


        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
               producto seleccionado;
               seleccionado = (producto)dgvProductos.CurrentRow.DataBoundItem;
            
               ventanaAgregar modificar = new ventanaAgregar(seleccionado);
               modificar.ShowDialog();
               cargar();

            }
            catch (Exception)
            {
                MessageBox.Show("Seleccione un producto para modificar.");
            }
            
        }
        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            productoNegocio producto = new productoNegocio();
            producto seleccionado;
            try
            {
                //lanza advertencia de eliminar
                DialogResult respuesta = MessageBox.Show("¿Desea eliminar el producto?","Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (producto)dgvProductos.CurrentRow.DataBoundItem;
                    producto.eliminar(seleccionado.Id);
                    cargar();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private bool validarFiltro()
        {
            if (cboxCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione campo y criterio para filtrar.");
                return true;
            }
            if (cboxCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar.");
                return true;
            }
            if(cboxCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(tboxFiltro.Text))
                {
                    MessageBox.Show("Ingrese numero para buscar.");
                }
                if (!(soloNumeros(tboxFiltro.Text)))
                {
                    MessageBox.Show("Ingrese solo numeros.");
                    return true;
                }
            }
            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))                  
                    return false;
            }
            return true;
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            productoNegocio negocio = new productoNegocio();
            try
            {
                if (validarFiltro())
                    return;

                string campo = cboxCampo.SelectedItem.ToString();
                string criterio = cboxCriterio.SelectedItem.ToString();
                string filtro = tboxFiltro.Text;
                dgvProductos.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void tboxBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void tboxBuscar_TextChanged(object sender, EventArgs e)
        {
            List<producto> listaFiltrada;
            string filtro = tboxBuscar.Text;

            if (filtro.Length >= 2)
            {
                listaFiltrada = listaProducto.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Categoria.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaProducto;
            }


            dgvProductos.DataSource = null;
            dgvProductos.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cboxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboxCampo.SelectedItem.ToString();
            if(opcion != "Precio")
            {
                cboxCriterio.Items.Clear();
                cboxCriterio.Items.Add("Comienza con");
                cboxCriterio.Items.Add("Termina con");
                cboxCriterio.Items.Add("Contiene");
            }
            else
            {
                cboxCriterio.Items.Clear();
                cboxCriterio.Items.Add("Mayor a");
                cboxCriterio.Items.Add("Menor a");
                cboxCriterio.Items.Add("Igual a");
            }
        }

        
    }
}
