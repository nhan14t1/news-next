CREATE SCHEMA `NewsNext` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_vietnamese_ci ;

CREATE TABLE `NewsNext`.User (
    Id int NOT NULL AUTO_INCREMENT,
    Email varchar(255) NOT NULL,
    Password varchar(255) character set utf8mb4,
    Salt varchar(255) not null,
    FirstName varchar(255) character set utf8mb4,
    LastName varchar(255) character set utf8mb4,
    Age int not null default 0,
    PhoneNumber varchar(255),
    PRIMARY KEY (Id)
);

CREATE TABLE `NewsNext`.Role (
    Id int NOT NULL AUTO_INCREMENT,
    Name varchar(255) NOT NULL,
    PRIMARY KEY (Id)
);

CREATE TABLE `NewsNext`.UserRole (
    Id int NOT NULL AUTO_INCREMENT,
    UserId int not null,
    RoleId int not null,
    PRIMARY KEY (Id),
    FOREIGN KEY (UserId) REFERENCES User(Id),
    FOREIGN KEY (RoleId) REFERENCES Role(Id)
);

INSERT INTO `NewsNext`.Role(`Name`) values ('Admin'), ('User');

CREATE TABLE `NewsNext`.Category (
	Id int NOT NULL AUTO_INCREMENT,
    Name varchar(255) character set utf8mb4 NOT NULL,
    IsActive INT NOT NULL DEFAULT 1,
    PRIMARY KEY (Id)
);

INSERT INTO `NewsNext`.Category(`Name`) values ('Showbiz Việt Nam'), ('Showbiz Quốc Tế');

CREATE TABLE `NewsNext`.Post (
    Id int NOT NULL AUTO_INCREMENT,
    Title varchar(2000) character set utf8mb4 NOT NULL,
    IntroText varchar(2000) character set utf8mb4 NOT NULL default '',
    Slug varchar(255) NOT NULL default '',
    Content TEXT character set utf8mb4 NOT NULL,
    UserId int not null,
    Status int not null default 1,
    CreatedDate BigInt not null,
    UpdatedDate BigInt not null default 0,
    ScheduleDate BigInt not null default 0,
    PRIMARY KEY (Id),
    FOREIGN KEY (UserId) REFERENCES User(Id)
);

CREATE TABLE `NewsNext`.PostCategory (
	Id int NOT NULL AUTO_INCREMENT,
    PostId int NOT NULL,
    CategoryId int NOT NULL,
    PRIMARY KEY (Id),
    UNIQUE KEY `post_category_id` (PostId, CategoryId),
    FOREIGN KEY (PostId) REFERENCES Post(Id),
	FOREIGN KEY (CategoryId) REFERENCES Category(Id)
);
