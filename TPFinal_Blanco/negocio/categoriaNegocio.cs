﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class categoriaNegocio
    {
        

        public List<categoria> listar()
        {
			List<categoria> lista = new List<categoria>();
			accesoDatos datos = new accesoDatos();

			try
			{
				datos.setearConsulta("Select Descripcion, Id From CATEGORIAS");
				datos.ejecutarLectura();

				//foreach (var item in lista)
							
				while (datos.Lector.Read())
				{
					categoria aux = new categoria();
					aux.Id = (int)datos.Lector["Id"];
					aux.Descripcion = (string)datos.Lector["Descripcion"];

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
				datos.cerrarConexion();
			}
        }
    }
}
