namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("tipoInventario")]
    public partial class tipoInventario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tipoInventario()
        {
            inventarios = new HashSet<inventarios>();
        }

        [Key]
        public int idTipoInventario { get; set; }

        [StringLength(50)]
        public string nombre_tipo { get; set; }

        [StringLength(50)]
        public string descripcion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventarios> inventarios { get; set; }

        public tipoInventario Obtener(int id)
        {
            var tipoInventario = new tipoInventario();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    tipoInventario = ctx.tipoInventario.Where(x => x.idTipoInventario == id)
                                          .SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return tipoInventario;
        }

        public ResponseModel Guardar()
        {
            var rm = new ResponseModel();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    if (this.idTipoInventario > 0) ctx.Entry(this).State = EntityState.Modified;
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
                    this.idTipoInventario = id;
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

        public List<tipoInventario> Listar()
        {
            var tipoInventario = new List<tipoInventario>();

            try
            {
                using (var ctx = new SistemaContext())
                {
                    tipoInventario = ctx.tipoInventario.OrderBy(x => x.idTipoInventario)
                                             .ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return tipoInventario;
        }

        public AnexGRIDResponde Listar(AnexGRID grid)
        {
            try
            {
                using (var ctx = new SistemaContext())
                {
                    grid.Inicializar();

                    var query = ctx.tipoInventario.Where(x => x.idTipoInventario > 0);

                    // Ordenamiento
                    if (grid.columna == "idTipoInventario")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.idTipoInventario)
                                                             : query.OrderBy(x => x.idTipoInventario);
                    }

                    if (grid.columna == "nombre_tipo")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.nombre_tipo)
                                                             : query.OrderBy(x => x.nombre_tipo);
                    }
                    if (grid.columna == "descripcion")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.descripcion)
                                                             : query.OrderBy(x => x.descripcion);
                    }


                    var tipoInventario = query.Skip(grid.pagina)
                                       .Take(grid.limite)
                                       .ToList();

                    var total = query.Count();

                    grid.SetData(
                        from a in tipoInventario
                        select new
                        {
                            a.idTipoInventario,
                            a.nombre_tipo,
                            a.descripcion
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
