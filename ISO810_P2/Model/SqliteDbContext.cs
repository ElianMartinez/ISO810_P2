using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ISO810_P2.Model
{
   
        public class Nomina
        {
            public int Id { get; set; }
            public string TipoRegistro { get; set; }
            public string ClaveNomina { get; set; }
            public string TipoDocumento { get; set; }
            public string NumeroDocumento { get; set; }
            public string Nombres { get; set; }
            public string PApellido { get; set; }
            public string SApellido { get; set; }
            public string Sexo { get; set; }
            public string Salario_ISR { get; set; }
            public string Otros_ISR { get; set; }
            public string RNC_Agente { get; set; }
            public string Remuneracio_Otros_Empleados { get; set; }
            public string Ingresos_Externos_ISR { get; set; }
            public string Salario_Infotep { get; set; }
            public string Tipo_Ingreso { get; set; }
            public string Regalia_Pascual { get; set; }
            public string Preaviso { get; set; }
        }
       
    
    public class SqliteDBContext : DbContext
    {
        public virtual DbSet<Nomina> Nominas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=C:\DB\nomina" , option =>
            {
                option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Nomina>().ToTable(name: "Nominas");
            model.Entity<Nomina>(entity =>
            {
                entity.HasKey(k => k.Id);
            });
            base.OnModelCreating(model);
        }
    }
}
