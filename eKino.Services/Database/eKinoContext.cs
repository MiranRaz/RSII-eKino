using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace eKino.Services.Database
{
    public partial class eKinoContext : DbContext
    {
        public eKinoContext()
        {
        }

        public eKinoContext(DbContextOptions<eKinoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Auditorium> Auditoria { get; set; } = null!;
        public virtual DbSet<Director> Directors { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<IsDeleted> IsDeleteds { get; set; } = null!;
        public virtual DbSet<Movie> Movies { get; set; } = null!;
        public virtual DbSet<MovieGenre> MovieGenres { get; set; } = null!;
        public virtual DbSet<Projection> Projections { get; set; } = null!;
        public virtual DbSet<Rating> Ratings { get; set; } = null!;
        public virtual DbSet<Reservation> Reservations { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=ekino-sql-mr,1433;Database=eKino;User=sa;Password=admin;ConnectRetryCount=0");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auditorium>(entity =>
            {
                entity.ToTable("Auditorium");

                entity.Property(e => e.AuditoriumId)
                    .ValueGeneratedNever()
                    .HasColumnName("AuditoriumID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Director>(entity =>
            {
                entity.ToTable("Director");

                entity.Property(e => e.DirectorId)
                    .ValueGeneratedNever()
                    .HasColumnName("DirectorID");

                entity.Property(e => e.Biography).HasMaxLength(500);

                entity.Property(e => e.FullName).HasMaxLength(50);
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("Genre");

                entity.Property(e => e.GenreId)
                    .ValueGeneratedNever()
                    .HasColumnName("GenreID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<IsDeleted>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("IsDeleted");

                entity.Property(e => e.IsDeleted1).HasColumnName("isDeleted");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("Movie");

                entity.HasIndex(e => e.DirectorId, "IX_Movie_DirectorID");

                entity.Property(e => e.MovieId)
                    .ValueGeneratedNever()
                    .HasColumnName("MovieID");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.DirectorId).HasColumnName("DirectorID");

                entity.Property(e => e.RunningTime).HasMaxLength(30);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.Property(e => e.Year).HasColumnType("date");

                //entity.HasOne(d => d.Director)
                //    .WithMany(p => p.Movies)
                //    .HasForeignKey(d => d.DirectorId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Movie_Director");
            });

            modelBuilder.Entity<MovieGenre>(entity =>
            {
                entity.ToTable("MovieGenre");

                entity.HasIndex(e => e.GenreId, "IX_MovieGenre_GenreID");

                entity.HasIndex(e => e.MovieId, "IX_MovieGenre_MovieID");

                entity.Property(e => e.MovieGenreId)
                    .ValueGeneratedNever()
                    .HasColumnName("MovieGenreID");

                entity.Property(e => e.GenreId).HasColumnName("GenreID");

                entity.Property(e => e.MovieId).HasColumnName("MovieID");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.MovieGenres)
                    .HasForeignKey(d => d.GenreId);

                //entity.HasOne(d => d.Movie)
                //    .WithMany(p => p.MovieGenres)
                //    .HasForeignKey(d => d.MovieId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_MovieGenre_Movie");
            });

            modelBuilder.Entity<Projection>(entity =>
            {
                entity.ToTable("Projection");

                entity.HasIndex(e => e.AuditoriumId, "IX_Projection_AuditoriumID");

                entity.HasIndex(e => e.MovieId, "IX_Projection_MovieID");

                entity.Property(e => e.ProjectionId)
                    .ValueGeneratedNever()
                    .HasColumnName("ProjectionID");

                entity.Property(e => e.AuditoriumId).HasColumnName("AuditoriumID");

                entity.Property(e => e.DateOfProjection).HasColumnType("date");

                entity.Property(e => e.MovieId).HasColumnName("MovieID");

                entity.Property(e => e.TicketPrice).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Auditorium)
                    .WithMany(p => p.Projections)
                    .HasForeignKey(d => d.AuditoriumId);

                //entity.HasOne(d => d.Movie)
                //    .WithMany(p => p.Projections)
                //    .HasForeignKey(d => d.MovieId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Projection_Movie");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("Rating");

                entity.HasIndex(e => e.MovieId, "IX_Rating_MovieID");

                entity.HasIndex(e => e.UserId, "IX_Rating_UserID");

                entity.Property(e => e.RatingId)
                    .ValueGeneratedNever()
                    .HasColumnName("RatingID");

                entity.Property(e => e.DateOfRating).HasColumnType("date");

                entity.Property(e => e.MovieId).HasColumnName("MovieID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                //entity.HasOne(d => d.Movie)
                //    .WithMany(p => p.Ratings)
                //    .HasForeignKey(d => d.MovieId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Rating_Movie");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("Reservation");

                entity.HasIndex(e => e.ProjectionId, "IX_Reservation_ProjectionID");

                entity.HasIndex(e => e.UserId, "IX_Reservation_UserID");

                entity.Property(e => e.ReservationId)
                    .ValueGeneratedNever()
                    .HasColumnName("ReservationID");

                entity.Property(e => e.Column).HasMaxLength(50);

                entity.Property(e => e.DateOfReservation).HasColumnType("date");

                entity.Property(e => e.NumTicket).HasMaxLength(50);

                entity.Property(e => e.ProjectionId).HasColumnName("ProjectionID");

                entity.Property(e => e.Row).HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Projection)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.ProjectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reservation_Projection");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoleID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.HasIndex(e => e.ReservationId, "IX_Transaction_ReservationID");

                entity.HasIndex(e => e.UserId, "IX_Transaction_UserID");

                entity.Property(e => e.TransactionId)
                    .ValueGeneratedNever()
                    .HasColumnName("TransactionID");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DateOfTransaction).HasColumnType("date");

                entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.ReservationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Reservation");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserID");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.PasswordHash).HasMaxLength(50);

                entity.Property(e => e.PasswordSalt).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");

                entity.HasIndex(e => e.RoleId, "IX_UserRole_RoleID");

                entity.HasIndex(e => e.UserId, "IX_UserRole_UserID");

                entity.Property(e => e.UserRoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserRoleID");

                entity.Property(e => e.DateModified).HasColumnType("date");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
