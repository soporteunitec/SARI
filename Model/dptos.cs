namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class dptos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public dptos()
        {
            reportes = new HashSet<reportes>();
        }

        [Key]
        public int idDpto { get; set; }

        [StringLength(50)]
        public string nombre_dpto { get; set; }

        

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reportes> reportes { get; set; }

        
        public dptos Obtener(int id)
        {
            var departamento = new dptos();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    departamento = ctx.dptos.Where(x => x.idDpto == id)
                                          .SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return departamento;
        }

        public ResponseModel Guardar()
        {
            var rm = new ResponseModel();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    if (this.idDpto > 0) ctx.Entry(this).State = EntityState.Modified;
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
                    this.idDpto = id;
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

        public AnexGRIDResponde Listar(AnexGRID grid)
        {
            try
            {
                using (var ctx = new SistemaContext())
                {
                    grid.Inicializar();

                    var query = ctx.dptos.Where(x => x.idDpto > 0);

                    // Ordenamiento
                    if (grid.columna == "idDpto")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.idDpto)
                                                             : query.OrderBy(x => x.idDpto);
                    }

                    if (grid.columna == "nombre_dpto")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.nombre_dpto)
                                                             : query.OrderBy(x => x.nombre_dpto);
                    }


                    var dpto = query.Skip(grid.pagina)
                                       .Take(grid.limite)
                                       .ToList();

                    var total = query.Count();

                    grid.SetData(
                        from a in dpto
                        select new
                        {
                            a.idDpto,
                            a.nombre_dpto
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

        public List<dptos> Listar()
        {
            var departamentos = new List<dptos>();

            try
            {
                using (var ctx = new SistemaContext())
                {
                    departamentos = ctx.dptos.OrderBy(x => x.idDpto)
                                             .ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return departamentos;
        }

        

    }
}
