using Microsoft.EntityFrameworkCore;

#nullable disable

namespace TuChanceTest_ASP.Net_Core_3_1.Models
{
    public partial class TuChanceTestContext : DbContext
    {
        public TuChanceTestContext()
        {
        }

        public TuChanceTestContext(DbContextOptions<TuChanceTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<SurveyQuestion> SurveyQuestions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestionAnswer>(entity =>
            {
                entity.HasKey(e => e.IdAnswer);

                entity.ToTable("QuestionAnswer");

                entity.Property(e => e.Answer)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdQuestionNavigation)
                    .WithMany(p => p.QuestionAnswers)
                    .HasForeignKey(d => d.IdQuestion)
                    .HasConstraintName("FK_QuestionAnswer_SurveyQuestion");
            });

            modelBuilder.Entity<Survey>(entity =>
            {
                entity.HasKey(e => e.IdSurvey);

                entity.ToTable("Survey");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SurveyQuestion>(entity =>
            {
                entity.HasKey(e => e.IdQuestion);

                entity.ToTable("SurveyQuestion");

                entity.Property(e => e.Question)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdSurveyNavigation)
                    .WithMany(p => p.SurveyQuestions)
                    .HasForeignKey(d => d.IdSurvey)
                    .HasConstraintName("FK_SurveyQuestion_Survey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
