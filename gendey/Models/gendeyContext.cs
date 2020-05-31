using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace gendey.Models
{
    public partial class gendeyContext : DbContext
    {
        public gendeyContext()
        {
        }

        public gendeyContext(DbContextOptions<gendeyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Adress> Adress { get; set; }
        public virtual DbSet<AttendantServiceRel> AttendantServiceRel { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<ConfAttendant> ConfAttendant { get; set; }
        public virtual DbSet<ConfClient> ConfClient { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<ScheduleConfig> ScheduleConfig { get; set; }
        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Database=gendey;Username=postgres;Password=admin");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adress>(entity =>
            {
                entity.ToTable("adress");

                entity.HasComment("endereço do usuário");

                entity.HasIndex(e => e.Id)
                    .HasName("adress_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Complement)
                    .HasColumnName("complement")
                    .HasMaxLength(100);

                entity.Property(e => e.FkCity).HasColumnName("fk_city");

                entity.Property(e => e.Nbhood)
                    .HasColumnName("nbhood")
                    .HasMaxLength(40);

                entity.Property(e => e.Num).HasColumnName("num");

                entity.Property(e => e.Postalcode).HasColumnName("postalcode");

                entity.Property(e => e.RegisterDate)
                    .HasColumnName("register_date")
                    .HasColumnType("date");

                entity.Property(e => e.Street)
                    .HasColumnName("street")
                    .HasMaxLength(40);

                entity.HasOne(d => d.FkCityNavigation)
                    .WithMany(p => p.Adress)
                    .HasForeignKey(d => d.FkCity)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("adress_city_fk");
            });

            modelBuilder.Entity<AttendantServiceRel>(entity =>
            {
                entity.ToTable("attendant_service_rel");

                entity.HasComment("Relation between attendant and his services");

                entity.HasIndex(e => e.Id)
                    .HasName("docsec_rel_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('attendantservice_rel_id_seq'::regclass)");

                entity.Property(e => e.AttendantId).HasColumnName("attendant_id");

                entity.Property(e => e.ServiceId).HasColumnName("service_id");

                entity.HasOne(d => d.Attendant)
                    .WithMany(p => p.AttendantServiceRelAttendant)
                    .HasForeignKey(d => d.AttendantId)
                    .HasConstraintName("docsec_rel_user_doc_id_fk");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.AttendantServiceRelService)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("docsec_rel_user_sec_id_fk");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.IbgeCode).HasColumnName("ibge_code");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.Uf)
                    .HasColumnName("uf")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<ConfAttendant>(entity =>
            {
                entity.ToTable("conf_attendant");

                entity.HasComment("Attendant configurations");

                entity.HasIndex(e => e.Id)
                    .HasName("conf_medic_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.RegisterDate)
                    .HasColumnName("register_date")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<ConfClient>(entity =>
            {
                entity.ToTable("conf_client");

                entity.HasComment("Configurações do paciente");

                entity.HasIndex(e => e.Id)
                    .HasName("conf_patient_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RegisterDate)
                    .HasColumnName("register_date")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("contact");

                entity.HasComment("contact de usuário");

                entity.HasIndex(e => e.Id)
                    .HasName("contact_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Fone)
                    .HasColumnName("fone")
                    .HasMaxLength(16);

                entity.Property(e => e.Fone2)
                    .HasColumnName("fone2")
                    .HasMaxLength(16);

                entity.Property(e => e.FoneAlt)
                    .HasColumnName("fone_alt")
                    .HasMaxLength(16);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.NameAlt)
                    .HasColumnName("name_alt")
                    .HasColumnType("character varying");

                entity.Property(e => e.RegisterDate)
                    .HasColumnName("register_date")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.HasComment("Identificadores de role");

                entity.HasIndex(e => e.Id)
                    .HasName("roles_id_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .HasName("roles_name_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("schedule");

                entity.HasIndex(e => e.Id)
                    .HasName("schedule_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AppointmentDate)
                    .HasColumnName("appointment_date")
                    .HasColumnType("date");

                entity.Property(e => e.AttendantId).HasColumnName("attendant_id");

                entity.Property(e => e.Canceled).HasColumnName("canceled");

                entity.Property(e => e.CanceledReason)
                    .HasColumnName("canceled_reason")
                    .HasColumnType("character varying");

                entity.Property(e => e.ClientId).HasColumnName("client_id");

                entity.Property(e => e.EndTime)
                    .HasColumnName("end_time")
                    .HasColumnType("time without time zone");

                entity.Property(e => e.Observation).HasColumnName("observation");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("money");

                entity.Property(e => e.RegisterDate)
                    .HasColumnName("register_date")
                    .HasColumnType("date");

                entity.Property(e => e.StartTime)
                    .HasColumnName("start_time")
                    .HasColumnType("time without time zone");

                entity.HasOne(d => d.Attendant)
                    .WithMany(p => p.ScheduleAttendant)
                    .HasForeignKey(d => d.AttendantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("schedule_user_doctor_id_fk");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ScheduleClient)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("schedule_user_patient_id_fk");
            });

            modelBuilder.Entity<ScheduleConfig>(entity =>
            {
                entity.ToTable("schedule_config");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DayOfWeek).HasColumnName("day_of_week");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.EndTime)
                    .HasColumnName("end_time")
                    .HasColumnType("time without time zone");

                entity.Property(e => e.StartTime)
                    .HasColumnName("start_time")
                    .HasColumnType("time without time zone");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ScheduleConfig)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("schedules_config_user_id_fk");
            });

            modelBuilder.Entity<Services>(entity =>
            {
                entity.ToTable("services");

                entity.HasComment("expertises medicas");

                entity.HasIndex(e => e.Id)
                    .HasName("expertise_id_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .HasName("expertise_name_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("session");

                entity.HasIndex(e => e.RefreshTokenCode)
                    .HasName("session_refresh_token_code_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AuthDate)
                    .HasColumnName("auth_date")
                    .HasColumnType("date");

                entity.Property(e => e.LastToken)
                    .IsRequired()
                    .HasColumnName("last_token")
                    .HasColumnType("character varying");

                entity.Property(e => e.RefreshTokenCode)
                    .IsRequired()
                    .HasColumnName("refresh_token_code")
                    .HasColumnType("character varying");

                entity.Property(e => e.TokenRefreshDate)
                    .HasColumnName("token_refresh_date")
                    .HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Session)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("session_user_id_fk");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasComment("Login e fk's");

                entity.HasIndex(e => e.Email)
                    .HasName("user_email_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.FkAdress)
                    .HasName("user_fk_adress_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.FkConfAttendant)
                    .HasName("user_fk_confmedico_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.FkConfClient)
                    .HasName("user_fk_confpaciente_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.FkContact)
                    .HasName("user_fk_contact_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Id)
                    .HasName("user_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Birthdate)
                    .HasColumnName("birthdate")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("character varying");

                entity.Property(e => e.FkAdress).HasColumnName("fk_adress");

                entity.Property(e => e.FkConfAttendant).HasColumnName("fk_conf_attendant");

                entity.Property(e => e.FkConfClient).HasColumnName("fk_conf_client");

                entity.Property(e => e.FkContact).HasColumnName("fk_contact");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("character varying");

                entity.Property(e => e.RegisterDate)
                    .HasColumnName("register_date")
                    .HasColumnType("date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.FkAdressNavigation)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.FkAdress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_adress_fk");

                entity.HasOne(d => d.FkConfAttendantNavigation)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.FkConfAttendant)
                    .HasConstraintName("user_conf_attendant_fk");

                entity.HasOne(d => d.FkConfClientNavigation)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.FkConfClient)
                    .HasConstraintName("user_conf_patient_id_fk");

                entity.HasOne(d => d.FkContactNavigation)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.FkContact)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_contact_fk");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_roles_id_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
