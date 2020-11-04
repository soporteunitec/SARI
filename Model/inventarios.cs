namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class inventarios
    {
        [Key]
        public int idInventario { get; set; }

        [StringLength(50)]
        public string nombre_inventario { get; set; }

        [StringLength(50)]
        public string cantidad { get; set; }

        [StringLength(50)]
        public string serial { get; set; }

        [StringLength(50)]
        public string ubicacion { get; set; }

        public int? idUsuario { get; set; }
        

        public int? idTipoInventario { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? fecha_creacion { get; set; }

        public virtual usuarios usuarios { get; set; }

        public virtual tipoInventario tipoInventario { get; set; }

        public inventarios Obtener(int id)
        {
            var inventario = new inventarios();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    inventario = ctx.inventarios.Where(x => x.idInventario == id)
                                          .SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return inventario;
        }

        public ResponseModel Guardar()
        {
            var rm = new ResponseModel();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    if (this.idInventario > 0) ctx.Entry(this).State = EntityState.Modified;
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
                    this.idInventario = id;
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

              
                    var query = ctx.inventarios.Include("tipoInventario")
                                            .Include("usuarios")
                                            .Where(x => x.idInventario > 0);

           





                    // Ordenamiento
                    if (grid.columna == "idInventario")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.idInventario)
                                                             : query.OrderBy(x => x.idInventario);
                    }
                    if (grid.columna == "nombre_inventario")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.nombre_inventario)
                                                             : query.OrderBy(x => x.nombre_inventario);
                    }
                    if (grid.columna == "cantidad")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.cantidad)
                                                             : query.OrderBy(x => x.cantidad);
                    }
                    if (grid.columna == "serial")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.serial)
                                                             : query.OrderBy(x => x.serial);
                    }
                    if (grid.columna == "ubicacion")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.ubicacion)
                                                             : query.OrderBy(x => x.ubicacion);
                    }
                    if (grid.columna == "nombre")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.usuarios.nombre)
                                                             : query.OrderBy(x => x.usuarios.nombre);
                    }
                    if (grid.columna == "nombre_tipo")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.tipoInventario.nombre_tipo)
                                                             : query.OrderBy(x => x.tipoInventario.nombre_tipo);
                    }
                    if (grid.columna == "fecha_creacion")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.fecha_creacion)
                                                             : query.OrderBy(x => x.fecha_creacion);
                    }

                    foreach (var f in grid.filtros)
                    {
                        if (f.columna == "nombre_inventario")
                            query = query.Where(x => x.nombre_inventario.StartsWith(f.valor));
                        if (f.columna == "cantidad")
                            query = query.Where(x => x.cantidad.StartsWith(f.valor));
                        if (f.columna == "serial")
                            query = query.Where(x => x.serial.StartsWith(f.valor));
                        if (f.columna == "ubicacion")
                            query = query.Where(x => x.ubicacion.StartsWith(f.valor));
                        if (f.columna == "nombre")
                            query = query.Where(x => x.usuarios.nombre.StartsWith(f.valor));
                        if (f.columna == "nombre_tipo")
                            query = query.Where(x => x.tipoInventario.nombre_tipo.StartsWith(f.valor));

                    }


                    var inventario = query.Skip(grid.pagina)
                                       .Take(grid.limite)
                                       .ToList();

                    var total = query.Count();


                    grid.SetData(
                        from a in inventario
                        select new
                        {
                            a.idInventario,
                            a.nombre_inventario,
                            a.cantidad,
                            a.serial,
                            a.ubicacion,
                            a.usuarios.nombre,
                            a.tipoInventario.nombre_tipo,
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
