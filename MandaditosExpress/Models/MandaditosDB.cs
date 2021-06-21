using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MandaditosExpress.Models
{
    public class MandaditosDB:DbContext
    {
        public MandaditosDB() : base("name=DataConnection")
        {
        }

        public virtual DbSet<Pago> Pagos { get; set; }
        public virtual DbSet<TipoDePago> TipoDePagos { get; set; }
        public virtual DbSet<Moneda> Monedas { get; set; }
        public virtual DbSet<Motocicleta> Motocicletas { get; set; }
        public virtual DbSet<Envio> Envios { get; set; }
        public virtual DbSet<Seguimiento> Seguimientos { get; set; }
        public virtual DbSet<DetalleDeSeguimiento> DetallesDeSeguimiento { get; set; }
        public virtual DbSet<HistorialDetalleSeguimiento> HistorialDeSeguimientos { get; set; }
        public virtual DbSet<Cotizacion> Cotizaciones { get; set; }
        public virtual DbSet<Costo> Costos { get; set; }
        public virtual DbSet<TipoDeServicio> TipoDeServicioSet { get; set; }
        public virtual DbSet<Salario> Salarios { get; set; }
        public virtual DbSet<Servicio> Gestiones { get; set; }
        public virtual DbSet<Credito> Creditos { get; set; }
        public virtual DbSet<HistorialSeguimiento> HistorialSeguimientos { get; set; }
        public virtual DbSet<Persona> Personas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public System.Data.Entity.DbSet<MandaditosExpress.Models.Cliente> Clientes { get; set; }

        public System.Data.Entity.DbSet<MandaditosExpress.Models.Asistente> Asistentes { get; set; }

        public System.Data.Entity.DbSet<MandaditosExpress.Models.Motorizado> Motorizadoes { get; set; }
    }
}