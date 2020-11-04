namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class prestamos
    {
        [Key]
        public int idPrestamos { get; set; }

        [StringLength(50)]
        public string Aquien_presta { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? fecha_entrega { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? fecha_devolucion { get; set; }

        [StringLength(50)]
        public string nota { get; set; }

        [StringLength(50)]
        public string nota_devolucion { get; set; }

        [StringLength(10)]
        public string estatus { get; set; }

        public int? idUsuario { get; set; }

        public virtual usuarios usuarios { get; set; }


        public prestamos Obtener(int id)
        {
            var Prestamo = new prestamos();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    Prestamo = ctx.prestamos.Where(x => x.idPrestamos == id)
                                          .SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Prestamo;
        }

        public ResponseModel Guardar()
        {
            var rm = new ResponseModel();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    if (this.idPrestamos > 0) ctx.Entry(this).State = EntityState.Modified;
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

        public ResponseModel CerrarPrestamo()
        {
            var rm = new ResponseModel();
            try
            {
                using (var ctx = new SistemaContext())
                {

                    ctx.Configuration.AutoDetectChangesEnabled = false;
                    ctx.Configuration.ValidateOnSaveEnabled = false;

                    var eCS = ctx.Entry(this);
                    eCS.State = EntityState.Modified;
                    eCS.Property(x => x.Aquien_presta).IsModified = false;
                    eCS.Property(x => x.fecha_entrega).IsModified = false;
                    eCS.Property(x => x.nota).IsModified = false;
                    eCS.Property(x => x.idUsuario).IsModified = false;
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
                    this.idPrestamos = id;
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

                    var query = ctx.prestamos.Include("usuarios")
                                            .Where(x => x.idPrestamos > 0)
                                            .Where(x => x.estatus == "1");





                    // Ordenamiento
                    if (grid.columna == "idPrestamos")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.idPrestamos)
                                                             : query.OrderBy(x => x.idPrestamos);
                    }
                    if (grid.columna == "Aquien_presta")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.Aquien_presta)
                                                             : query.OrderBy(x => x.Aquien_presta);
                    }
                    if (grid.columna == "fecha_entrega")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.fecha_entrega)
                                                             : query.OrderBy(x => x.fecha_entrega);
                    }
                    if (grid.columna == "nota")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.nota)
                                                             : query.OrderBy(x => x.nota);
                    }
                    if (grid.columna == "nombre")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.usuarios.nombre)
                                                             : query.OrderBy(x => x.usuarios.nombre);
                    }

                    foreach (var f in grid.filtros)
                    {
                        if (f.columna == "Aquien_presta")
                            query = query.Where(x => x.Aquien_presta.StartsWith(f.valor));
                        if (f.columna == "nota")
                            query = query.Where(x => x.nota.StartsWith(f.valor));
                        if (f.columna == "nombre")
                            query = query.Where(x => x.usuarios.nombre.StartsWith(f.valor));

                    }

                    var prestamo = query.Skip(grid.pagina)
                                       .Take(grid.limite)
                                       .ToList();

                    var total = query.Count();


                    grid.SetData(
                        from a in prestamo
                        select new
                        {
                            a.idPrestamos,
                            a.Aquien_presta,
                            a.fecha_entrega,
                            a.nota,
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

        public AnexGRIDResponde ListarCerrados(AnexGRID grid)
        {
            try
            {
                using (var ctx = new SistemaContext())
                {
                    grid.Inicializar();
                    ctx.Configuration.LazyLoadingEnabled = false;

                    var query = ctx.prestamos.Include("usuarios")
                                            .Where(x => x.idPrestamos > 0)
                                            .Where(x => x.estatus == "2");



                    if (grid.columna == "idPrestamos")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.idPrestamos)
                                                             : query.OrderBy(x => x.idPrestamos);
                    }
                    if (grid.columna == "Aquien_presta")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.Aquien_presta)
                                                             : query.OrderBy(x => x.Aquien_presta);
                    }
                    if (grid.columna == "fecha_entrega")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.fecha_entrega)
                                                             : query.OrderBy(x => x.fecha_entrega);
                    }
                    if (grid.columna == "fecha_devolucion")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.fecha_devolucion)
                                                             : query.OrderBy(x => x.fecha_devolucion);
                    }
                    if (grid.columna == "nombre")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.usuarios.nombre)
                                                             : query.OrderBy(x => x.usuarios.nombre);
                    }
                    if (grid.columna == "nota")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.nota)
                                                             : query.OrderBy(x => x.nota);
                    }

                    if (grid.columna == "nota_devolucion")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.nota_devolucion)
                                                             : query.OrderBy(x => x.nota_devolucion);
                    }

                    foreach (var f in grid.filtros)
                    {
                        if (f.columna == "Aquien_presta")
                            query = query.Where(x => x.Aquien_presta.StartsWith(f.valor));
                        if (f.columna == "nota")
                            query = query.Where(x => x.nota.StartsWith(f.valor));
                        if (f.columna == "nombre")
                            query = query.Where(x => x.usuarios.nombre.StartsWith(f.valor));
                        if (f.columna == "nota_devolucion")
                            query = query.Where(x => x.nota_devolucion.StartsWith(f.valor));
                    }

                    var prestamo = query.Skip(grid.pagina)
                                      .Take(grid.limite)
                                      .ToList();

                    var total = query.Count();


                    grid.SetData(
                        from a in prestamo
                        select new
                        {
                            a.idPrestamos,
                            a.Aquien_presta,
                            a.fecha_entrega,
                            a.nota,
                            a.nota_devolucion,
                            a.fecha_devolucion,
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

