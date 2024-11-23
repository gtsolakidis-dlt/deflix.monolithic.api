CREATE DATABASE [Deflix]
GO
USE [Deflix] 
GO

CREATE TABLE Users (
    UserId uniqueidentifier Primary KEY Default NEWID() ,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,  -- Consider a larger size for hashed passwords
    Email NVARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE Genre (
    GenreId uniqueidentifier Primary KEY Default NEWID() ,
    Name NVARCHAR(255) NOT NULL Unique
);

CREATE TABLE Movies (
    MovieId uniqueidentifier Primary KEY Default NEWID() ,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    Poster NVARCHAR(255) NULL,  -- URL or path to the movie poster
	Backdrop NVARCHAR(255) NULL,  -- URL or path to the movie Backdrop
	Logo NVARCHAR(255) NULL,  -- URL or path to the movie logo
    GenreId uniqueidentifier NOT NULL,
    YoutubeKey NVARCHAR(50) NULL,  -- Key to identify the related YouTube video
    CriticsRating decimal(18,2) DEFAULT 0,
    PlanType int NOT NULL  Default 1 -- e.g., "Free", "Basic", "Premium"
);

CREATE TABLE Subscriptions (
    [SubscriptionId] uniqueidentifier Primary KEY Default NEWID() ,
	[SubscriptionCode] int NOT NULL UNIQUE,
	[Name] nvarchar(50) NOT NULL,
	[Description] NVARCHAR(255) NULL,
	[DurationInDays] int NOT NULL Default 30,
	[Price] nvarchar(50) NULL,
);

CREATE TABLE UserSubscriptions (
    [UserSubscriptionId] uniqueidentifier Primary KEY Default NEWID() ,
	[UserId] uniqueIdentifier NOT NULL UNIQUE,
	[SubscriptionCode] int NOT NULL Default 1,
	[ExpirationDate] datetime NOT NULL,
	[PaymentMethod] nvarchar(50) NULL,
);

CREATE TABLE [UserPreferences](
	[UserPreferencesId] uniqueidentifier Primary KEY Default NEWID() ,
	[UserId] uniqueidentifier NOT NULL,
	[MovieId] uniqueidentifier NOT NULL,
	UserRating decimal(18,2) DEFAULT 0,  -- Average user rating
	UserComment NVARCHAR(MAX) NULL,
	[IsFavorite] bit NOT NULL Default 0,
	[IsWatchList] bit NOT NULL Default 0,
 CONSTRAINT [IX_UserPreferences] UNIQUE NONCLUSTERED 
(
	[UserId] ASC,
	[MovieId] ASC
) ON [PRIMARY]
) ON [PRIMARY];
