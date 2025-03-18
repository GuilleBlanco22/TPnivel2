using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class producto
    {
        public int Id { get; set; }
        [DisplayName("Código")] //como se muestra el nombre en la gridview
        public string Codigo {  get; set; }
        public string Nombre { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        [DisplayName("url Imagen")]
        public string ImagenUrl { get; set; }
        public decimal Precio { get; set; }
        public categoria Categoria { get; set; }
        public marca Marca { get; set; }
    }
}
