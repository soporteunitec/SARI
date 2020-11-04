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

    public partial class usuarios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public usuarios()
        {
            inventarios = new HashSet<inventarios>();
            reportes = new HashSet<reportes>();
            vitacora = new HashSet<vitacora>();
        }

        [Key]
        public int idUsuario { get; set; }

        [StringLength(50)]
        public string nombre { get; set; }

        [StringLength(50)]
        public string apellido { get; set; }

        [StringLength(50)]
        public string correo { get; set; }

        [StringLength(50)]
        public string clave { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime fecha_n { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventarios> inventarios { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reportes> reportes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<vitacora> vitacora { get; set; }
        public ResponseModel Acceder(string Email, string Password)
        {
            var rm = new ResponseModel();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    Password = HashHelper.MD5(Password);
                    var usuario = ctx.usuarios.Where(x => x.correo == Email)
                                               .Where(x => x.clave == Password)
                                               .SingleOrDefault();

                    if (usuario != null)
                    {
                        SessionHelper.AddUserToSession(usuario.idUsuario.ToString());
                        rm.SetResponse(true);
                    }
                    else
                    {
                        rm.SetResponse(false, "Correo o contraseña incorrecta");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return rm;
        }

        public usuarios Obtener(int id)
        {
            var usuario = new usuarios();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    usuario = ctx.usuarios.Where(x => x.idUsuario == id)
                                          .SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return usuario;
        }

        public ResponseModel Guardar()
        {
            var rm = new ResponseModel();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    ctx.Configuration.ValidateOnSaveEnabled = false;
                    var usuario = ctx.Entry(this);
                    usuario.State = EntityState.Modified;
                    if (this.clave == null)
                    {
                        usuario.Property(x => x.clave).IsModified = false;
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

        public ResponseModel CambiarClave()
        {
            var rm = new ResponseModel();
            try
            {
                using (var ctx = new SistemaContext())
                {
                    ctx.Configuration.ValidateOnSaveEnabled = false;
                    var usuario = ctx.Entry(this);
                    this.clave = HashHelper.MD5(this.clave);
                    usuario.State = EntityState.Modified;
                    usuario.Property(x => x.nombre).IsModified = false;
                    usuario.Property(x => x.apellido).IsModified = false;
                    usuario.Property(x => x.correo).IsModified = false;
                    usuario.Property(x => x.fecha_n).IsModified = false;
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

        public List<usuarios> Listar()
        {
            var usuarios = new List<usuarios>();

            try
            {
                using (var ctx = new SistemaContext())
                {
                    usuarios = ctx.usuarios.OrderBy(x => x.idUsuario)
                                             .ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return usuarios;
        }
    }
}
