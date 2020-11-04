namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("AdjuntosDocumentacion")]
    public partial class AdjuntosDocumentacion
    {
        [Key]
        public int idAdjuntosDocumentacion { get; set; }

        [StringLength(50)]
        public string archivo { get; set; }

        public int? idDocumentacion { get; set; }

        public virtual documentacion documentacion { get; set; }
        public List<AdjuntosDocumentacion> Listar(int id)
        {
            var adjuntos = new List<AdjuntosDocumentacion>();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    adjuntos = ctx.AdjuntosDocumentacion.Where(x => x.idDocumentacion == id)
                                          .ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return adjuntos;
        }

        public void eliminarAdjunto(int id)
        {

            try
            {
                using (var ctx = new SistemaContext())
                {
                    this.idAdjuntosDocumentacion = id;
                    ctx.Entry(this).State = EntityState.Deleted;

                    ctx.SaveChanges();

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public ResponseModel Guardar()
        {
            var rm = new ResponseModel();

            try
            {
                using (var ctx = new SistemaContext())
                {
                    ctx.Entry(this).State = EntityState.Added;

                    rm.SetResponse(true);
                    ctx.SaveChanges();
                }
            }
            catch (Exception E)
            {
                throw;
            }

            return rm;
        }

        public AdjuntosDocumentacion Obtener(int id)
        {
            var adjunto = new AdjuntosDocumentacion();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    adjunto = ctx.AdjuntosDocumentacion.Where(x => x.idAdjuntosDocumentacion == id)
                                          .SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return adjunto;
        }
    }
}
