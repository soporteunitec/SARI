namespace Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SistemaContext : DbContext
    {
        public SistemaContext()
            : base("name=SistemaContext")
        {
        }

        public virtual DbSet<AdjuntosDocumentacion> AdjuntosDocumentacion { get; set; }
        public virtual DbSet<documentacion> documentacion { get; set; }
        public virtual DbSet<dptos> dptos { get; set; }
        public virtual DbSet<inventarios> inventarios { get; set; }
        public virtual DbSet<reportes> reportes { get; set; }
        public virtual DbSet<tipo_falla> tipo_falla { get; set; }
        public virtual DbSet<tipoInventario> tipoInventario { get; set; }
        public virtual DbSet<usuarios> usuarios { get; set; }
        public virtual DbSet<vitacora> vitacora { get; set; }
        public virtual DbSet<prestamos> prestamos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdjuntosDocumentacion>()
                .Property(e => e.archivo)
                .IsUnicode(false);

            modelBuilder.Entity<documentacion>()
                .Property(e => e.nombre_documento)
                .IsUnicode(false);

            modelBuilder.Entity<documentacion>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<documentacion>()
                .HasMany(e => e.AdjuntosDocumentacion)
                .WithOptional(e => e.documentacion)
                .WillCascadeOnDelete();

            modelBuilder.Entity<dptos>()
                .Property(e => e.nombre_dpto)
                .IsUnicode(false);

            modelBuilder.Entity<inventarios>()
                .Property(e => e.nombre_inventario)
                .IsUnicode(false);

            modelBuilder.Entity<inventarios>()
                .Property(e => e.cantidad)
                .IsUnicode(false);

            modelBuilder.Entity<inventarios>()
                .Property(e => e.serial)
                .IsUnicode(false);

            modelBuilder.Entity<inventarios>()
                .Property(e => e.ubicacion)
                .IsUnicode(false);

            modelBuilder.Entity<reportes>()
                .Property(e => e.reportado_por)
                .IsUnicode(false);

            modelBuilder.Entity<reportes>()
                .Property(e => e.problema_presentado)
                .IsUnicode(false);

            modelBuilder.Entity<reportes>()
                .Property(e => e.falla)
                .IsUnicode(false);

            modelBuilder.Entity<reportes>()
                .Property(e => e.estatus)
                .IsUnicode(false);

            modelBuilder.Entity<tipo_falla>()
                .Property(e => e.nombre_falla)
                .IsUnicode(false);

            modelBuilder.Entity<tipoInventario>()
                .Property(e => e.nombre_tipo)
                .IsUnicode(false);

            modelBuilder.Entity<tipoInventario>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<usuarios>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<usuarios>()
                .Property(e => e.apellido)
                .IsUnicode(false);

            modelBuilder.Entity<usuarios>()
                .Property(e => e.correo)
                .IsUnicode(false);

            modelBuilder.Entity<usuarios>()
                .Property(e => e.clave)
                .IsUnicode(false);

            modelBuilder.Entity<vitacora>()
                .Property(e => e.descripcion)
                .IsUnicode(false);
        }
    }
}
