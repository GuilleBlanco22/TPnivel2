using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;
using System.Diagnostics.Eventing.Reader;

namespace negocio
{
    public class productoNegocio
    {
        public List<producto> listar() 
        {
            List <producto> lista = new List<producto>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;
            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "Select Codigo, Nombre, A.Descripcion, Precio, ImagenUrl, C.Descripcion as Categoria, M.Descripcion as Marca, A.IdCategoria, A.IdMarca, A.Id from ARTICULOS A, CATEGORIAS C, MARCAS M where C.Id = A.IdCategoria And M.Id = A.IdMarca";    
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();
                
                foreach (var item in lector) 
                //while (lector.Read())
                {
                    producto aux = new producto();
                    aux.Id = (int)lector["Id"];
                    aux.Codigo = (string)lector["Codigo"];
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];
                    aux.Precio = (decimal)lector["Precio"];
                    if (!(lector["ImagenUrl"] is DBNull));
                       aux.ImagenUrl = (string)lector["ImagenUrl"];
                    
                    aux.Categoria = new categoria();
                    aux.Categoria.Id = (int)lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)lector["Categoria"];
                    aux.Marca = new marca();
                    aux.Marca.Id = (int)lector["IdMarca"];
                    aux.Marca.Descripcion = (string)lector["Marca"];


                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            finally 
            {
                conexion.Close();
            }
        }

        public void agregar(producto nuevo)
        {
            accesoDatos datos = new accesoDatos();
            //producto producto = new producto();
               // categoria categoria = new categoria();
              //  marca marca = new marca();
            try
            {
                datos.setearConsulta("Insert into ARTICULOS (Nombre, Codigo, Descripcion, ImagenUrl, Precio, IdCategoria, IdMarca) values(@Nombre, @Codigo, @Descripcion, @ImagenUrl, @Precio, @IdCategoria, @IdMarca)");
                datos.setearParamentro("@Nombre", nuevo.Nombre);
                datos.setearParamentro("@Codigo", nuevo.Codigo);
                datos.setearParamentro("@Descripcion", nuevo.Descripcion);
                datos.setearParamentro("@ImagenUrl", nuevo.ImagenUrl);
                datos.setearParamentro("@Precio", nuevo.Precio);
                datos.setearParamentro("@idCategoria", nuevo.Categoria.Id);
                datos.setearParamentro("@idMarca", nuevo.Marca.Descripcion);   
                
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(producto modificado)
        {
            accesoDatos datos = new accesoDatos();
            try
            {
                datos.setearConsulta("update ARTICULOS set Codigo = @Codigo, Nombre = @Nombre, Descripcion = @descripcion, Precio = @Precio, ImagenUrl = @ImagenUrl, IdCategoria = @IdCategoria, IdMarca = @IdMarca where Id = @Id");
                datos.setearParamentro("@Codigo", modificado.Codigo);
                datos.setearParamentro("@Nombre", modificado.Nombre);
                datos.setearParamentro("@Descripcion", modificado.Descripcion);
                datos.setearParamentro("@Precio", modificado.Precio);
                datos.setearParamentro("@ImagenUrl", modificado.ImagenUrl);
                datos.setearParamentro("@IdCategoria", modificado.Categoria.Id);
                datos.setearParamentro("@IdMarca", modificado.Marca.Id);
                datos.setearParamentro("@Id", modificado.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally 
            {
                datos.cerrarConexion();
            }
        }
        public void eliminar(int id) 
        {
            try
            {
                accesoDatos datos = new accesoDatos();
                datos.setearConsulta("delete from ARTICULOS where id = @id");
                datos.setearParamentro("@id", id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<producto> filtrar(string campo, string criterio, string filtro)
        {
            List<producto > lista = new List<producto>();
            accesoDatos datos = new accesoDatos();
            try
            {
                string consulta = "Select Codigo, Nombre, A.Descripcion, Precio, ImagenUrl, C.Descripcion as Categoria, M.Descripcion as Marca, A.IdCategoria, A.IdMarca, A.Id from ARTICULOS A, CATEGORIAS C, MARCAS M where C.Id = A.IdCategoria And M.Id = A.IdMarca And ";
                if (campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Precio > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Precio < " + filtro;
                            break;
                        default:
                            consulta += "Precio = " + filtro;
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "A.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "A.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "A.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                foreach (var item in datos.Lector)
                //while (datos.Lector.Read())
                {
                    producto aux = new producto();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Precio = (decimal)datos.Lector["Precio"];
                    if (!(datos.Lector["ImagenUrl"] is DBNull)) ;
                    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];

                    aux.Categoria = new categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.Marca = new marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];


                    lista.Add(aux);
                }


                return lista;
            }


            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
