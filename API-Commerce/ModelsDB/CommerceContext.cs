using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_Commerce.ModelsDB;

public partial class CommerceContext : DbContext
{
    public CommerceContext()
    {
    }

    public CommerceContext(DbContextOptions<CommerceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Businessman> Businessmen { get; set; }

    public virtual DbSet<BusinessmanStatus> BusinessmanStatuses { get; set; }

    public virtual DbSet<Establishment> Establishments { get; set; }

    public virtual DbSet<Municipality> Municipalities { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Businessman>(entity =>
        {
            entity.HasKey(e => e.BusId).HasName("PK__Business__B0524D19B4CA8A7F");

            entity.ToTable("Businessman", tb => tb.HasTrigger("trg_Businessman_Update"));

            entity.Property(e => e.BusId).HasColumnName("Bus_Id");
            entity.Property(e => e.BusDateRegistration).HasColumnName("Bus_Date_Registration");
            entity.Property(e => e.BusDateUpdate).HasColumnName("Bus_Date_Update");
            entity.Property(e => e.BusEmail)
                .HasMaxLength(40)
                .HasColumnName("Bus_Email");
            entity.Property(e => e.BusMunicipality)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Bus_Municipality");
            entity.Property(e => e.BusName)
                .HasMaxLength(40)
                .HasColumnName("Bus_Name");
            entity.Property(e => e.BusPhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("Bus_Phone_Number");
            entity.Property(e => e.BusStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Bus_Status");
            entity.Property(e => e.BusUserUpdate)
                .HasMaxLength(30)
                .HasColumnName("Bus_User_Update");

            entity.HasOne(d => d.BusMunicipalityNavigation).WithMany(p => p.Businessmen)
                .HasForeignKey(d => d.BusMunicipality)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Businessman_Municipality");

            entity.HasOne(d => d.BusStatusNavigation).WithMany(p => p.Businessmen)
                .HasForeignKey(d => d.BusStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Businessman_Status");
        });

        modelBuilder.Entity<BusinessmanStatus>(entity =>
        {
            entity.HasKey(e => e.BstId).HasName("PK__Business__ECA85BCFD038EEE2");

            entity.ToTable("Businessman_Status");

            entity.Property(e => e.BstId)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Bst_Id");
            entity.Property(e => e.BstDescription)
                .HasMaxLength(10)
                .HasColumnName("Bst_Description");
        });

        modelBuilder.Entity<Establishment>(entity =>
        {
            entity.HasKey(e => e.EstId).HasName("PK__Establis__345473BCD5E131F8");

            entity.ToTable("Establishment", tb => tb.HasTrigger("trg_Establishment_Update"));

            entity.Property(e => e.EstId).HasColumnName("Est_Id");
            entity.Property(e => e.EstBusinessman).HasColumnName("Est_Businessman");
            entity.Property(e => e.EstDateUpdate).HasColumnName("Est_Date_Update");
            entity.Property(e => e.EstEmployees).HasColumnName("Est_Employees");
            entity.Property(e => e.EstMoneyIncome)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Est_Money_Income");
            entity.Property(e => e.EstNombre)
                .HasMaxLength(30)
                .HasColumnName("Est_Nombre");
            entity.Property(e => e.EstUserUpdate)
                .HasMaxLength(30)
                .HasColumnName("Est_User_Update");

            entity.HasOne(d => d.EstBusinessmanNavigation).WithMany(p => p.Establishments)
                .HasForeignKey(d => d.EstBusinessman)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Establishment_Businessman");
        });

        modelBuilder.Entity<Municipality>(entity =>
        {
            entity.HasKey(e => e.MunId).HasName("PK__Municipa__106B04B90BE6A317");

            entity.ToTable("Municipality");

            entity.Property(e => e.MunId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Mun_Id");
            entity.Property(e => e.MunNombre)
                .HasMaxLength(30)
                .HasColumnName("Mun_Nombre");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Rol__795EBD495FAA2E4A");

            entity.ToTable("Rol");

            entity.Property(e => e.RolId).HasColumnName("Rol_Id");
            entity.Property(e => e.RolDescription)
                .HasMaxLength(30)
                .HasColumnName("Rol_Description");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UseId).HasName("PK__User__09268C61179E72DE");

            entity.ToTable("User");

            entity.Property(e => e.UseId).HasColumnName("Use_Id");
            entity.Property(e => e.UseEmail)
                .HasMaxLength(40)
                .HasColumnName("Use_Email");
            entity.Property(e => e.UseFirstLastName)
                .HasMaxLength(20)
                .HasColumnName("Use_First_Last_Name");
            entity.Property(e => e.UseFirstName)
                .HasMaxLength(20)
                .HasColumnName("Use_First_Name");
            entity.Property(e => e.UseMiddleLastName)
                .HasMaxLength(20)
                .HasColumnName("Use_Middle_Last_Name");
            entity.Property(e => e.UseMiddleName)
                .HasMaxLength(20)
                .HasColumnName("Use_Middle_Name");
            entity.Property(e => e.UsePassword)
                .HasMaxLength(40)
                .HasColumnName("Use_Password");
            entity.Property(e => e.UseRol).HasColumnName("Use_Rol");

            entity.HasOne(d => d.UseRolNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UseRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
