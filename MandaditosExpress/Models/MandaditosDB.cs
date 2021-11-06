using MandaditosExpress.Models.ViewModels;
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
        public virtual DbSet<TipoDePago> TiposDePago { get; set; }
        public virtual DbSet<Moneda> Monedas { get; set; }
        public virtual DbSet<Motocicleta> Motocicletas { get; set; }
        public virtual DbSet<Lugar> Lugares { get; set; }
        public virtual DbSet<Envio> Envios { get; set; }
        public virtual DbSet<Seguimiento> Seguimientos { get; set; }
        public virtual DbSet<DetalleDeSeguimiento> DetallesDeSeguimiento { get; set; }
        public virtual DbSet<HistorialDetalleSeguimiento> HistorialDeSeguimientos { get; set; }
        public virtual DbSet<Cotizacion> Cotizaciones { get; set; }
        public virtual DbSet<Costo> Costos { get; set; }
        public virtual DbSet<TipoDeServicio> TiposDeServicio { get; set; }
        public virtual DbSet<Salario> Salarios { get; set; }
        public virtual DbSet<Servicio> Servicios { get; set; }
        public virtual DbSet<Credito> Creditos { get; set; }
        public virtual DbSet<HistorialSeguimiento> HistorialSeguimientos { get; set; }
        public virtual DbSet<Disponibilidad> Disponibilidad { get; set; }
        public virtual DbSet<VelocidadDeConexion> VelocidadDeConexion { get; set; }
        public virtual DbSet<CostoGestionBancaria> CostoGestionBancaria { get; set; }
        public virtual DbSet<Persona> Personas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Cotizacion>().Property(x => x.MontoTotal).HasPrecision(7, 2);
            modelBuilder.Entity<CotizacionViewModel>().Property(x => x.MontoTotal).HasPrecision(7, 2);//establecerle la precision al correspóndiente viewmodel de la Cotizacion

            modelBuilder.Entity<Cotizacion>().Property(x => x.MontoDeDinero).HasPrecision(7, 2);
            modelBuilder.Entity<CotizacionViewModel>().Property(x => x.MontoDeDinero).HasPrecision(7, 2);//establecerle la precision al correspóndiente viewmodel de la Cotizacion

            modelBuilder.Entity<CostoGestionBancaria>().Property(x => x.MontoDesde).HasPrecision(7, 2);
            modelBuilder.Entity<CostoGestionBancaria>().Property(x => x.MontoDesde).HasPrecision(7, 2);//establecerle la precision al correspóndiente viewmodel de la Cotizacion

            modelBuilder.Entity<CostoGestionBancaria>().Property(x => x.MontoHasta).HasPrecision(7, 2);
            modelBuilder.Entity<CostoGestionBancaria>().Property(x => x.MontoHasta).HasPrecision(7, 2);//establecerle la precision al correspóndiente viewmodel de la Cotizacion

        }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Asistente> Asistentes { get; set; }

        public DbSet<Motorizado> Motorizados { get; set; }
    }
}