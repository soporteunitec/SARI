namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("documentacion")]
    public partial class documentacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public documentacion()
        {
            AdjuntosDocumentacion = new HashSet<AdjuntosDocumentacion>();
        }

        [Key]
        public int idDocumentacion { get; set; }

        [StringLength(50)]
        public string nombre_documento { get; set; }

        [StringLength(50)]
        public string descripcion { get; set; }


        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)] 
        [DataType(DataType.Date)]
        public DateTime fecha_creacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AdjuntosDocumentacion> AdjuntosDocumentacion { get; set; }

        public documentacion Obtener(int id)
        {
            var Documentacion = new documentacion();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    Documentacion = ctx.documentacion.Where(x => x.idDocumentacion == id)
                                                     .SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Documentacion;
        }

        public ResponseModel Guardar()
        {
            var rm = new ResponseModel();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    if (this.idDocumentacion > 0)
                    {
                        
                        ctx.Entry(this).State = EntityState.Modified;
                    }
                    else
                    {
                        ctx.Entry(this).State = EntityState.Added;
                    }
                   

                    ctx.SaveChanges();
                    rm.SetResponse(true);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return rm;
        }

        public ResponseModel eliminar(int id)
        {
            var rm = new ResponseModel();
            try
            {
                using (var ctx = new SistemaContext())
                {

                    var delete = ctx.documentacion.Include("AdjuntosDocumentacion")
                                                 .Where(c => c.idDocumentacion == id)
                                                 .FirstOrDefault();
                    ctx.documentacion.Remove(delete);
                    ctx.SaveChanges();
                    rm.SetResponse(true);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return rm;
        }

        public AnexGRIDResponde Listar(AnexGRID grid)
        {
            try
            {
                using (var ctx = new SistemaContext())
                {
                    grid.Inicializar();

                    var query = ctx.documentacion.Where(x => x.idDocumentacion > 0);

                    // Ordenamiento
                    if (grid.columna == "idDocumentacion")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.idDocumentacion)
                                                             : query.OrderBy(x => x.idDocumentacion);
                    }

                    if (grid.columna == "nombre_documento")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.nombre_documento)
                                                             : query.OrderBy(x => x.nombre_documento);
                    }
                    if (grid.columna == "descripcion")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.descripcion)
                                                             : query.OrderBy(x => x.descripcion);
                    }
                    if (grid.columna == "fecha_creacion")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.fecha_creacion)
                                                             : query.OrderBy(x => x.fecha_creacion);
                    }
                    foreach (var f in grid.filtros)
                    {
                        if (f.columna == "nombre_documento")
                            query = query.Where(x => x.nombre_documento.StartsWith(f.valor));

                    }


                    var documentacion = query.Skip(grid.pagina)
                                       .Take(grid.limite)
                                       .ToList();

                    var total = query.Count();

                    grid.SetData(
                        from a in documentacion
                        select new
                        {
                            a.idDocumentacion,
                            a.nombre_documento,
                            a.descripcion,
                            a.fecha_creacion,
                        },
                        total
                    );
                }
            }
            catch (Exception E)
            {

                throw;
            }

            return grid.responde();
        }
    }
}
