public class dptoConsulta
{
    public string nombre;
    public int cantidad;
}
namespace Model
{
    using Helper;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class reportes
    {
        [Key]
        public int idReportes { get; set; }

        [StringLength(50)]
        public string reportado_por { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? fecha_inicio { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? fecha_cierre { get; set; }

        [StringLength(50)]
        public string problema_presentado { get; set; }

        [StringLength(100)]
        public string falla { get; set; }

        [StringLength(100)]
        public string solucion { get; set; }

        [StringLength(10)]
        public string estatus { get; set; }

        public int? idUsuario { get; set; }

        public int? idTipoFalla { get; set; }

        public int? idDpto { get; set; }

        public virtual dptos dptos { get; set; }

        public virtual usuarios usuarios { get; set; }

        public virtual tipo_falla tipo_falla { get; set; }

        public reportes Obtener(int id)
        {
            var reportes = new reportes();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    reportes = ctx.reportes.Where(x => x.idReportes == id)
                                          .SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return reportes;
        }

        public ResponseModel Guardar()
        {
            var rm = new ResponseModel();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    if (this.idReportes > 0) ctx.Entry(this).State = EntityState.Modified;
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

        public ResponseModel CerrarCaso()
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
                    //eCS.Property(x => x.idReportes).IsModified = false;
                    eCS.Property(x => x.reportado_por).IsModified = false;
                    eCS.Property(x => x.fecha_inicio).IsModified = false;
                    eCS.Property(x => x.problema_presentado).IsModified = false;
                    eCS.Property(x => x.idUsuario).IsModified = false;
                    eCS.Property(x => x.idDpto).IsModified = false;
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
                    this.idReportes = id;
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

                    var query = ctx.reportes.Include("dptos")
                                            .Include("usuarios")
                                            .Where(x => x.idReportes > 0)
                                            .Where(x => x.estatus == "1");





                    // Ordenamiento
                    if (grid.columna == "idReportes")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.idReportes)
                                                             : query.OrderBy(x => x.idReportes);
                    }
                    if (grid.columna == "reportado_por")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.reportado_por)
                                                             : query.OrderBy(x => x.reportado_por);
                    }
                    if (grid.columna == "nombre_dpto")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.dptos.nombre_dpto)
                                                             : query.OrderBy(x => x.dptos.nombre_dpto);
                    }
                    if (grid.columna == "fecha_inicio")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.fecha_inicio)
                                                             : query.OrderBy(x => x.fecha_inicio);
                    }
                    if (grid.columna == "problema_presentado")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.problema_presentado)
                                                             : query.OrderBy(x => x.problema_presentado);
                    }
                    if (grid.columna == "nombre")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.usuarios.nombre)
                                                             : query.OrderBy(x => x.usuarios.nombre);
                    }

                    foreach (var f in grid.filtros)
                    {
                        if (f.columna == "reportado_por")
                            query = query.Where(x => x.reportado_por.StartsWith(f.valor));
                        if (f.columna == "nombre_dpto")
                            query = query.Where(x => x.dptos.nombre_dpto.StartsWith(f.valor));
                        if (f.columna == "problema_presentado")
                            query = query.Where(x => x.problema_presentado.StartsWith(f.valor));
                        if (f.columna == "nombre")
                            query = query.Where(x => x.usuarios.nombre.StartsWith(f.valor));

                    }


                    var reportes = query.Skip(grid.pagina)
                                       .Take(grid.limite)
                                       .ToList();

                    var total = query.Count();


                    grid.SetData(
                        from a in reportes
                        select new
                        {
                            a.idReportes,
                            a.reportado_por,
                            a.dptos.nombre_dpto,
                            a.fecha_inicio,
                            a.problema_presentado,
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

                    var query = ctx.reportes.Include("dptos")
                                            .Include("usuarios")
                                            .Where(x => x.idReportes > 0)
                                            .Where(x => x.estatus == "2");




                    // Ordenamiento
                    if (grid.columna == "idReportes")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.idReportes)
                                                             : query.OrderBy(x => x.idReportes);
                    }
                    if (grid.columna == "reportado_por")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.reportado_por)
                                                             : query.OrderBy(x => x.reportado_por);
                    }
                    if (grid.columna == "nombre_dpto")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.dptos.nombre_dpto)
                                                             : query.OrderBy(x => x.dptos.nombre_dpto);
                    }
                    if (grid.columna == "fecha_inicio")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.fecha_inicio)
                                                             : query.OrderBy(x => x.fecha_inicio);
                    }
                    if (grid.columna == "problema_presentado")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.problema_presentado)
                                                             : query.OrderBy(x => x.problema_presentado);
                    }
                    if (grid.columna == "nombre")
                    {
                        query = grid.columna_orden == "DESC" ? query.OrderByDescending(x => x.usuarios.nombre)
                                                             : query.OrderBy(x => x.usuarios.nombre);
                    }

                    foreach (var f in grid.filtros)
                    {
                        if (f.columna == "reportado_por")
                            query = query.Where(x => x.reportado_por.StartsWith(f.valor));
                        if (f.columna == "nombre_dpto")
                            query = query.Where(x => x.dptos.nombre_dpto.StartsWith(f.valor));
                        if (f.columna == "problema_presentado")
                            query = query.Where(x => x.problema_presentado.StartsWith(f.valor));
                        if (f.columna == "nombre")
                            query = query.Where(x => x.usuarios.nombre.StartsWith(f.valor));
                    }

                    var reportes = query.Skip(grid.pagina)
                                   .Take(grid.limite)
                                   .ToList();

                    var total = query.Count();


                    grid.SetData(
                        from a in reportes
                        select new
                        {
                            a.idReportes,
                            a.reportado_por,
                            a.dptos.nombre_dpto,
                            a.fecha_inicio,
                            a.problema_presentado,
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


        //reportes

        public int CantidadReportesAbiertos(DateTime inicio, DateTime fin)
        {
            int conteo;
            try
            {
                using (var ctx = new SistemaContext())
                {
                    if (inicio == fin)
                    {
                        conteo = ctx.reportes.Where(x => DbFunctions.TruncateTime(x.fecha_inicio) == DbFunctions.TruncateTime(inicio))
                                    .Count(x => x.estatus == "1");
                    }
                    else
                    {
                        conteo = ctx.reportes.Where(x => DbFunctions.TruncateTime(x.fecha_inicio) >= DbFunctions.TruncateTime(inicio) && DbFunctions.TruncateTime(x.fecha_inicio) <= DbFunctions.TruncateTime(fin))
                                    .Count(x => x.estatus == "1");
                    }


                }
            }
            catch (Exception)
            {

                throw;
            }
            return conteo;
        }

        public int CantidadReportesCerrados(DateTime inicio, DateTime fin)
        {
            int conteo;
            var reporte = new reportes();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    if (inicio == fin)
                    {

                        conteo = ctx.reportes.Where(x => DbFunctions.TruncateTime(x.fecha_inicio) == DbFunctions.TruncateTime(inicio))
                                        .Count(x => x.estatus == "2");
                    }
                    else
                    {
                        conteo = ctx.reportes.Where(x => DbFunctions.TruncateTime(x.fecha_inicio) >= DbFunctions.TruncateTime(inicio) && DbFunctions.TruncateTime(x.fecha_inicio) <= DbFunctions.TruncateTime(fin))
                                         .Count(x => x.estatus == "2");
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return conteo;
        }

        public int CantidadReportesCerradosTecnicoTotal(int id)
        {
            int conteo;
            var reporte = new reportes();
            try
            {
                using (var ctx = new SistemaContext())
                {                 
                        conteo = ctx.reportes.Where(x => x.idUsuario == id)
                        .Count(x => x.estatus == "2");
                }
            }
            catch (Exception)
            {

                throw;
            }
            return conteo;
        }

        public List<dptoConsulta> CantidadReportesAbiertosDpto(DateTime inicio, DateTime fin)
        { 

            try
            {
                using (var ctx = new SistemaContext())
                {
                    if (inicio == fin)
                    {
                        var query = from a in ctx.reportes
                                    where a.estatus == "1"
                                    where inicio == a.fecha_inicio
                                    join aa in ctx.dptos
                                    on a.idDpto equals aa.idDpto
                                    group a by a.dptos.nombre_dpto into g

                                    select new dptoConsulta
                                    {
                                        nombre = g.Key,
                                        cantidad = g.Count()
                                    };

                        return query.ToList();
                    }
                    else
                    {
                        var query = from a in ctx.reportes
                                    where a.estatus == "1"
                                    where a.fecha_inicio >= inicio && a.fecha_inicio <= fin
                                    join aa in ctx.dptos
                                    on a.idDpto equals aa.idDpto
                                    group a by a.dptos.nombre_dpto into g

                                    select new dptoConsulta
                                    {
                                        nombre = g.Key,
                                        cantidad = g.Count()
                                    };

                        return query.ToList();
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public List<dptoConsulta> CantidadReportesCerradosDpto(DateTime inicio, DateTime fin)
        {

            try
            {
                using (var ctx = new SistemaContext())
                {
                    if (inicio == fin)
                    {
                        var query = from a in ctx.reportes
                                    where a.estatus == "2"
                                    where inicio == a.fecha_cierre
                                    join aa in ctx.dptos
                                    on a.idDpto equals aa.idDpto
                                    group a by a.dptos.nombre_dpto into g

                                    select new dptoConsulta
                                    {
                                        nombre = g.Key,
                                        cantidad = g.Count()
                                    };

                        return query.ToList();
                    }
                    else
                    {
                        var query = from a in ctx.reportes
                                    where a.estatus == "2"
                                    where a.fecha_cierre >= inicio && a.fecha_cierre <= fin
                                    join aa in ctx.dptos
                                    on a.idDpto equals aa.idDpto
                                    group a by a.dptos.nombre_dpto into g

                                    select new dptoConsulta
                                    {
                                        nombre = g.Key,
                                        cantidad = g.Count()
                                    };

                        return query.ToList();
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<dptoConsulta> CantidadReportesCerradosPorTecnico(DateTime inicio, DateTime fin)
        {

            try
            {
                using (var ctx = new SistemaContext())
                {
                    if (inicio == fin)
                    {
                        var query = from a in ctx.reportes
                                    where a.estatus == "2"
                                    where inicio == a.fecha_cierre
                                    join aa in ctx.usuarios
                                    on a.idUsuario equals aa.idUsuario
                                    group a by a.usuarios.nombre into g

                                    select new dptoConsulta
                                    {
                                        nombre = g.Key,
                                        cantidad = g.Count()
                                    };

                        return query.ToList();
                    }
                    else
                    {
                        var query = from a in ctx.reportes
                                    where a.estatus == "2"
                                    where a.fecha_cierre >= inicio && a.fecha_cierre <= fin
                                    join aa in ctx.usuarios
                                    on a.idUsuario equals aa.idUsuario
                                    group a by a.usuarios.nombre into g

                                    select new dptoConsulta
                                    {
                                        nombre = g.Key,
                                        cantidad = g.Count()
                                    };

                        return query.ToList();
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public int CantidadPrestamosAbiertos(DateTime inicio, DateTime fin)
        {
            int conteo;
            try
            {
                using (var ctx = new SistemaContext())
                {
                    if (inicio == fin)
                    {
                        conteo = ctx.prestamos.Where(x => DbFunctions.TruncateTime(x.fecha_entrega) == DbFunctions.TruncateTime(inicio))
                                    .Count(x => x.estatus == "1");
                    }
                    else
                    {
                        conteo = ctx.prestamos.Where(x => DbFunctions.TruncateTime(x.fecha_entrega) >= DbFunctions.TruncateTime(inicio) && DbFunctions.TruncateTime(x.fecha_entrega) <= DbFunctions.TruncateTime(fin))
                                    .Count(x => x.estatus == "1");
                    }


                }
            }
            catch (Exception)
            {

                throw;
            }
            return conteo;
        }

        public int CantidadPrestamosCerrados(DateTime inicio, DateTime fin)
        {
            int conteo;
            try
            {
                using (var ctx = new SistemaContext())
                {
                    if (inicio == fin)
                    {
                        conteo = ctx.prestamos.Where(x => DbFunctions.TruncateTime(x.fecha_entrega) == DbFunctions.TruncateTime(inicio))
                                    .Count(x => x.estatus == "2");
                    }
                    else
                    {
                        conteo = ctx.prestamos.Where(x => DbFunctions.TruncateTime(x.fecha_entrega) >= DbFunctions.TruncateTime(inicio) && DbFunctions.TruncateTime(x.fecha_entrega) <= DbFunctions.TruncateTime(fin))
                                    .Count(x => x.estatus == "2");
                    }


                }
            }
            catch (Exception)
            {

                throw;
            }
            return conteo;
        }

    }
}
