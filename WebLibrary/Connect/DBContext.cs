using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebLibrary.Models
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Chapter> Chapters { get; set; }
        public virtual DbSet<ChapterProgress> ChapterProgresses { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseProgress> CourseProgresses { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Instruct> Instructs { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<Learner> Learners { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<LessonProgress> LessonProgresses { get; set; }
        public virtual DbSet<Quiz> Quizzes { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }
        public virtual DbSet<VoucherUsage> VoucherUsages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-H5V6RB1O\\SQLEXPRESS;Database=Project_Group03;uid=sa;pwd=1234;encrypt=true;trustServerCertificate=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("admin");

                entity.Property(e => e.AdminId).HasColumnName("adminID");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("answer");

                entity.Property(e => e.AnswerId).HasColumnName("answerID");

                entity.Property(e => e.Answer1)
                    .HasMaxLength(1)
                    .HasColumnName("answer");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(255)
                    .HasColumnName("category_name");

                entity.Property(e => e.Description)
                    .HasColumnType("ntext")
                    .HasColumnName("description");
            });

            modelBuilder.Entity<Chapter>(entity =>
            {
                entity.ToTable("chapter");

                entity.Property(e => e.ChapterId).HasColumnName("chapterID");

                entity.Property(e => e.ChapterName)
                    .HasMaxLength(50)
                    .HasColumnName("chapter_name");

                entity.Property(e => e.CourseId).HasColumnName("courseID");

                entity.Property(e => e.Description)
                    .HasColumnType("ntext")
                    .HasColumnName("description");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.TotalTime)
                    .HasColumnName("total_time")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Chapters)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__chapter__courseI__4F7CD00D");
            });

            modelBuilder.Entity<ChapterProgress>(entity =>
            {
                entity.ToTable("chapter_progress");

                entity.Property(e => e.ChapterProgressId).HasColumnName("chapter_progressID");

                entity.Property(e => e.ChapterId).HasColumnName("chapterID");

                entity.Property(e => e.Completed)
                    .HasColumnName("completed")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CourseProgressId).HasColumnName("course_progressID");

                entity.Property(e => e.ProgressPercent)
                    .HasColumnName("progress_percent")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.StartAt)
                    .HasColumnType("datetime")
                    .HasColumnName("start_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TotalTime)
                    .HasColumnName("total_time")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.ChapterProgresses)
                    .HasForeignKey(d => d.ChapterId)
                    .HasConstraintName("FK__chapter_p__chapt__6EF57B66");

                entity.HasOne(d => d.CourseProgress)
                    .WithMany(p => p.ChapterProgresses)
                    .HasForeignKey(d => d.CourseProgressId)
                    .HasConstraintName("FK__chapter_p__cours__6FE99F9F");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("courses");

                entity.Property(e => e.CourseId).HasColumnName("courseID");

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.CourseName)
                    .HasMaxLength(255)
                    .HasColumnName("course_name");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("date")
                    .HasColumnName("creation_date");

                entity.Property(e => e.Description)
                    .HasColumnType("ntext")
                    .HasColumnName("description");

                entity.Property(e => e.Picture)
                    .HasColumnType("text")
                    .HasColumnName("picture");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.Status)
                    .HasMaxLength(30)
                    .HasColumnName("status");

                entity.Property(e => e.TotalTime)
                    .HasColumnName("total_time")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__courses__categor__46E78A0C");
            });

            modelBuilder.Entity<CourseProgress>(entity =>
            {
                entity.ToTable("course_progress");

                entity.Property(e => e.CourseProgressId).HasColumnName("course_progressID");

                entity.Property(e => e.Completed)
                    .HasColumnName("completed")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CourseId).HasColumnName("courseID");

                entity.Property(e => e.LearnerId).HasColumnName("learnerID");

                entity.Property(e => e.ProgressPercent)
                    .HasColumnName("progress_percent")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Rated)
                    .HasColumnName("rated")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.StartAt)
                    .HasColumnType("datetime")
                    .HasColumnName("start_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TotalTime)
                    .HasColumnName("total_time")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseProgresses)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__course_pr__cours__68487DD7");

                entity.HasOne(d => d.Learner)
                    .WithMany(p => p.CourseProgresses)
                    .HasForeignKey(d => d.LearnerId)
                    .HasConstraintName("FK__course_pr__learn__6754599E");
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.ToTable("enrollment");

                entity.Property(e => e.EnrollmentId).HasColumnName("enrollmentID");

                entity.Property(e => e.CourseId).HasColumnName("courseID");

                entity.Property(e => e.CourseName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("courseName");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.EnrollmentDate)
                    .HasColumnType("date")
                    .HasColumnName("enrollmentDate");

                entity.Property(e => e.IsPaid).HasColumnName("isPaid");

                entity.Property(e => e.LearnerId).HasColumnName("learnerID");

                entity.Property(e => e.LearnerName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("learnerName");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__enrollmen__cours__5DCAEF64");

                entity.HasOne(d => d.Learner)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.LearnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__enrollmen__learn__5CD6CB2B");
            });

            modelBuilder.Entity<Instruct>(entity =>
            {
                entity.ToTable("instruct");

                entity.HasIndex(e => new { e.InstructorId, e.CourseId }, "UQ__instruct__029B7EDAFE004C8E")
                    .IsUnique();

                entity.Property(e => e.InstructId).HasColumnName("instructID");

                entity.Property(e => e.CourseId).HasColumnName("courseID");

                entity.Property(e => e.InstructorId).HasColumnName("instructorID");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Instructs)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__instruct__course__4BAC3F29");

                entity.HasOne(d => d.Instructor)
                    .WithMany(p => p.Instructs)
                    .HasForeignKey(d => d.InstructorId)
                    .HasConstraintName("FK__instruct__instru__4AB81AF0");
            });

            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.ToTable("instructor");

                entity.Property(e => e.InstructorId).HasColumnName("instructorID");

                entity.Property(e => e.Birthday)
                    .HasColumnType("date")
                    .HasColumnName("birthday");

                entity.Property(e => e.Country)
                    .HasMaxLength(255)
                    .HasColumnName("country");

                entity.Property(e => e.Email)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .HasColumnName("first_name");

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .HasColumnName("gender");

                entity.Property(e => e.Income)
                    .HasColumnType("money")
                    .HasColumnName("income");

                entity.Property(e => e.Introduce)
                    .HasColumnType("ntext")
                    .HasColumnName("introduce");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasColumnName("last_name");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.Picture)
                    .HasColumnType("text")
                    .HasColumnName("picture");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("date")
                    .HasColumnName("registration_Date");

                entity.Property(e => e.Status)
                    .HasMaxLength(30)
                    .HasColumnName("status");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Learner>(entity =>
            {
                entity.ToTable("learner");

                entity.Property(e => e.LearnerId).HasColumnName("learnerID");

                entity.Property(e => e.Birthday)
                    .HasColumnType("date")
                    .HasColumnName("birthday");

                entity.Property(e => e.Country)
                    .HasMaxLength(255)
                    .HasColumnName("country");

                entity.Property(e => e.Email)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .HasColumnName("first_name");

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .HasColumnName("gender");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasColumnName("last_name");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.Picture)
                    .HasColumnType("text")
                    .HasColumnName("picture");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("date")
                    .HasColumnName("registration_Date");

                entity.Property(e => e.Status)
                    .HasMaxLength(30)
                    .HasColumnName("status");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.ToTable("lesson");

                entity.Property(e => e.LessonId).HasColumnName("lessonID");

                entity.Property(e => e.ChapterId).HasColumnName("chapterID");

                entity.Property(e => e.Content)
                    .HasColumnType("ntext")
                    .HasColumnName("content");

                entity.Property(e => e.CourseId).HasColumnName("courseID");

                entity.Property(e => e.Description)
                    .HasColumnType("ntext")
                    .HasColumnName("description");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.LessonName)
                    .HasMaxLength(50)
                    .HasColumnName("lesson_name");

                entity.Property(e => e.PercentToPassed)
                    .HasColumnName("percent_to_passed")
                    .HasDefaultValueSql("((80))");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.ChapterId)
                    .HasConstraintName("FK__lesson__chapterI__5441852A");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__lesson__courseID__5535A963");
            });

            modelBuilder.Entity<LessonProgress>(entity =>
            {
                entity.ToTable("lesson_progress");

                entity.Property(e => e.LessonProgressId).HasColumnName("lesson_progressID");

                entity.Property(e => e.ChapterId).HasColumnName("chapterID");

                entity.Property(e => e.Completed)
                    .HasColumnName("completed")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LearnerId).HasColumnName("learnerID");

                entity.Property(e => e.LessonId).HasColumnName("lessonID");

                entity.Property(e => e.StartAt)
                    .HasColumnType("datetime")
                    .HasColumnName("start_at")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Learner)
                    .WithMany(p => p.LessonProgresses)
                    .HasForeignKey(d => d.LearnerId)
                    .HasConstraintName("FK__lesson_pr__learn__74AE54BC");

                entity.HasOne(d => d.Lesson)
                    .WithMany(p => p.LessonProgresses)
                    .HasForeignKey(d => d.LessonId)
                    .HasConstraintName("FK__lesson_pr__lesso__75A278F5");
            });

            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.ToTable("quiz");

                entity.Property(e => e.QuizId).HasColumnName("quizID");

                entity.Property(e => e.AnswerId).HasColumnName("answerID");

                entity.Property(e => e.ChapterId).HasColumnName("chapterID");

                entity.Property(e => e.CourseId).HasColumnName("courseID");

                entity.Property(e => e.DapAnA)
                    .HasMaxLength(250)
                    .HasColumnName("dap_an_a");

                entity.Property(e => e.DapAnB)
                    .HasMaxLength(250)
                    .HasColumnName("dap_an_b");

                entity.Property(e => e.DapAnC)
                    .HasMaxLength(250)
                    .HasColumnName("dap_an_c");

                entity.Property(e => e.DapAnD)
                    .HasMaxLength(250)
                    .HasColumnName("dap_an_d");

                entity.Property(e => e.Note)
                    .HasMaxLength(250)
                    .HasColumnName("note");

                entity.Property(e => e.Quiz1)
                    .HasMaxLength(250)
                    .HasColumnName("quiz");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.Quizzes)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("FK__quiz__answerID__7C4F7684");

                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.Quizzes)
                    .HasForeignKey(d => d.ChapterId)
                    .HasConstraintName("FK__quiz__chapterID__7B5B524B");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Quizzes)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__quiz__courseID__7A672E12");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("report");

                entity.Property(e => e.ReportId).HasColumnName("reportID");

                entity.Property(e => e.Content)
                    .HasColumnType("text")
                    .HasColumnName("content");

                entity.Property(e => e.Email)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.SubmittedTime)
                    .HasColumnType("datetime")
                    .HasColumnName("submitted_time");

                entity.Property(e => e.Title)
                    .HasColumnType("ntext")
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("review");

                entity.Property(e => e.ReviewId).HasColumnName("reviewID");

                entity.Property(e => e.Comment).HasColumnName("comment");

                entity.Property(e => e.CourseId).HasColumnName("courseID");

                entity.Property(e => e.LearnerId).HasColumnName("learnerID");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.ReviewDate)
                    .HasColumnType("date")
                    .HasColumnName("review_date");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__review__courseID__59063A47");

                entity.HasOne(d => d.Learner)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.LearnerId)
                    .HasConstraintName("FK__review__learnerI__59FA5E80");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.ToTable("voucher");

                entity.Property(e => e.VoucherId).HasColumnName("voucherID");

                entity.Property(e => e.AdminId).HasColumnName("adminID");

                entity.Property(e => e.CodeVoucher).HasMaxLength(50);

                entity.Property(e => e.EndAt)
                    .HasColumnType("datetime")
                    .HasColumnName("end_at");

                entity.Property(e => e.PercentDiscount).HasColumnName("percent_discount");

                entity.Property(e => e.StartAt)
                    .HasColumnType("datetime")
                    .HasColumnName("start_at");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.InverseAdmin)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FK__voucher__adminID__3D5E1FD2");
            });

            modelBuilder.Entity<VoucherUsage>(entity =>
            {
                entity.ToTable("VoucherUsage");

                entity.Property(e => e.CodeVoucher).HasMaxLength(50);

                entity.Property(e => e.LearnerId).HasColumnName("learnerID");

                entity.Property(e => e.VoucherId).HasColumnName("voucherID");

                entity.HasOne(d => d.Learner)
                    .WithMany(p => p.VoucherUsages)
                    .HasForeignKey(d => d.LearnerId)
                    .HasConstraintName("FK__VoucherUs__learn__403A8C7D");

                entity.HasOne(d => d.Voucher)
                    .WithMany(p => p.VoucherUsages)
                    .HasForeignKey(d => d.VoucherId)
                    .HasConstraintName("FK__VoucherUs__vouch__412EB0B6");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
