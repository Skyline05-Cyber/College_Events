using CollegeEventHub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CollegeEventHub.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<CollegeEvent> Events => Set<CollegeEvent>();
    public DbSet<EventCategory> EventCategories => Set<EventCategory>();
    public DbSet<Registration> Registrations => Set<Registration>();
    public DbSet<RegistrationFormField> RegistrationFormFields => Set<RegistrationFormField>();
    public DbSet<RegistrationResponse> RegistrationResponses => Set<RegistrationResponse>();
    public DbSet<DummyPayment> DummyPayments => Set<DummyPayment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<EventCategory>().HasIndex(c => c.Name).IsUnique();

        builder.Entity<CollegeEvent>()
            .HasOne(e => e.Category).WithMany(c => c.Events)
            .HasForeignKey(e => e.CategoryId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<CollegeEvent>()
            .HasOne(e => e.Organizer).WithMany()
            .HasForeignKey(e => e.OrganizerId).OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Registration>().HasIndex(r => new { r.StudentId, r.EventId }).IsUnique();
        builder.Entity<Registration>().HasOne(r => r.Student).WithMany()
            .HasForeignKey(r => r.StudentId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Registration>().HasOne(r => r.Event).WithMany(e => e.Registrations)
            .HasForeignKey(r => r.EventId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<RegistrationFormField>().HasOne(f => f.Event).WithMany(e => e.FormFields)
            .HasForeignKey(f => f.EventId).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RegistrationResponse>().HasIndex(r => new { r.RegistrationId, r.RegistrationFormFieldId }).IsUnique();
        builder.Entity<RegistrationResponse>().HasOne(r => r.Registration).WithMany(r => r.Responses)
            .HasForeignKey(r => r.RegistrationId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<RegistrationResponse>().HasOne(r => r.FormField).WithMany(f => f.Responses)
            .HasForeignKey(r => r.RegistrationFormFieldId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<DummyPayment>().HasIndex(p => p.TransactionReference).IsUnique();
        builder.Entity<DummyPayment>().HasOne(p => p.Registration).WithMany(r => r.Payments)
            .HasForeignKey(p => p.RegistrationId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<CollegeEvent>().Property(e => e.RegistrationFee).HasPrecision(10, 2);
        builder.Entity<DummyPayment>().Property(p => p.Amount).HasPrecision(10, 2);

        builder.Entity<EventCategory>().HasData(
            new EventCategory { Id = 1, Name = "Technical", Description = "Coding, robotics, and technology events" },
            new EventCategory { Id = 2, Name = "Cultural", Description = "Music, dance, and cultural competitions" },
            new EventCategory { Id = 3, Name = "Sports", Description = "Sports and athletic events" },
            new EventCategory { Id = 4, Name = "Academic", Description = "Seminars, workshops, and quizzes" }
        );
    }
}
