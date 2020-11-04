namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class tipo_falla
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tipo_falla()
        {
            reportes = new HashSet<reportes>();
        }

        [Key]
        public int idTipoFalla { get; set; }

        [StringLength(50)]
        public string nombre_falla { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reportes> reportes { get; set; }


        public tipo_falla Obtener(int id)
        {
            var falla = new tipo_falla();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    falla = ctx.tipo_falla.Where(x => x.idTipoFalla == id)
                                          .SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return falla;
        }

        public ResponseModel Guardar()
        {
            var rm = new ResponseModel();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    if (this.idTipoFalla > 0) ctx.Entry(this).State = EntityState.Modified;
                    else ctx.Entry(this).State = EntityState.Added;

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
                    this.idTipoFalla = id;
                    ctx.Entry(this).State = EntityState.Deleted;

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

        public List<tipo_falla> Listar()
        {
            var tipofalla = new List<tipo_falla>();

            try
            {
                using (var ctx = new SistemaContext())
                {
                    tipofalla = ctx.tipo_falla.OrderBy(x => x.idTipoFalla)
                                             .ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return tipofalla;
        }

        public AnexGRIDResponde Listar(AnexGRID grid)
        {
            try
            {
                using (var ctx = new SistemaContext())
                {
                    grid.Inicializar();

                    var query = ctx.tipo_falla.Where(x => x.idTipoFalla > 0);

                    // Ordenamiento
                    if (grid.columna == "idTipoFalla")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.idTipoFalla)
                                                             : query.OrderBy(x => x.idTipoFalla);
                    }

                    if (grid.columna == "nombre_falla")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.nombre_falla)
                                                             : query.OrderBy(x => x.nombre_falla);
                    }


                    var falla = query.Skip(grid.pagina)
                                       .Take(grid.limite)
                                       .ToList();

                    var total = query.Count();

                    grid.SetData(
                        from a in falla
                        select new
                        {
                            a.idTipoFalla,
                            a.nombre_falla
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
