/* check whether the database exists; if so, drop it */
IF EXISTS(SELECT 1 FROM master.dbo.sysdatabases
			WHERE name = 'Circus_Days_db')
BEGIN
	DROP DATABASE Circus_Days_db
	print '' print '*** dropping database Circus_Days_db'
END
GO

print '' print '*** creating database Circus_Days_db'
GO
CREATE DATABASE [Circus_Days_db]
GO

Print '' Print '*** using Circus_Days_db'
GO
USE [Circus_Days_db]
GO

/* Zip table */
print '' print '*** creating zip_code TABLE'
GO
CREATE TABLE [dbo].[zip_code](
	[zip_code]		[CHAR](5)			NOT NULL,
	[city]			[NVARCHAR](50)		NOT NULL,
	[state]			[CHAR](2)			NOT NULL,
	CONSTRAINT [pk_zip_code] PRIMARY KEY([zip_code])
)
GO

/* Zip test records */
print '' print '*** inseritng zip_code test records'
GO 
INSERT INTO [dbo].[zip_code]
	([zip_code], [city], [state])
	VALUES
		('52403', 'Cedar Rapids', 'IA'),
		('50317', 'Des Moins', 'IA'),
		('44444', 'Someburg', 'IL'),
		('36386', 'Sample Vill', 'OH'),
		('83202', 'Pocatello', 'ID'),
		('11111', 'Example Town', 'SD'),
		('01702', 'Farmingham', 'MA')
GO

/* Employee table */
print '' print '*** creating employee table'
GO 
CREATE TABLE [dbo].[Employee] (
	[employee_id] 	[int] IDENTITY(100000,1) 	NOT NULL,
	[given_name]		[nvarchar](50)				NOT NULL,
	[family_name]	[nvarchar](100)				NOT NULL,
	[phone_number]			[nvarchar](13)				NOT NULL,
	[email]			[nvarchar](100)				NOT NULL,
	[password_hash]	[nvarchar](100)				NOT NULL DEFAULT 			'9c9064c59f1ffa2e174ee754d2979be80dd30db552ec03e7e327e9b1a4bd594e',
	[active]		[bit]						NOT NULL DEFAULT 1,
	CONSTRAINT [pk_employee_id] PRIMARY KEY([employee_id]),
	CONSTRAINT [ak_email] UNIQUE([email])
)
GO

/* Employee test records */
print '' print '*** inseritng employee test records'
GO 
INSERT INTO [dbo].[Employee]
		([given_name], [family_name], [phone_number], [email])	
    VALUES
		('Joanne', 'smith', '3195551111', 'joanne@circus.com'),
		('Mario', 'Mario', '3195552222', 'mario@circus.com'),
		('Shoko', 'Komi', '3195553333', 'shoko@circus.com')
GO

/* Role table */
print '' print '*** creating Role table'
GO
CREATE TABLE [dbo].[Role] (
	[role_id]		[nvarchar](50)		NOT NULL,
	[description]	[nvarchar](250)		NULL,
	CONSTRAINT [pk_role_id] PRIMARY KEY ([role_id])
)
GO

print '' print '*** inserting sample role records'
GO 
INSERT INTO [dbo].[Role]
		([role_id], [description])
	VALUES
		("Tour Planner", "Aranges what venues the circus will go to and when."),
		("Ad Planner", "Aranges what types of ads will be used and what act of the show they will focus on."),
		("Preformer", "Is part of the show")
GO

/* EmployeeRole join table */

print '' print '*** creation EmployeeRole table'

CREATE TABLE [dbo].[EmployeeRole] (
	[employee_id] 	[int] 			 	NOT NULL,
	[role_id]		[nvarchar](50)		NOT NULL,
	CONSTRAINT [fk_EmployeeRole_employee_id] FOREIGN KEY ([employee_id])
		REFERENCES [dbo].[Employee]([employee_id]),
	CONSTRAINT [fk_EmployeeRole_role_id] FOREIGN KEY ([role_id])
		REFERENCES [dbo].[Role]([role_id]),
	CONSTRAINT [pk_EmployeeRole] PRIMARY KEY ([employee_id], [role_id])
)
GO

print '' print '*** inserting sample EmployeeRole records'
GO 
INSERT INTO [dbo].[EmployeeRole]
		([employee_id], [role_id])
	VALUES
		(100000, "Tour Planner"),
		(100001, "Ad Planner"),
		(100002, "Preformer")
GO

/* login-related stored procedures */

print '' print '*** creating sp_authenticate_user'
GO 
CREATE PROCEDURE [dbo].[sp_authenticate_user]
(
	@email			[nvarchar](100),
	@password_hash	[nvarchar](100)
)
AS 
	BEGIN
		SELECT COUNT([employee_id]) AS 'Authenticated'
		FROM	[Employee]
		WHERE	@email = [email]
			AND @password_hash = [password_hash]
			AND [active] = 1
	
	END
GO

print '' print '*** creationg sp_select_employee_by_email'
GO 
CREATE PROCEDURE [dbo].[sp_select_employee_by_email]
(
	@email			[nvarchar](100)
)
AS 
	BEGIN
		SELECT 	[employee_id], [given_name], [family_name],	
				[phone_number], [email], [active]		
		FROM 	[Employee]
		WHERE 	@email = [email]
	END
GO

print '' print '*** creating  sp_select_roles_by_employee_id'
GO
CREATE PROCEDURE [dbo].[sp_select_roles_by_employee_id]
(
	@employee_id			[int]
)
AS 
	BEGIN
		SELECT 	[role_id]		
		FROM 	[EmployeeRole]
		WHERE 	@employee_id = [employee_id]
	END
GO

print '' print '*** creating sp_update_password_hash'
GO
CREATE PROCEDURE [dbo].[sp_update_password_hash]
(
	@employee_id 		[int],
	@password_hash		[nvarchar](100),
	@Oldpassword_hash	[nvarchar](100)
)
AS 
	BEGIN
		UPDATE 	[Employee]		
			SET 	[password_hash] = @password_hash
		WHERE 	@employee_id = [employee_id]
			AND 	@Oldpassword_hash = [password_hash]
			
		RETURN @@ROWCOUNT
	END
GO

/* ad related tables */

/* act table */
print '' print '*** creating act table'
GO
CREATE TABLE  [dbo].[act](
	[act_name]		[NVARCHAR](50)	NOT NULL,
    [description]	[NVARCHAR](100)	null,
	CONSTRAINT [pk_act_name] PRIMARY KEY ([act_name])
)
GO
print '' print '*** inserting sample act records'
GO
INSERT INTO [dbo].[act]
	([act_name], [description])
	VALUES
		('trapeze', 'People preforming gymnastics while swinging form objects suspended in the air.'),
		('human cannon-ball', 'A person being shot out of a cannon.'),
		('knife thrower', null),
		('tight-rope walker', 'Someone walking from one platform to another on a samll rope many feet in the air.'),
		('clowns', null),
		('strong man', 'A man who lifts seamingly impossibly heavy things.')
GO

/* ad_type table */
print '' print '*** creating ad_type table'
GO
CREATE TABLE  [dbo].[ad_type](
	[ad_type]		[NVARCHAR](50)	NOT NULL,
	CONSTRAINT [pk_ad_type] PRIMARY KEY ([ad_type])
)
GO
print '' print '*** inserting sample ad_type records'
GO
INSERT INTO [dbo].[ad_type]
	([ad_type])
	VALUES
		('TV'),
		('radio'),
		('billboards'),
		('posters'),
		('pamphlets')
GO

/* ad_company table */
print '' print '*** creating ad_company table'
GO 
CREATE TABLE [dbo].[ad_company] (
	[company_id] 	[int] IDENTITY(100000,1) 	NOT NULL,
	[company_name]	[NVARCHAR](50)				not null,
    [street_address]	[NVARCHAR](150) 		not null,
    [zip_code]		[CHAR](5)					not null,
    [phone_number]	[NVARCHAR](20)				not null,
	CONSTRAINT [pk_company_id] PRIMARY KEY([company_id]),
    CONSTRAINT [fk_company_zip_code] FOREIGN KEY ([zip_code])
		REFERENCES [zip_code]([zip_code])
)
CREATE INDEX [idx_ad_company_name] 
	ON [ad_company]([company_name]);
GO
print '' print '*** inserting sample ad_company records'
GO
INSERT INTO [dbo].[ad_company]
	([company_name], [street_address], [zip_code], [phone_number])
	VALUES
		('ads-R-us', '123 Sesamy St.', '36386', '(107) 133-1000'),
		('Get Attention', '2338 fake Ave.', '44444', '(211) 311-2989'),
		('CommCreative', '75 Fountain St.', '01702', '(877) 602-6664'),
		('Advert-EZ', '1960 Left Lane', '44444', '(211) 133-9892')
GO

/* ad_campaign table */
print '' print '*** creating ad_campaign table'
GO 
CREATE TABLE [dbo].[ad_campaign] (
	[campaign_id] 	[int] IDENTITY(100000,1) 	NOT NULL,
	[company_id]	[INT] 						not null,
    [total_cost]	[MONEY]						not null,
	[employee_id]	[INT]						not null,
	CONSTRAINT [pk_campaign_id] PRIMARY KEY([campaign_id]),
    CONSTRAINT	[fk_ad_campaign_company] FOREIGN KEY ([company_id])
		REFERENCES [ad_company]([company_id]),
	CONSTRAINT	[fk_ad_campaign_employee] FOREIGN KEY ([employee_id])
		REFERENCES [Employee]([employee_id])
)
GO
print '' print '*** inserting sample ad_campaign records'
GO
INSERT INTO [dbo].[ad_campaign]
	([company_id], [total_cost], [employee_id])
	VALUES
		(100000, 525.26, 100001),
		(100001, 600.00, 100001),
		(100003, 1025.69, 100001),
		(100000, 223.00, 100001),
		(100002, 903.33, 100001),
		(100003, 500.20, 100001)
GO

/* ad_item table */
print '' print '*** creating ad_item table'
GO 
CREATE TABLE [dbo].[ad_item] (
	[campaign_id]	[INT] 			not null,
    [ad_type]		[NVARCHAR](50)	not null,
    [focus_act]		[NVARCHAR](50)	null,
    [cost]			[MONEY]			not null,
    CONSTRAINT [fk_ad_item_campaign_id] FOREIGN KEY ([campaign_id])
		REFERENCES [ad_campaign]([campaign_id]),
	CONSTRAINT [fk_ad_item_type] FOREIGN KEY ([ad_type])
		REFERENCES [ad_type]([ad_type]),
	CONSTRAINT [pk_ad_item] PRIMARY KEY ([campaign_id], [ad_type]),
	CONSTRAINT [fk_ad_item_focus_act] FOREIGN KEY ([focus_act])
		REFERENCES [act]([act_name])
		
)
GO
print '' print '*** inserting sample ad_item records'
GO
INSERT INTO [dbo].[ad_item]
	([campaign_id], [ad_type], [focus_act], [cost])
VALUES
	(100000, 'radio', null, 400.00),
    (100000, 'pamphlets', null, 125.26),
    (100001, 'TV', null, 225.00),
    (100001, 'billboards', 'human cannon-ball', 375.00),
    (100002, 'posters', 'knife thrower', 125.69),
    (100002, 'TV', 'trapeze', 400.00),
    (100002, 'billboards', 'tight-rope walker', 500.00),
    (100003, 'TV', 'clowns', 223.00),
    (100004, 'radio', null, 400.00),
    (100004, 'posters', 'trapeze', 203.33),
    (100004, 'pamphlets', 'trapeze', 200.00),
    (100005, 'billboards', null, 500.20)

/* venue Stuff */

/* venue table */
print '' print '*** creating venue table'
GO 
CREATE TABLE [dbo].[venue] (
	[venue_id]		[INT]IDENTITY(100000,1)		not null,
    [venue_name]	[NVARCHAR](50)		not null,
    [street_address]	[NVARCHAR](50)	not null,
    [zip_code]		[CHAR](5)			not null,
    [phone_number]	[NVARCHAR](20)		not null,
    [terms_of_use]	[NVARCHAR](500)		not null,
	CONSTRAINT [pk_venue_id] PRIMARY KEY([venue_id]),
    CONSTRAINT [fk_venue_zip_code] FOREIGN KEY ([zip_code])
		REFERENCES [zip_code]([zip_code])
)
CREATE INDEX [idx_venue_name] 
	ON [venue]([venue_name]);
GO
print '' print '*** inserting sample venue records'
GO
INSERT INTO [dbo].[venue]
	([venue_name], [street_address], [zip_code], [phone_number], [terms_of_use])
VALUES
	('Example Venue', '111 Example St.', '11111', '(111) 111-1111', 'Pay 11% of the revenue from shows.'),
    ('Circus Space', '0691 Right Lane', '44444', '(400) 444-0691', 'Pay $400 per day'),
    ('Ford Prefecture', '42 Hitch-hiker Dr.', '36386', '(042) 308-1978', 'Pay $42 up front, 4.2% of the revenue from shows, and by drinks for the venue staff.'),
    ('Iowa State FairGrounds', '3000 E Grand Ave.', '50317', '(515) 262-3111', 'Pay $650 per day, any damages caused by visitors is our responsability'),
    ('Bannock County Fairgrounds', '10588 Fairground Dr.', '83202', '(208) 221-3656.', 'Pay $1,500 per week.'),
    ('Space For Rent', '543 Open Rd.', '01702', '(987) 654-3210', 'Pay $900 per day.')
GO

/* venue_use table */
print '' print '*** creating venue_use table'
GO 
CREATE TABLE [dbo].[venue_use] (
	[use_id]		[INT] IDENTITY(100000,1)	not null,
    [venue_id]		[INT] 		not null,
    [start_date]	[DATE] 		not null,
    [end_date]		[DATE] 		not null,
    [campaign_id]	[INT] 		  null,
    [total_tickets_sold]	[INT] 	  DEFAULT 0,
    [total_revenue]	[MONEY]		DEFAULT 0,
	[employee_id]	[INT]		not null,
	CONSTRAINT [pk_use_id] PRIMARY KEY([use_id]),
    CONSTRAINT [fk_venue_use_venue_id] FOREIGN KEY ([venue_id])
		REFERENCES [venue]([venue_id]),
	CONSTRAINT	[fk_venue_use_ad_campaign_id] FOREIGN KEY ([campaign_id])
		REFERENCES [ad_campaign]([campaign_id]),
	CONSTRAINT  [fk_venue_use_employee_id] FOREIGN KEY ([employee_id])
		REFERENCES [Employee]([employee_id])
)
CREATE INDEX [idx_venue_use_start_date] 
	ON [venue_use]([start_date]);
CREATE INDEX [idx_venue_use_revenue]
	ON [venue_use]([total_revenue]);
GO
print '' print '*** inserting sample venue_use records'
GO
INSERT INTO [dbo].[venue_use]
	([venue_id], [start_date], [end_date], [campaign_id], [total_tickets_sold], [total_revenue], [employee_id])
	VALUES
		(100000, '2020-07-14', '2020-07-20', 100000, 231, 1039.50, 100000),
		(100002, '2020-08-05', '2020-08-07', 100001, 125, 562.50, 100000),
		(100001, '2020-08-14', '2020-08-16', 100001, 132, 594.00, 100000),
		(100003, '2021-05-01', '2021-05-05', 100002, 145, 690.20, 100000),
		(100000, '2021-05-12', '2021-05-15', 100002, 126, 598.50, 100000),
		(100004, '2021-05-22', '2021-05-29', 100002, 222, 1056.72, 100000),
		(100002, '2021-06-17', '2021-06-27', 100003, 191,  909.16, 100000),
		(100003, '2021-07-12', '2021-08-12', 100004, 1027, 4888.52, 100000),
		(100000, '2021-08-20', '2021-08-23', 100005, 156, 742.56, 100000),
		(100004, '2022-05-19', '2022-06-02', null, default, default, 100000)
GO

/* use_day table */
print '' print '*** creating use_day table'
GO 
CREATE TABLE [dbo].[use_day] (
	[use_id]		[INT] 	not null,
    [use_date]		[DATE] 	not null,
    [tickets_sold]	[INT] 	DEFAULT 0,
    [revenue]		[MONEY]	DEFAULT 0,
	[employee_id]	[INT]	not null,
    CONSTRAINT [fk_use_id] FOREIGN KEY ([use_id])
		REFERENCES [venue_use] ([use_id]),
    CONSTRAINT [pk_use_date] PRIMARY KEY ([use_id], [use_date]),
	CONSTRAINT [fk_use_day_employee_id] FOREIGN KEY ([employee_id])
		REFERENCES [Employee] ([employee_id])
)
GO
print '' print '*** inserting sample use_day records'
GO
INSERT INTO [dbo].[use_day]
	([use_id], [use_date], [tickets_sold], [revenue], [employee_id])
	VALUES
		(100000, '2020-07-14', 23, 103.50, 100000),
		(100000, '2020-07-15', 34, 153.00, 100000),
		(100000, '2020-07-16', default, default, 100000),
		(100000, '2020-07-17', 50, 225.00, 100000),
		(100000, '2020-07-18', default, default, 100000),
		(100000, '2020-07-19', 63, 283.50, 100000),
		(100000, '2020-07-20', 61, 274.50, 100000),
		(100001, '2020-08-06', 52, 312.00, 100000),
		(100002, '2020-08-14', 43, 258.00, 100000),
		(100003, '2021-05-01', 34, 204.00, 100000),
		(100004, '2021-05-13', 40, 234.38, 100000),
		(100005, '2021-05-24', 77, 400.50, 100000),
		(100006, '2021-06-23', 20, 101.03, 100000),
		(100007, '2021-08-01', 50, 222.22, 100000),
		(100008, '2021-08-20', 36, 156.50, 100000)
GO

/* stored Procedures for log-in */

print '' print '*** creating sp_authenticate_user'
GO 
CREATE PROCEDURE [dbo].[sp_authenticate_user_cd]
(
	@Email			[nvarchar](100),
	@PasswordHash	[nvarchar](100)
)
AS 
	BEGIN
		SELECT COUNT([employee_id]) AS 'Authenticated'
		FROM	[Employee]
		WHERE	@Email = [email]
			AND @PasswordHash = [password_hash]
			AND [active] = 1
	
	END
GO

print '' print '*** creationg sp_select_employee_by_email'
GO 
CREATE PROCEDURE [dbo].[sp_select_employee_by_email_cd]
(
	@Email			[nvarchar](100)
)
AS 
	BEGIN
		SELECT 	[employee_id], [given_name], [family_name],	
				[phone_number], [email], [active]		
		FROM 	[Employee]
		WHERE 	@Email = [email]
	END
GO

print '' print '*** creating  sp_select_roles_by_employeeID'
GO
CREATE PROCEDURE [dbo].[sp_select_roles_by_employeeID]
(
	@EmployeeID			[int]
)
AS 
	BEGIN
		SELECT 	[role_id]		
		FROM 	[EmployeeRole]
		WHERE 	@EmployeeID = [employee_id]
	END
GO

/* stored procedurs for working with venues */

print '' print '*** creating vw_venue_use_stats'
GO
CREATE VIEW [vw_venue_use_stats]
AS
	SELECT  [use_id], [venue_id], [end_date], DATEDIFF(day, [start_date], [end_date]) AS 'days',
			ROUND([total_tickets_sold] / DATEDIFF(day, [start_date], [end_date]), 0) AS 'average_tickets_sold',
			ROUND([total_revenue] / DATEDIFF(day, [start_date], [end_date]), 2) AS 'average_revenue'
	FROM [venue_use]

GO

print '' print '*** creating sp_select_venues_with_stats'
GO
CREATE PROCEDURE [sp_select_venues_with_stats]
AS
	BEGIN
		SELECT [venue].[venue_id], [venue].[venue_name], [venue].[street_address], [venue].[zip_code],
				[venue].[terms_of_use], [venue].[phone_number],
				AVG([vw_venue_use_stats].[average_tickets_sold]) AS 'average_tickets_sold',
				AVG([vw_venue_use_stats].[average_revenue]) AS 'average_revenue',
				MAX([vw_venue_use_stats].[end_date]) AS 'last_used_on'
		FROM [venue] LEFT JOIN [vw_venue_use_stats]
			ON [venue].[venue_id] = [vw_venue_use_stats].[venue_id]
		GROUP BY [venue].[venue_id], [venue].[venue_name], [venue].[street_address], [venue].[zip_code], 
		[venue].[terms_of_use], [venue].[phone_number]
	END
GO

print '' print '*** creating sp_select_city_and_state_by_zipcode'
GO
CREATE PROCEDURE [sp_select_city_and_state_by_zipcode](
	@zip_code		[CHAR](5)
)
AS
	BEGIN
		SELECT [city], [state]
		FROM [zip_code]
		WHERE [zip_code].[zip_code] = @zip_code
	END
GO

/* stored procedures for venue_use */

print '' print '*** creating sp_insert_venue_use'
GO
CREATE PROCEDURE [sp_insert_venue_use](
	@venue_id		[INT], 
	@start_date		[DATE],
	@end_date		[DATE],
	@employee_id	[INT]
)
AS
	BEGIN
		INSERT INTO [dbo].[venue_use]
			([venue_id], [start_date], [end_date], [employee_id])
		VALUES
			(@venue_id, @start_date, @end_date, @employee_id)
	END
GO

print '' print '*** creating sp_select_venue_uses'
GO
CREATE PROCEDURE [sp_select_venue_uses]
AS
	BEGIN
		SELECT [venue_use].[use_id], [venue_use].[venue_id], [venue].[venue_name], [venue].[street_address], [venue].[zip_code],
			[zip_code].[city], [zip_code].[state],
			[start_date], [end_date], [campaign_id], [total_tickets_sold], [total_revenue]
		FROM [venue_use] JOIN [venue]
			ON [venue_use].[venue_id] = [venue].[venue_id]
			JOIN [zip_code] ON [venue].[zip_code] = [zip_code].[zip_code]
	END
GO

print '' print '*** creating sp_select_use_days_by_use_id'
GO
CREATE PROCEDURE [sp_select_use_days_by_use_id](
	@use_id			[INT]
)
AS
	BEGIN
		SELECT [use_id], [use_date], [tickets_sold], [revenue]
		FROM [use_day]
		WHERE [use_id] = @use_id
	END
GO

print '' print '*** creating sp_insert_use_day'
GO
CREATE PROCEDURE [sp_insert_use_day](
	@use_id			[INT],
	@date 			[DATE],
	@tickets_sold 	[INT],
	@revenue		[MONEY],
	@employee_id	[INT]
)
AS
	BEGIN
		INSERT INTO [dbo].[use_day]
			([use_id], [use_date], [tickets_sold], [revenue], [employee_id])
		VALUES
			(@use_id, @date, @tickets_sold, @revenue, @employee_id)
		;
		UPDATE [venue_use]
		SET [total_tickets_sold] = [total_tickets_sold] + @tickets_sold,
			[total_revenue] = [total_revenue] + @revenue
		WHERE [use_id] = @use_id
		;
	END
GO

/* stored procedures for ad_companies */

print '' print '*** creating sp_select_ad_companies'
GO
CREATE PROCEDURE [sp_select_ad_companies]
AS
	BEGIN
		SELECT [ad_company].[company_id], [ad_company].[company_name], [ad_company].[street_address], [ad_company].[zip_code],
			[zip_code].[city], [zip_code].[state], [ad_company].[phone_number]
		FROM [ad_company] JOIN [zip_code] 
			ON [ad_company].[zip_code] = [zip_code].[zip_code]
	END
GO

/* stored procedures for ad_campaign */

print '' print '*** creating procedure sp_insert_ad_campaign'
GO
CREATE PROCEDURE [sp_insert_ad_campaign](
	@company_id			[INT],
	@total_cost			[MONEY],
	@employee_id		[INT]
)
AS
	BEGIN
		INSERT INTO [dbo].[ad_campaign]
			([company_id], [total_cost], [employee_id])
		VALUES
			(@company_id, @total_cost, @employee_id)
		;
		SELECT MAX([campaign_id])
		FROM [ad_campaign]
		;
	END
GO

print '' print '*** creating procedure sp_insert_ad_item'
GO
CREATE PROCEDURE [sp_insert_ad_item](
	@campaign_id		[INT],
	@ad_type			[NVARCHAR](50),
	@focus_act			[NVARCHAR](50)		null,
	@cost				[MONEY]
)
AS
	BEGIN
		INSERT INTO [dbo].[ad_item]
			([campaign_id], [ad_type], [focus_act], [cost])
		VALUES
			(@campaign_id, @ad_type, @focus_act, @cost)
	END
GO

print '' print '*** creating sp_select_ad_types'
GO
CREATE PROCEDURE [sp_select_ad_types]
AS
	BEGIN
		SELECT [ad_type]
		FROM [ad_type]
	END
GO

print '' print '*** creating sp_select_acts'
GO
CREATE PROCEDURE [sp_select_acts]
AS
	BEGIN
		SELECT [act_name]
		FROM [act]
	END
GO

print '' print '*** creating vw_ad_campaign_stats'
GO
CREATE VIEW [vw_ad_campaign_stats]
AS
	SELECT  [ad_campaign].[campaign_id], [ad_campaign].[total_cost] AS 'campaign_total_cost',
		ROUND(AVG([vw_venue_use_stats].[average_tickets_sold]), 0) AS 'average_tickets_sold', 
        ROUND(AVG([vw_venue_use_stats].[average_revenue]), 2) AS 'average_revenue_per_day'
	FROM [ad_campaign]
		LEFT JOIN [venue_use]
			ON [ad_campaign].[campaign_id] = [venue_use].[campaign_id]
		LEFT JOIN [vw_venue_use_stats]
			ON [venue_use].[use_id] = [vw_venue_use_stats].[use_id]
	GROUP BY [venue_use].[campaign_id], [ad_campaign].[campaign_id], [ad_campaign].[total_cost]

GO

print '' print '*** creating sp_select_ad_campaigns'
GO
CREATE PROCEDURE [sp_select_ad_campaigns]
AS
	BEGIN
		SELECT [ad_campaign].[campaign_id], [ad_campaign].[company_id], [ad_company].[company_name],
			[ad_campaign].[total_cost], [vw_ad_campaign_stats].[average_tickets_sold], 
			[vw_ad_campaign_stats].[average_revenue_per_day]
		FROM [ad_campaign] JOIN [vw_ad_campaign_stats]
			ON [ad_campaign].[campaign_id] = [vw_ad_campaign_stats].[campaign_id]
			JOIN [ad_company] ON [ad_campaign].[company_id] = [ad_company].[company_id]
	END
GO

print '' print '*** creating sp_select_ad_items_by_campaign_id'
GO
CREATE PROCEDURE [sp_select_ad_items_by_campaign_id](
	@campaign_id			[INT]
)
AS
	BEGIN
		SELECT [campaign_id], [ad_type], [focus_act], [cost]
		FROM [ad_item]
		WHERE [campaign_id] = @campaign_id
	END
GO

print '' print '*** creating sp_update_venueuse_adcampaign'
GO
CREATE PROCEDURE [sp_update_venueuse_adcampaign](
	@use_id					[INT],
	@campaign_id			[INT]
)
AS
	BEGIN
		UPDATE [venue_use]
		SET [campaign_id] = @campaign_id
		WHERE [use_id] = @use_id
	END
GO

