using DynamicForm.API.Models.SubmissionFolder;
using DynamicForm.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Form> Forms { get; set; }
    public DbSet<Field> Fields { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<SubmissionAnswer> SubmissionAnswers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //one to many relationship between Form and the others, and submisson and submission answer.
        modelBuilder.Entity<Form>()
            .HasMany(f => f.Fields)
            .WithOne()
            .HasForeignKey(f => f.FormId);

        modelBuilder.Entity<Form>()
            .HasMany(f => f.Submissions)
            .WithOne()
            .HasForeignKey(s => s.FormId);

        modelBuilder.Entity<Submission>()
            .HasMany(s => s.Answers)
            .WithOne()
            .HasForeignKey(a => a.SubmissionId);
    }


}
