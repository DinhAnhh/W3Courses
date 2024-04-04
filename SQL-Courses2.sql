Create DATABASE Project_Group03
GO

USE Project_Group03
GO

CREATE TABLE [admin]--
(
    [adminID] INT IDENTITY (1,1) PRIMARY KEY,
    [username] VARCHAR(50),
    [password] VARCHAR(50)
);
GO

CREATE TABLE [learner]--
(
    [learnerID] INT IDENTITY (1,1) PRIMARY KEY,
    [first_name] NVARCHAR(50),
    [last_name] NVARCHAR(50),
    [gender] NVARCHAR(10),
    [birthday] DATE,
    [phoneNumber] VARCHAR(20),
    [email] VARCHAR(30),
    [country] NVARCHAR(255),
    [username] VARCHAR(50),
    [password] VARCHAR(50),
    [picture] TEXT,
    [registration_Date] DATE,
    [status] NVARCHAR(30)
);
GO

CREATE TABLE [instructor]--
(
    [instructorID] INT IDENTITY (1,1) PRIMARY KEY,
    [first_name] NVARCHAR(50),
    [last_name] NVARCHAR(50),
    [gender] NVARCHAR(10),
    [birthday] DATE,
    [phoneNumber] VARCHAR(20),
    [email] VARCHAR(30),
    [country] NVARCHAR(255),
    [username] VARCHAR(50),
    [password] VARCHAR(50),
    [picture] TEXT,
    [registration_Date] DATE,
    [income] MONEY,
	[introduce] NTEXT,
    [status] NVARCHAR(30)
);
GO
create TABLE [voucher](
	voucherID INT IDENTITY (1,1) PRIMARY KEY,
	adminID INT,
	percent_discount INT,
	start_at DATETIME,
	end_at DATETIME,
	CodeVoucher nvarchar(50),
	IsActive bit,
    FOREIGN KEY (adminID) REFERENCES [voucher]([voucherID]),
);
GO

CREATE TABLE VoucherUsage (
    Id INT IDENTITY (1,1) PRIMARY KEY,
    CodeVoucher nvarchar(50),
    learnerID INT,
	voucherID INT,
   FOREIGN KEY (learnerID) REFERENCES learner(learnerID),
 FOREIGN KEY (voucherID) REFERENCES [voucher](voucherID)
);

CREATE TABLE [categories] (--
    [categoryID] INT IDENTITY (1,1) PRIMARY KEY,
    [category_name] NVARCHAR(255),
    [description] NTEXT
);
GO


CREATE TABLE [courses] (--
    [courseID] INT IDENTITY (1,1) PRIMARY KEY,
    [categoryID] INT,
    [course_name] NVARCHAR(255),
    [description] NTEXT,
    [picture] TEXT,
    [total_time] INT DEFAULT 0, --tổng thời gian học hết lesson, chapter
    [creation_date] DATE,
    [price] MONEY,
    [status] NVARCHAR(30),
    FOREIGN KEY ([categoryID]) REFERENCES [categories]([categoryID])
);
GO

CREATE TABLE [instruct]
(
    [instructID] INT IDENTITY (1,1) PRIMARY KEY,
    [courseID] INT,
    [instructorID] INT,
    FOREIGN KEY ([instructorID]) REFERENCES [instructor]([instructorID]),
    FOREIGN KEY ([courseID]) REFERENCES [courses]([courseID]),
    UNIQUE ([instructorID], [courseID])
);
GO

CREATE TABLE [chapter]--
(
    [chapterID] INT IDENTITY (1,1) PRIMARY KEY,
    [courseID] INT,
    [chapter_name] NVARCHAR(50),
    [index] INT,
    [description] NTEXT,
    [total_time] INT DEFAULT 0,
    FOREIGN KEY (courseID) REFERENCES [courses](courseID)
);
GO

CREATE TABLE [lesson]--
(
    [lessonID] INT IDENTITY (1,1) PRIMARY KEY,
	[courseID] INT,
    [chapterID] INT,
    [lesson_name] NVARCHAR(50),
    [description] NTEXT,
    [percent_to_passed] INT DEFAULT 80,
    --[must_be_completed] BIT DEFAULT 1,
    [content] NTEXT,
    [index] INT,
    [time] INT DEFAULT 0 ,
    FOREIGN KEY (chapterID) REFERENCES [chapter](chapterID),
    FOREIGN KEY (courseID) REFERENCES [courses](courseID)
);
GO

CREATE TABLE [review] (
	[reviewID] INT IDENTITY (1,1) PRIMARY KEY,
	[courseID] INT,
	[learnerID] INT,
	[rating] INT CHECK (rating >= 1 AND rating <= 5),
	[comment] NVARCHAR(MAX),
	[review_date] DATE,
	FOREIGN KEY (courseID) REFERENCES [courses](courseID),
	FOREIGN KEY (learnerID) REFERENCES [learner](learnerID)
);
GO

CREATE TABLE [enrollment] (
	[enrollmentID] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[learnerID] [int] NOT NULL,
	[courseID] [int] NOT NULL,
	[learnerName] [nvarchar](50) NOT NULL,
	[courseName] [nvarchar](50) NOT NULL,
	[enrollmentDate] [date] NOT NULL,
	[email] [nvarchar](50) NOT NULL,
	[isPaid] [bit] NOT NULL,
    FOREIGN KEY ([learnerID]) REFERENCES learner([learnerID]),
    FOREIGN KEY ([courseId]) REFERENCES courses([courseID])
);
GO

CREATE TABLE [report](
	[reportID] INT IDENTITY (1,1) PRIMARY KEY,
	[name] NVARCHAR(100),
	[email] VARCHAR(30),
	[title] NTEXT,
	[content] TEXT,
	[submitted_time] DATETIME
)

CREATE TABLE [course_progress]
(
    [course_progressID] INT IDENTITY (1,1) PRIMARY KEY,
    [learnerID] INT,
    [courseID] INT,
    --[enrolled] BIT DEFAULT 0,
    [completed] BIT DEFAULT 0,
    [progress_percent] INT DEFAULT 0,
    [rated] BIT DEFAULT 0,
    [total_time] INT DEFAULT 0,
    [start_at] DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (learnerID) REFERENCES [learner](learnerID),
    FOREIGN KEY (courseID) REFERENCES [courses](courseID)
);
GO

CREATE TABLE [chapter_progress]
(
    [chapter_progressID] INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    [chapterID] INT,
    [course_progressID] INT,
    [progress_percent] INT DEFAULT 0,
    [completed] BIT DEFAULT 0,
    [total_time] INT DEFAULT 0,
    [start_at] DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (chapterID) REFERENCES [chapter](chapterID),
    FOREIGN KEY (course_progressID) REFERENCES [course_progress](course_progressID)
);
GO

CREATE TABLE [lesson_progress]
(
    [lesson_progressID] INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
	learnerID int,
    [lessonID] INT,
    [chapterID] INT,
    [completed] BIT DEFAULT 0,
    [start_at] DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (learnerID) REFERENCES learner(learnerID),
	 FOREIGN KEY (lessonID) REFERENCES [lesson](lessonID),
  
);
GO

CREATE TABLE answer (
answerID INT IDENTITY (1,1) PRIMARY KEY,
answer NVARCHAR(1)

);
CREATE TABLE quiz (
quizID INT IDENTITY (1,1) PRIMARY KEY,
[courseID] INT,
chapterID INT,
answerID INT,
quiz NVARCHAR(250),
dap_an_a NVARCHAR(250),
dap_an_b NVARCHAR(250),
dap_an_c NVARCHAR(250),
dap_an_d NVARCHAR(250),
note NVARCHAR(250),
IsCorrect BIT,
 FOREIGN KEY (courseID) REFERENCES [courses](courseID),
 FOREIGN KEY (chapterID) REFERENCES [chapter](chapterID),
 FOREIGN KEY (answerID) REFERENCES answer(answerID)
);
INSERT INTO [admin] ([username], [password])
VALUES ('admin123', '12345');
GO

INSERT INTO [learner] 
([first_name], [last_name], [gender], [birthday], [phoneNumber], [email], [country], [username], [password], [picture], [registration_Date], [status])
VALUES
(N'Trần Hồng', N'Lĩnh', N'Male', '1990-05-15', '0963820388', 'thlinh10c2@gmail.com', N'USA', 'linh123', '12345', NULL, '2023-01-01', 'Active'),
(N'Jane', N'Smith', N'Female', '1995-08-20', '0983792526', 'smith@gmail.com', N'Canada', 'janesmith123', '12345', NULL, '2023-01-02', 'Active'),
(N'Michael', N'Johnson', N'Male', '1988-11-10', '0987654321', 'johnson@gmail.com', N'UK', 'johnson123', '12345', NULL, '2023-01-03', 'Active'),
(N'Emily', N'Brown', N'Female', '1993-04-25', '0984831945', 'brown@gmail.com', N'Australia', 'emily123', '12345', NULL, '2023-01-04', 'Delete'),
(N'William', N'Davis', N'Male', '1992-09-18', '0847105822', 'davis@gmail.com', N'New Zealand', 'davis123', '12345', NULL, '2023-01-05', 'Wait'),
(N'Sophia', N'Wilson', N'Female', '1997-12-05', '0957238919', 'wilson@gmail.com', N'Germany', 'wilson123', '12345', NULL, '2023-01-06', 'Active');
GO

INSERT INTO [instructor] 
([first_name], [last_name], [gender], [birthday], [phoneNumber], [email], [country], [username], [password], [picture], [registration_Date], [income], [introduce], [status])
VALUES
(N'Huỳnh Nguyên', N'Bảo', N'Male', '1985-03-10', '0987654321', 'huynhnguyenbao3105@gmail.com', N'Vietnam', 'bao123', '12345', NULL, '2023-01-01', 500.00, N'Tôi là một giáo viên có kinh nghiệm trong lĩnh vực này.', 'Active'),
(N'Trần', N'Thị Bưởi', N'Female', '1990-07-15', '0912345678', 'tranthib@gmail.com', N'Vietnam', 'buoi123', '12345', NULL, '2023-01-02', 700.00, N'Tôi đã giảng dạy nhiều khóa học về chủ đề này.', 'Delete'),
(N'Phạm', N'Văn Cao', N'Male', '1988-12-20', '0971234567', 'phamvanc@gmail.com', N'Vietnam', 'cao123', '12345', NULL, '2023-01-03', 600.00, N'Tôi mong muốn chia sẻ kiến thức của mình với các học viên.', 'Active');
GO

INSERT INTO [categories] 
([category_name], [description])
VALUES
(N'Technology', N'This category includes courses related to technology such as programming, software development, and networking.'),
(N'Business', N'Courses under this category focus on business management, entrepreneurship, and finance.'),
(N'Language', N'Language courses cover various languages including English, Spanish, French, and Chinese.'),
(N'Health & Fitness', N'This category offers courses on health, fitness, nutrition, and wellness.'),
(N'Arts & Crafts', N'Arts and crafts category includes courses on painting, drawing, pottery, and other creative activities.'),
(N'Science & Engineering', N'This category covers courses related to science, engineering, physics, chemistry, and electronics.'),
(N'Personal Development', N'Courses in this category focus on personal development, soft skills, leadership, time management, and creative thinking.'),
(N'Music & Entertainment', N'This category offers courses on music, performing arts, singing, dancing, theater, and entertainment.'),
(N'Career Development', N'Courses under this category provide guidance on career development, job searching, interviews, resume building, and career management.'),
(N'Lifestyle & Hobbies', N'This category includes courses on lifestyle, personal hobbies, cooking, photography, travel, interior decoration, and local culture.');
GO

INSERT INTO [courses] 
([categoryID], [course_name], [description], [picture], [total_time], [creation_date], [price], [status])
VALUES
(1, N'Introduction to Python Programming', N'This course provides an introduction to Python programming language.', NULL, 30, '2024-03-01', 49.00, 'Active'),
(1, N'Web Development with HTML & CSS', N'Learn how to create responsive websites using HTML and CSS.', NULL, 45, '2024-03-15', 79.00, 'Active'),
(1, N'Introduction to Business Management', N'Explore the fundamentals of business management and organization.', NULL, 60, '2024-04-01', 99.00, 'Active'),
(1, N'Financial Planning for Beginners', N'Learn how to manage personal finances and plan for the future.', NULL, 30, '2024-04-15', 59.00, 'Active'),
(2, N'English Conversation Practice', N'Practice English conversation skills with native speakers.', NULL, 60, '2024-05-01', 69.00, 'Active'),
(2, N'French Language Basics', N'Beginner course covering basic French language skills.', NULL, 45, '2024-05-15', 49.00, 'Active'),
(2, N'Yoga for Beginners', N'Learn the basics of yoga including poses and breathing techniques.', NULL, 30, '2024-06-01', 29.00, 'Active'),
(3, N'Healthy Eating Habits', N'Discover the importance of nutrition and healthy eating habits.', NULL, 45, '2024-06-15', 39.00, 'Active'),
(3, N'Introduction to Watercolor Painting', N'Learn the basics of watercolor painting techniques.', NULL, 60, '2024-07-01', 89.00, 'Active'),
(3, N'Pottery Making Workshop', N'Hands-on workshop covering pottery making techniques.', NULL, 90, '2024-07-15', 129.00, 'Active');
GO

-- Tạo thông tin cho bảng instruct
INSERT INTO [instruct] ([courseID], [instructorID])
VALUES
(1, 1), 
(2, 1), 
(3, 1),
(4, 2),
(5, 2),
(6, 2),
(7, 2),
(8, 3),
(9, 3),
(10, 3);


-- Tạo thông tin cho bảng instruct
--INSERT INTO [instruct] ([courseID], [instructorID]) VALUES (1, 1);
--INSERT INTO [instruct] ([courseID], [instructorID]) VALUES (2, 2);
--INSERT INTO [instruct] ([courseID], [instructorID]) VALUES (3, 3);
--INSERT INTO [instruct] ([courseID], [instructorID]) VALUES (4, 3);
--INSERT INTO [instruct] ([courseID], [instructorID]) VALUES (5, 1);
--INSERT INTO [instruct] ([courseID], [instructorID]) VALUES (6, 2);
--INSERT INTO [instruct] ([courseID], [instructorID]) VALUES (7, 3);
--INSERT INTO [instruct] ([courseID], [instructorID]) VALUES (8, 1);
--INSERT INTO [instruct] ([courseID], [instructorID]) VALUES (9, 2);
--INSERT INTO [instruct] ([courseID], [instructorID]) VALUES (10, 3);
--GO

-- Tạo thông tin cho bảng Chapter
-- Chapter cho khóa học "Introduction to Python Programming"
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (1, N'Introduction', 1, N'Introduction to Python programming language', 10);
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (1, N'Variables and Data Types', 2, N'Understanding variables and data types in Python', 20);

-- Chapter cho khóa học "Web Development with HTML & CSS"
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (2, N'HTML Basics', 1, N'Introduction to HTML basics', 15);
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (2, N'CSS Basics', 2, N'Introduction to CSS basics', 20);

-- Chapter cho khóa học "Introduction to Business Management"
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (3, N'Introduction to Business', 1, N'Introduction to business fundamentals', 20);
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (3, N'Management Principles', 2, N'Understanding management principles', 25);

-- Chapter cho khóa học "Financial Planning for Beginners"
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (4, N'Personal Finance Basics', 1, N'Introduction to personal finance basics', 20);
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (4, N'Financial Planning', 2, N'Learn about financial planning for the future', 25);

-- Chapter cho khóa học "English Conversation Practice"
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (5, N'Greetings and Introductions', 1, N'Practice greetings and introductions in English', 15);
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (5, N'Everyday Conversations', 2, N'Practice everyday conversations in English', 25);

-- Chapter cho khóa học "French Language Basics"
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (6, N'Basic French Phrases', 1, N'Learn basic French phrases for everyday use', 20);
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (6, N'Introduction to French Grammar', 2, N'Introduction to basic French grammar concepts', 25);

-- Chapter cho khóa học "Yoga for Beginners"
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (7, N'Yoga Basics', 1, N'Introduction to yoga basics and poses', 20);
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (7, N'Breathing Techniques', 2, N'Learn about different breathing techniques in yoga', 25);

-- Chapter cho khóa học "Healthy Eating Habits"
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (8, N'Nutrition Basics', 1, N'Introduction to nutrition basics', 20);
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (8, N'Healthy Meal Planning', 2, N'Learn how to plan healthy meals', 25);

-- Chapter cho khóa học "Introduction to Watercolor Painting"
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (9, N'Watercolor Techniques', 1, N'Introduction to watercolor painting techniques', 25);
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (9, N'Color Mixing', 2, N'Learn about color mixing techniques in watercolor', 35);

-- Chapter cho khóa học "Pottery Making Workshop"
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (10, N'Pottery Basics', 1, N'Introduction to pottery basics and techniques', 30);
INSERT INTO [chapter] ([courseID], [chapter_name], [index], [description], [total_time]) VALUES (10, N'Hand-building Techniques', 2, N'Learn about hand-building techniques in pottery', 40);
GO

-- Tạo thông tin cho bảng Lesson
-- Lesson cho Chapter 1
-- Lesson cho khóa học "Introduction to Python Programming"
INSERT INTO [lesson] ([courseID], [chapterID], [lesson_name], [description], [content], [index], [time])
VALUES (1, 1, N'Python Introduction', N'Introduction to Python programming language', N'Content of Python Introduction lesson', 1, 10),
       (1, 1, N'Python Installation', N'Installing Python on your computer', N'Content of Python Installation lesson', 2, 20),
       (1, 2, N'Variables', N'Understanding variables in Python', N'Content of Variables lesson', 1, 15),
       (1, 2, N'Data Types', N'Understanding data types in Python', N'Content of Data Types lesson', 2, 25);

-- Lesson cho khóa học "Web Development with HTML & CSS"
INSERT INTO [lesson] ([courseID], [chapterID], [lesson_name], [description], [content], [index], [time])
VALUES (2, 3, N'HTML Introduction', N'Introduction to HTML basics', N'Content of HTML Introduction lesson', 1, 15),
       (2, 3, N'HTML Tags', N'Understanding HTML tags', N'Content of HTML Tags lesson', 2, 20),
       (2, 4, N'CSS Introduction', N'Introduction to CSS basics', N'Content of CSS Introduction lesson', 1, 20),
       (2, 4, N'CSS Selectors', N'Understanding CSS selectors', N'Content of CSS Selectors lesson', 2, 25);

-- Lesson cho khóa học "Introduction to Business Management"
INSERT INTO [lesson] ([courseID], [chapterID], [lesson_name], [description], [content], [index], [time])
VALUES (3, 5, N'Business Fundamentals', N'Introduction to business fundamentals', N'Content of Business Fundamentals lesson', 1, 20),
       (3, 5, N'Business Ethics', N'Understanding business ethics', N'Content of Business Ethics lesson', 2, 25),
       (3, 6, N'Management Principles', N'Understanding management principles', N'Content of Management Principles lesson', 1, 25),
       (3, 6, N'Leadership Styles', N'Exploring different leadership styles', N'Content of Leadership Styles lesson', 2, 30);

-- Lesson cho khóa học "Financial Planning for Beginners"
INSERT INTO [lesson] ([courseID], [chapterID], [lesson_name], [description], [content], [index], [time])
VALUES (4, 7, N'Personal Finance Basics', N'Introduction to personal finance basics', N'Content of Personal Finance Basics lesson', 1, 20),
       (4, 7, N'Budgeting', N'Understanding budgeting and financial planning', N'Content of Budgeting lesson', 2, 25),
       (4, 8, N'Investment Strategies', N'Learning about investment strategies', N'Content of Investment Strategies lesson', 1, 25),
       (4, 8, N'Retirement Planning', N'Planning for retirement', N'Content of Retirement Planning lesson', 2, 30);

-- Lesson cho khóa học "English Conversation Practice"
INSERT INTO [lesson] ([courseID], [chapterID], [lesson_name], [description], [content], [index], [time])
VALUES (5, 9, N'Greetings and Introductions', N'Practice greetings and introductions in English', N'Content of Greetings and Introductions lesson', 1, 15),
       (5, 9, N'Everyday Conversations', N'Practice everyday conversations in English', N'Content of Everyday Conversations lesson', 2, 25),
       (5, 10, N'Listening Comprehension', N'Improving listening comprehension skills', N'Content of Listening Comprehension lesson', 1, 30),
       (5, 10, N'Pronunciation Practice', N'Practice English pronunciation', N'Content of Listening Comprehension lesson', 1, 30);

INSERT INTO [lesson] ([courseID], [chapterID], [lesson_name], [description], [content], [index], [time])
VALUES (1, 1, N'Python Introduction', N'Introduction to Python programming language', N'Content of Python Introduction lesson', 1, 10),
       (1, 1, N'Python Installation', N'Installing Python on your computer', N'Content of Python Installation lesson', 2, 20),
       (1, 2, N'Variables', N'Understanding variables in Python', N'Content of Variables lesson', 1, 15),
       (1, 2, N'Data Types', N'Understanding data types in Python', N'Content of Data Types lesson', 2, 25);

-- Lesson cho khóa học "Web Development with HTML & CSS"
INSERT INTO [Lesson] ([CourseID], [ChapterID], [lesson_name], [Description], [Content], [Index], [Time])
VALUES (2, 3, N'HTML Introduction', 'Introduction to HTML basics', N'Content of HTML Introduction lesson', 1, 15),
       (2, 3, N'HTML Tags', 'Understanding HTML tags', N'Content of HTML Tags lesson', 2, 20),
       (2, 4, N'CSS Introduction', 'Introduction to CSS basics', N'Content of CSS Introduction lesson', 1, 20),
       (2, 4, N'CSS Selectors', 'Understanding CSS selectors', N'Content of CSS Selectors lesson', 2, 25);

-- Lesson cho khóa học "Introduction to Business Management"
INSERT INTO [Lesson] ([CourseID], [ChapterID], [lesson_name], [Description], [Content], [Index], [Time])
VALUES (3, 5, N'Business Fundamentals', N'Introduction to business fundamentals', N'Content of Business Fundamentals lesson', 1, 20),
       (3, 5, N'Business Ethics', N'Understanding business ethics', N'Content of Business Ethics lesson', 2, 25),
       (3, 6, N'Management Principles', N'Understanding management principles', N'Content of Management Principles lesson', 1, 25),
       (3, 6, N'Leadership Styles', N'Exploring different leadership styles', N'Content of Leadership Styles lesson', 2, 30);

-- Lesson cho khóa học "Financial Planning for Beginners"
INSERT INTO [Lesson] ([CourseID], [ChapterID], [lesson_name], [Description], [Content], [Index], [Time])
VALUES (4, 7, N'Personal Finance Basics', N'Introduction to personal finance basics', N'Content of Personal Finance Basics lesson', 1, 20),
       (4, 7, N'Budgeting', N'Understanding budgeting and financial planning', N'Content of Budgeting lesson', 2, 25),
       (4, 8, N'Investment Strategies', N'Learning about investment strategies', N'Content of Investment Strategies lesson', 1, 25),
       (4, 8, N'Retirement Planning', N'Planning for retirement', N'Content of Retirement Planning lesson', 2, 30);

-- Lesson cho khóa học "English Conversation Practice"
INSERT INTO [Lesson] ([CourseID], [ChapterID], [lesson_name], [Description], [Content], [Index], [Time])
VALUES (5, 9, N'Greetings and Introductions', N'Practice greetings and introductions in English', N'Content of Greetings and Introductions lesson', 1, 15),
       (5, 9, N'Everyday Conversations', N'Practice everyday conversations in English', N'Content of Everyday Conversations lesson', 2, 25),
       (5, 10, N'Listening Comprehension', N'Improving listening comprehension skills', N'Content of Listening Comprehension lesson', 1, 30),
       (5, 10, N'Pronunciation Practice', N'Practice English pronunciation', N'Content of Pronunciation Practice lesson', 2, 35);

-- Review for Course 1
INSERT INTO [review] ([courseID], [learnerID], [rating], [comment], [review_date])
VALUES (1, 1, 5, N'Great course!', '2024-03-02');

INSERT INTO [review] ([courseID], [learnerID], [rating], [comment], [review_date])
VALUES (1, 2, 4, N'Enjoyed learning Python.', '2024-03-03');

INSERT INTO [review] ([courseID], [learnerID], [rating], [comment], [review_date])
VALUES (1, 3, 3, N'Could be more challenging.', '2024-03-04');

INSERT INTO [review] ([courseID], [learnerID], [rating], [comment], [review_date])
VALUES (1, 2, 3, N'Helpful for managing personal finances.', '2024-03-11');

INSERT INTO [review] ([courseID], [learnerID], [rating], [comment], [review_date])
VALUES (1, 4, 4, N'Useful financial planning tips.', '2024-03-12');

INSERT INTO [review] ([courseID], [learnerID], [rating], [comment], [review_date])
VALUES (1, 6, 5, N'Exactly what I needed.', '2024-03-13');

-- Review for Course 2
INSERT INTO [review] ([courseID], [learnerID], [rating], [comment], [review_date])
VALUES (2, 1, 4, N'Good introduction to web development.', '2024-03-05');

INSERT INTO [review] ([courseID], [learnerID], [rating], [comment], [review_date])
VALUES (2, 4, 5, N'Highly recommended!', '2024-03-06');

INSERT INTO [review] ([courseID], [learnerID], [rating], [comment], [review_date])
VALUES (2, 6, 3, N'Could cover more advanced topics.', '2024-03-07');

INSERT INTO [review] ([courseID], [learnerID], [rating], [comment], [review_date])
VALUES (2, 1, 5, N'Great practice for English conversation.', '2024-03-14');

-- Review for Course 3
INSERT INTO [review] ([courseID], [learnerID], [rating], [comment], [review_date])
VALUES (3, 3, 5, N'Learned a lot about business management.', '2024-03-08');

INSERT INTO [review] ([courseID], [learnerID], [rating], [comment], [review_date])
VALUES (3, 5, 4, N'Good course for beginners.', '2024-03-09');

INSERT INTO [review] ([courseID], [learnerID], [rating], [comment], [review_date])
VALUES (3, 6, 2, N'Not what I expected.', '2024-03-10');
GO

---
CREATE TRIGGER delete_instruct_triggerr
ON instructor
INSTEAD OF DELETE
AS
BEGIN
    -- Xóa các bài học (lessons) thuộc về khóa học (course) bị xóa
    DELETE FROM instruct WHERE instructorID IN (SELECT instructorID FROM deleted);

    -- Lấy danh sách các khóa học (course) thuộc về người hướng dẫn (instructor) bị xóa
    DECLARE @DeletedCourses TABLE (courseID INT);
    INSERT INTO @DeletedCourses (courseID)
    SELECT DISTINCT courseID FROM instruct WHERE instructorID IN (SELECT instructorID FROM deleted);

    -- Xóa các bài học (lessons) thuộc về các khóa học (course) bị xóa
    DELETE FROM lesson WHERE courseID IN (SELECT courseID FROM @DeletedCourses);
    DELETE FROM chapter WHERE courseID IN (SELECT courseID FROM @DeletedCourses);

    -- Xóa các khóa học (course) bị xóa
    DELETE FROM courses WHERE courseID IN (SELECT courseID FROM @DeletedCourses);

    -- Xóa người hướng dẫn (instructor) bị xóa
    DELETE FROM instructor WHERE instructorID IN (SELECT instructorID FROM deleted);
END;
GO
---

CREATE TRIGGER delete_learner_trigger
ON learner
INSTEAD OF DELETE
AS
BEGIN
    -- Xóa các bài đăng ký (enrollment) của người học (learner) bị xóa
    DELETE FROM enrollment WHERE learnerID IN (SELECT learnerID FROM deleted);

    -- Xóa các đánh giá (review) của người học (learner) bị xóa
    DELETE FROM review WHERE learnerID IN (SELECT learnerID FROM deleted);

    -- Xóa người học (learner) bị xóa
    DELETE FROM learner WHERE learnerID IN (SELECT learnerID FROM deleted);
END;
go
---


CREATE TRIGGER delete_course_trigger
ON courses
INSTEAD OF DELETE
AS
BEGIN
    -- Xóa các bài học (lessons) thuộc về khóa học (course) bị xóa
    DELETE FROM lesson WHERE chapterID IN (SELECT chapterID FROM deleted);
	
	    DELETE FROM quiz WHERE chapterID IN (SELECT chapterID FROM deleted);
    -- Xóa các chương (chapters) thuộc về khóa học (course) bị xóa
    DELETE FROM chapter WHERE courseID IN (SELECT courseID FROM deleted);

    -- Xóa các hướng dẫn (instruct) thuộc về khóa học (course) bị xóa
    DELETE FROM instruct WHERE courseID IN (SELECT courseID FROM deleted);

    -- Xóa các bản ghi từ bảng enrollment thuộc về khóa học (course) bị xóa
    DELETE FROM enrollment WHERE courseID IN (SELECT courseID FROM deleted);
	    DELETE FROM review WHERE courseID IN (SELECT courseID FROM deleted);
    -- Xóa khóa học (course) bị xóa
    DELETE FROM courses WHERE courseID IN (SELECT courseID FROM deleted);
END;
GO

---
CREATE TRIGGER delete_chapter_trigger
ON chapter
INSTEAD OF DELETE
AS
BEGIN
    -- Xóa các bài học (lessons) thuộc về khóa học (course) bị xóa
    DELETE FROM lesson WHERE chapterID IN (SELECT chapterID FROM deleted);

	   DELETE FROM quiz WHERE chapterID IN (SELECT chapterID FROM deleted);
    -- Xóa các chương (chapters) thuộc về khóa học (course) bị xóa
    DELETE FROM chapter WHERE chapterID IN (SELECT chapterID FROM deleted);
END;
GO
---
CREATE TRIGGER delete_learner_trigger
ON learner
INSTEAD OF DELETE
AS
BEGIN
    -- Xóa các bài đăng ký (enrollment) của người học (learner) bị xóa
    DELETE FROM enrollment WHERE learnerID IN (SELECT learnerID FROM deleted);

    -- Xóa các đánh giá (review) của người học (learner) bị xóa
    DELETE FROM review WHERE learnerID IN (SELECT learnerID FROM deleted);

    -- Xóa người học (learner) bị xóa
    DELETE FROM learner WHERE learnerID IN (SELECT learnerID FROM deleted);
END;
GO

CREATE TRIGGER delete_cate_trigger
ON categories
INSTEAD OF DELETE
AS
BEGIN
    DELETE FROM courses WHERE categoryID IN (SELECT categoryID FROM deleted);
    DELETE FROM categories WHERE categoryID IN (SELECT categoryID FROM deleted);
END;
go
CREATE TRIGGER delete_answer_trigger
ON answer
INSTEAD OF DELETE
AS
BEGIN
    DELETE FROM quiz
    WHERE answerID IN (SELECT answerID FROM deleted);
	  DELETE FROM answer WHERE answerID IN (SELECT answerID FROM deleted);
END;
GO

INSERT INTO [enrollment] ([learnerID], [courseID], [learnerName], [courseName], [enrollmentDate], [email], [isPaid])
VALUES (6, 2, 'Sophia Wilson', 'Web Development with HTML & CSS', '2022-01-01', 'wilson@gmail.com', 1);

