namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("vitacora")]
    public partial class vitacora
    {
        [Key]
        public int idVitacora { get; set; }

        [StringLength(50)]
        public string descripcion { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime fecha { get; set; }

        public int? idUsuario { get; set; }

        public virtual usuarios usuarios { get; set; }

        public vitacora Obtener(int id)
        {
            var Vitacora = new vitacora();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    Vitacora = ctx.vitacora.Where(x => x.idVitacora == id)
                                          .SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Vitacora;
        }

        public ResponseModel Guardar()
        {
            var rm = new ResponseModel();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    if (this.idVitacora > 0) ctx.Entry(this).State = EntityState.Modified;
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
                    this.idVitacora = id;
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
                    ctx.Configuration.LazyLoadingEnabled = false;

                    var query = ctx.vitacora.Include("usuarios")
                                            .Where(x => x.idVitacora > 0);


                    if (grid.columna == "idVitacora")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.idVitacora)
                                                             : query.OrderBy(x => x.idVitacora);
                    }
                    if (grid.columna == "descripcion")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.descripcion)
                                                             : query.OrderBy(x => x.descripcion);
                    }
                    if (grid.columna == "fecha")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.fecha)
                                                             : query.OrderBy(x => x.fecha);
                    }
                
                    if (grid.columna == "nombre")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.usuarios.nombre)
                                                             : query.OrderBy(x => x.usuarios.nombre);
                    }


                var Vitacora = query.Skip(grid.pagina)
                                       .Take(grid.limite)
                                       .ToList();

                    var total = query.Count();


                    grid.SetData(
                        from a in Vitacora
                        select new
                        {
                            a.idVitacora,
                            a.descripcion,
                            a.fecha,
                            a.usuarios.nombre,
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
