INSERT INTO Genre (GenreId, Name) VALUES
('57a6dd52-04fc-4d23-b7b0-ee3a54b7c346', 'Adventure'),
('2b6ddfcc-6e12-42a8-b8df-9c5286f6f17e', 'Fantasy'),
('f06287a4-e145-4421-a216-90ae15d52d77', 'Animation'),
('64d4af6a-bf26-4615-8d88-cb19ae5798b2', 'Drama'),
('05a25418-6489-4296-aaec-fd63902f3e3b', 'Horror'),
('516c66ec-c08a-4c39-a370-c4b6b3bd7ec7', 'Action'),
('dd3fee93-7ba9-44a2-bd7c-34042beee050', 'Comedy'),
('1c0f359f-1a92-4cb6-a661-03ce966371f7', 'History'),
('c9a4f4f7-a510-4f5b-9257-72acadff678a', 'Western'),
('99fbbe16-1e14-4c6d-8902-8a61456a7b3e', 'Thriller'),
('5d0dd214-a9e7-4a76-bb61-4f2109da1ed0', 'Crime'),
('81984c23-a9b0-4bf9-b4e3-fbe3af5df3b1', 'Documentary'),
('5e1c4593-0302-4841-b3a9-fd4033329d6d', 'Science Fiction'),
('a5a0563a-7951-4c81-8e15-59f9d5222662', 'Mystery'),
('aac28b48-463d-405c-a4c2-f0fa7eb7a1c2', 'Music'),
('8dc958a8-73bd-41f4-8fe5-78caa881069e', 'Romance'),
('7435e2b1-69d8-401c-80eb-dfd573c3dd7f', 'Family'),
('f4e2d157-f797-4332-b551-b19b28573560', 'War'),
('1028bf0e-8617-4a93-b606-d56f1b49e941', 'TV Movie');

INSERT INTO Users (UserId, Username, Password, Email) VALUES
('3FFA0D7E-DA9F-4C63-919D-78319A69CC11', 'test', '1231', 'test@test.gr'),
('6D9A1C82-AF34-4D56-8E4B-12345678ABCD', 'john_doe', 'pass123', 'john.doe@example.com'),
('E1B0D3F4-3F19-41C2-9D83-A1B2C3D4E5F6', 'jane_smith', 'secure456', 'jane.smith@example.com'),
('A9C83B71-1234-4F67-8D9E-6F7A8B9C0DEF', 'admin', 'adminpass', 'admin@domain.com'),
('1234F9E8-D7C6-4A5B-9E8D-7A6B5C4D3F2A', 'alice', 'wonderland', 'alice@fantasy.com'),
('B5C7A8D9-E6F5-4C3D-2B1A-9F8E7D6C5B4A', 'bob_builder', 'hammer123', 'bob.builder@construction.net'),
('F9E8D7C6-B5A4-4C3D-2B1A-6F7A8B9C0DEF', 'charlie', 'choco123', 'charlie@factory.com'),
('D3C4B5A6-7890-4E1F-9A8D-2B3C4D5E6F7A', 'eve_hacker', 'hackme', 'eve@cyber.net'),
('A1B2C3D4-E5F6-4789-90AB-CDEF12345678', 'mallory', 'malicious', 'mallory@evil.com'),
('F1E2D3C4-B5A6-7D89-9A0B-2C3D4E5F6A7B', 'oscar', 'oscar2024', 'oscar@movies.com');

INSERT INTO Subscriptions (SubscriptionId, SubscriptionCode, Name, Description, DurationInDays, Price) VALUES
('3FFA0D7E-DA9F-4C63-919D-78319A69CC24', '1', 'Free', 'A free subscription plan', 30, '0.0'),
('6D9A1C82-AF34-4D56-8E4B-12345678ABCD', '2', 'Basic', 'A basic subscription plan with limited features', 30, '9.99'),
('E1B0D3F4-3F19-41C2-9D83-A1B2C3D4E5F6', '3', 'Premium', 'A premium subscription plan with all features', 30, '19.99');

INSERT INTO UserSubscriptions (UserSubscriptionId, UserId, SubscriptionCode, ExpirationDate, PaymentMethod) VALUES
(NEWID(),'3FFA0D7E-DA9F-4C63-919D-78319A69CC11', '1', dateadd(dd,30,getdate()),'None'),
(NEWID(),'6D9A1C82-AF34-4D56-8E4B-12345678ABCD', '1', dateadd(dd,30,getdate()),'None'),
(NEWID(),'E1B0D3F4-3F19-41C2-9D83-A1B2C3D4E5F6', '2', dateadd(dd,30,getdate()),'None'),
(NEWID(),'A9C83B71-1234-4F67-8D9E-6F7A8B9C0DEF', '1', dateadd(dd,30,getdate()),'None'),
(NEWID(),'1234F9E8-D7C6-4A5B-9E8D-7A6B5C4D3F2A', '2', dateadd(dd,30,getdate()),'None'),
(NEWID(),'B5C7A8D9-E6F5-4C3D-2B1A-9F8E7D6C5B4A', '3', dateadd(dd,30,getdate()),'None'),
(NEWID(),'F9E8D7C6-B5A4-4C3D-2B1A-6F7A8B9C0DEF', '1', dateadd(dd,30,getdate()),'None'),
(NEWID(),'D3C4B5A6-7890-4E1F-9A8D-2B3C4D5E6F7A', '2', dateadd(dd,30,getdate()),'None'),
(NEWID(),'A1B2C3D4-E5F6-4789-90AB-CDEF12345678', '1', dateadd(dd,30,getdate()),'None'),
(NEWID(),'F1E2D3C4-B5A6-7D89-9A0B-2C3D4E5F6A7B', '3', dateadd(dd,30,getdate()),'None');
