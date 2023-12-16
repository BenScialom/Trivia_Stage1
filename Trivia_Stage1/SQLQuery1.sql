
CREATE DATABASE TriviaGame;

USE TriviaGame;


CREATE TABLE [Ranks]
(
RankId int Identity(1,1) not null PRIMARY KEY,
RankName nvarchar(150) not null
);



CREATE TABLE [Players]
( 
PlayerId int Identity(1,1) not null PRIMARY KEY,
Mail nvarchar(30) not null,
[Name] nvarchar(20) not null,
[Password] nvarchar(20) not null,
RankId int not null,
CONSTRAINT FK_RankId FOREIGN KEY (RankId) REFERENCES [Ranks](RankId),
Points int not null
);




CREATE TABLE [Subject]
(
SubjectId int Identity(1,1) not null PRIMARY KEY,
SubjectName nvarchar(100) not null
);


CREATE TABLE [Status]
(
StatusId int Identity(1,1) not null PRIMARY KEY,
[Status] nvarchar(200) not null
);



CREATE TABLE [Questions]
(
QuestionId  int Identity(1,1) not null PRIMARY KEY,
StatusId int not null,
CONSTRAINT FK_StatusId FOREIGN KEY (StatusId) REFERENCES [Status](StatusId),
SubjectId int not null,
CONSTRAINT FK_SubjectId FOREIGN KEY (SubjectId) REFERENCES [Subject](SubjectId),
PlayerId int not null,
CONSTRAINT FK_PlayerId FOREIGN KEY (PlayerId) REFERENCES [Players](PlayerId),
Question nvarchar(1000) not null,
RightA nvarchar(300) not null,
WrongA1 nvarchar(300) not null,
WrongA2 nvarchar(300) not null,
WrongA3 nvarchar(300) not null
);

INSERT INTO Ranks VALUES('Admin');
INSERT INTO [Subject] VALUES('Sport');
INSERT INTO [Subject] VALUES('Politics');
INSERT INTO [Subject] VALUES('History');
INSERT INTO [Subject] VALUES('Science');
INSERT INTO [Subject] VALUES('Ramon');
INSERT INTO [Status] VALUES('Approved');
INSERT INTO [Players] VALUES('m@gmail.com','TalsiHamelch','TalsiKing123',(SELECT [RankId] FROM [Ranks] WHERE [RankName]='Admin'),0);
INSERT INTO [Questions] VALUES((SELECT [StatusId] FROM [Status] WHERE [Status]='Approved'),(SELECT [SubjectId] FROM [Subject] WHERE [SubjectName]='Sport'),(SELECT [PlayerId] FROM [Players] WHERE [Name]='TalsiHamelch'),'Who won the football champions legue last season?','Manchester City','Real Madrid','Liverpool','HapoelTLV');
INSERT INTO [Questions] VALUES((SELECT [StatusId] FROM [Status] WHERE [Status]='Approved'),(SELECT [SubjectId] FROM [Subject] WHERE [SubjectName]='Politics'),(SELECT [PlayerId] FROM [Players] WHERE [Name]='TalsiHamelch'),'Who was the first prime minister of Israel?','David Ben Gurion','Benjemin Netanyahu','Golda Meir','Mnahem Begin');
INSERT INTO [Questions] VALUES((SELECT [StatusId] FROM [Status] WHERE [Status]='Approved'),(SELECT [SubjectId] FROM [Subject] WHERE [SubjectName]='History'),(SELECT [PlayerId] FROM [Players] WHERE [Name]='TalsiHamelch'),'What year Israel was established?','1948','1950','1990','1939');
INSERT INTO [Questions] VALUES((SELECT [StatusId] FROM [Status] WHERE [Status]='Approved'),(SELECT [SubjectId] FROM [Subject] WHERE [SubjectName]='Science'),(SELECT [PlayerId] FROM [Players] WHERE [Name]='TalsiHamelch'),'Who discoverd the force of gravity?','Isak Newton','Galileo','Itamar','Einstein');
INSERT INTO [Questions] VALUES((SELECT [StatusId] FROM [Status] WHERE [Status]='Approved'),(SELECT [SubjectId] FROM [Subject] WHERE [SubjectName]='Ramon'),(SELECT [PlayerId] FROM [Players] WHERE [Name]='TalsiHamelch'),'Who is the best teacher?','Talsi','Alex','Tali','Dalia');



