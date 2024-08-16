CREATE DATABASE EsemkaHeHo;
GO 
USE EsemkaHeHo;
GO

CREATE TABLE Departments (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL UNIQUE,
    Description TEXT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),  
    ModifiedDate DATETIME NULL
);

CREATE TABLE Users (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    DepartmentId UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES Departments(Id),  
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL, 
    Role INT NOT NULL, -- (0: Developer, 1: Designer, 2: Manager, 3: QA, 4: Admin)
    PhoneNumber VARCHAR(20) NULL, 
    Address VARCHAR(255) NULL, 
    DateOfBirth DATE NULL,  
    HireDate DATE NULL,  
    Salary DECIMAL(18, 2) NULL, 
    IsActive BIT NOT NULL DEFAULT 1, -- Status aktif (1: Active, 0: Non Active) 
    Photo VARCHAR(1000) NULL 
);


CREATE TABLE Projects (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	ProjectManagerId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id),
    Name VARCHAR(100) NOT NULL,
    Description TEXT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    Status INT NOT NULL -- (0: Planned, 1: InProgress, 2: Completed, 3: Cancelled)
);
 
CREATE TABLE Tasks (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    ProjectId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Projects(Id),
    Title VARCHAR(100) NOT NULL,
    Description TEXT NULL,
    DueDate DATETIME NOT NULL,
	CategoryIssue INT NOT NULL, -- (0: New Task, 1: New Request, 2: Bug, 3: Error, 4: Case Issue)
    Priority INT NOT NULL, -- (0: Low, 1: Medium, 2: High)
    Status INT NOT NULL -- (0: Not Started, 1: In Progress, 2: Completed, 3: Blocked)
);

CREATE TABLE AssignedTasks (
    TaskId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Tasks(Id),
    UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id), 
    PRIMARY KEY (TaskId, UserId)
);
 
INSERT INTO Departments (Id, Name, Description, CreatedDate, ModifiedDate)
VALUES
    (NEWID(), 'Development', 'Department responsible for software development.', GETDATE(), NULL),
    (NEWID(), 'Design', 'Department responsible for design and user experience.', GETDATE(), NULL),
    (NEWID(), 'Project Management', 'Department responsible for overseeing projects.', GETDATE(), NULL),
    (NEWID(), 'Quality Assurance', 'Department responsible for quality testing.', GETDATE(), NULL),
    (NEWID(), 'Administration', 'Department responsible for administrative tasks.', GETDATE(), NULL),
    (NEWID(), 'Marketing', 'Department responsible for marketing and promotions.', GETDATE(), NULL),
    (NEWID(), 'Sales', 'Department responsible for sales and customer relations.', GETDATE(), NULL),
    (NEWID(), 'Customer Support', 'Department responsible for customer support and service.', GETDATE(), NULL),
    (NEWID(), 'HR', 'Department responsible for human resources and employee relations.', GETDATE(), NULL),
    (NEWID(), 'Finance', 'Department responsible for financial management and accounting.', GETDATE(), NULL),
    (NEWID(), 'Legal', 'Department responsible for legal affairs and compliance.', GETDATE(), NULL),
    (NEWID(), 'IT Support', 'Department responsible for IT support and maintenance.', GETDATE(), NULL),
    (NEWID(), 'Procurement', 'Department responsible for purchasing and procurement.', GETDATE(), NULL),
    (NEWID(), 'Research and Development', 'Department responsible for innovation and product development.', GETDATE(), NULL),
    (NEWID(), 'Logistics', 'Department responsible for logistics and supply chain management.', GETDATE(), NULL),
    (NEWID(), 'Public Relations', 'Department responsible for public relations and media communication.', GETDATE(), NULL),
    (NEWID(), 'Training and Development', 'Department responsible for employee training and skill development.', GETDATE(), NULL),
    (NEWID(), 'Facilities Management', 'Department responsible for managing facilities and infrastructure.', GETDATE(), NULL),
    (NEWID(), 'Compliance', 'Department responsible for regulatory compliance and auditing.', GETDATE(), NULL),
    (NEWID(), 'Corporate Strategy', 'Department responsible for corporate planning and strategy.', GETDATE(), NULL),
    (NEWID(), 'Risk Management', 'Department responsible for assessing and managing risks.', GETDATE(), NULL),
    (NEWID(), 'Operations', 'Department responsible for day-to-day operations and process optimization.', GETDATE(), NULL),
    (NEWID(), 'Business Development', 'Department responsible for identifying and developing business opportunities.', GETDATE(), NULL),
    (NEWID(), 'Customer Experience', 'Department responsible for enhancing customer experiences.', GETDATE(), NULL),
    (NEWID(), 'Data Analytics', 'Department responsible for data analysis and insights.', GETDATE(), NULL),
    (NEWID(), 'Security', 'Department responsible for ensuring the security of systems and information.', GETDATE(), NULL),
    (NEWID(), 'Corporate Communications', 'Department responsible for internal and external communications.', GETDATE(), NULL),
    (NEWID(), 'Sustainability', 'Department responsible for environmental and sustainability initiatives.', GETDATE(), NULL),
    (NEWID(), 'Internal Audit', 'Department responsible for internal audits and controls.', GETDATE(), NULL),
    (NEWID(), 'Innovation', 'Department responsible for fostering innovation within the organization.', GETDATE(), NULL);


INSERT INTO Users (Password, Id, DepartmentId, FullName, Email, Role, PhoneNumber, Address, DateOfBirth, HireDate, Salary, IsActive, Photo)
VALUES
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'Development'), 'John Doe', 'john.doe@example.com', 0, '555-1234', '123 Elm St, Springfield', '1985-03-12', '2020-01-15', 75000.00, 1, 'images/johndoe.jpg'),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'Design'), 'Jane Smith', 'jane.smith@example.com', 1, '555-5678', '456 Oak St, Springfield', '1990-07-22', '2021-05-10', 68000.00, 1,  'images/janesmith.jpg'),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'Project Management'), 'Michael Johnson', 'michael.johnson@example.com', 2, '555-8765', '789 Pine St, Springfield', '1982-11-30', '2018-03-12', 85000.00, 1, 'images/michaeljohnson.jpg'),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'Quality Assurance'), 'Emily Davis', 'emily.davis@example.com', 3, '555-4321', '101 Maple St, Springfield', '1988-09-05', '2019-07-19', 70000.00, 1, NULL),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'Administration'), 'Alice Walker', 'alice.walker@example.com', 4, '555-9999', '202 Main St, Springfield', '1980-05-14', '2017-10-01', NULL, 1,  NULL),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'Marketing'), 'Tom Brown', 'tom.brown@example.com', 4, '555-3456', '202 Cedar St, Springfield', '1992-02-25', '2021-09-01', 72000.00, 1, NULL),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'Sales'), 'Sarah Wilson', 'sarah.wilson@example.com', 4, '555-6543', '303 Birch St, Springfield', '1985-11-17', '2016-06-15', 77000.00, 1,  NULL),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'Customer Support'), 'David Lee', 'david.lee@example.com', 4, '555-7890', '404 Spruce St, Springfield', '1993-08-14', '2019-12-01', 69000.00, 1,  NULL),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'HR'), 'Nina Martinez', 'nina.martinez@example.com', 4, '555-0123', '505 Fir St, Springfield', '1988-12-21', '2018-04-05', 71000.00, 1,  NULL),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'Finance'), 'James Taylor', 'james.taylor@example.com', 4, '555-4567', '606 Pine St, Springfield', '1979-06-09', '2017-11-25', 79000.00, 1, NULL),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'Development'), 'Lisa Green', 'lisa.green@example.com', 0, '555-2345', '707 Oak St, Springfield', '1987-03-19', '2020-11-10', 76000.00, 1, NULL),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'Design'), 'Mark Adams', 'mark.adams@example.com', 1, '555-6789', '808 Maple St, Springfield', '1991-07-14', '2022-06-25', 69000.00, 1,  NULL),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'Project Management'), 'Linda Scott', 'linda.scott@example.com', 2, '555-9876', '909 Cedar St, Springfield', '1984-10-31', '2021-03-10', 86000.00, 1,  NULL),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'Quality Assurance'), 'Paul Harris', 'paul.harris@example.com', 3, '555-5432', '1010 Birch St, Springfield', '1990-09-09', '2020-07-19', 71000.00, 1,  NULL),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'Marketing'), 'Sophia Adams', 'sophia.adams@example.com', 4, '555-3457', '1111 Spruce St, Springfield', '1995-01-30', '2022-05-10', 73000.00, 1, NULL),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'Sales'), 'Robert Miller', 'robert.miller@example.com', 4, '555-7654', '1212 Fir St, Springfield', '1987-11-20', '2019-10-30', 78000.00, 1, NULL),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'Customer Support'), 'Laura Wilson', 'laura.wilson@example.com', 4, '555-8901', '1313 Pine St, Springfield', '1994-05-22', '2020-02-15', 70000.00, 1, NULL),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'HR'), 'Steven Clark', 'steven.clark@example.com', 4, '555-3210', '1414 Maple St, Springfield', '1989-04-11', '2019-08-01', 72000.00, 1, NULL),
    ('P@ssw0rd1123', NEWID(), (SELECT Id FROM Departments WHERE Name = 'Finance'), 'Olivia Davis', 'olivia.davis@example.com', 4, '555-6540', '1515 Oak St, Springfield', '1981-12-15', '2022-02-10', 80000.00, 1, NULL);

INSERT INTO Projects (Id, ProjectManagerId, Name, Description, StartDate, EndDate, Status)
VALUES
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Michael Johnson'), 'Project Alpha', 'A major project focusing on new features for the product.', '2024-01-01', '2024-12-31', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Michael Johnson'), 'Project Beta', 'An internal project aimed at improving internal tools.', '2024-03-01', '2024-11-30', 0),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Linda Scott'), 'Project Gamma', 'A new initiative to explore emerging technologies.', '2024-02-01', '2024-10-31', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Sarah Wilson'), 'Project Delta', 'A project focusing on customer experience improvements.', '2024-04-01', '2024-09-30', 0),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'James Taylor'), 'Project Epsilon', 'A project for developing a new financial tool.', '2024-05-01', '2024-12-01', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Robert Miller'), 'Project Zeta', 'A project aimed at expanding market reach.', '2024-06-01', '2024-11-30', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Paul Harris'), 'Project Eta', 'An internal project for system enhancements.', '2024-07-01', '2024-10-31', 0),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Sophia Adams'), 'Project Theta', 'A project for developing a new marketing campaign.', '2024-08-01', '2024-12-15', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Nina Martinez'), 'Project Iota', 'A project for HR system improvements.', '2024-09-01', '2024-11-15', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Emily Davis'), 'Project Kappa', 'A QA project for testing new product features.', '2024-10-01', '2024-12-30', 0),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Linda Scott'), 'Project Lambda', 'A project to enhance project management tools.', '2024-02-01', '2024-09-30', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Michael Johnson'), 'Project Mu', 'A project to develop a new feature for the product.', '2024-01-15', '2024-12-01', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Robert Miller'), 'Project Nu', 'A project focused on market analysis and growth.', '2024-03-01', '2024-11-01', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Sophia Adams'), 'Project Xi', 'A marketing project aimed at brand development.', '2024-06-01', '2024-12-15', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'James Taylor'), 'Project Omicron', 'A project for financial systems upgrade.', '2024-04-15', '2024-10-31', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Paul Harris'), 'Project Pi', 'A project for automated testing implementation.', '2024-07-01', '2024-12-01', 0),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Sarah Wilson'), 'Project Rho', 'A project for improving customer satisfaction.', '2024-05-01', '2024-09-30', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Linda Scott'), 'Project Sigma', 'A project to streamline project management processes.', '2024-03-10', '2024-11-30', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Michael Johnson'), 'Project Tau', 'A strategic project for business development.', '2024-02-15', '2024-12-31', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Robert Miller'), 'Project Upsilon', 'A project for international market expansion.', '2024-06-01', '2024-12-31', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Sophia Adams'), 'Project Phi', 'A marketing campaign for a new product launch.', '2024-08-01', '2024-12-15', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'James Taylor'), 'Project Chi', 'A financial analysis project for budget optimization.', '2024-05-01', '2024-11-30', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Paul Harris'), 'Project Psi', 'A QA project for system reliability testing.', '2024-07-15', '2024-12-31', 0),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Emily Davis'), 'Project Omega', 'A project focused on final product testing.', '2024-09-01', '2024-12-31', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Nina Martinez'), 'Project Alpha-2', 'A project for improving employee engagement.', '2024-10-01', '2024-12-31', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Michael Johnson'), 'Project Beta-2', 'A project for enhancing internal collaboration tools.', '2024-01-01', '2024-12-01', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Linda Scott'), 'Project Gamma-2', 'A project to explore new technologies for product development.', '2024-02-01', '2024-10-31', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Sarah Wilson'), 'Project Delta-2', 'A project focusing on customer experience improvements for international clients.', '2024-04-01', '2024-09-30', 0),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'James Taylor'), 'Project Epsilon-2', 'A project for developing new financial strategies.', '2024-05-01', '2024-12-01', 1),
    (NEWID(), (SELECT Id FROM Users WHERE FullName = 'Robert Miller'), 'Project Zeta-2', 'A project aimed at expanding market reach in new regions.', '2024-06-01', '2024-11-30', 1);

 
INSERT INTO Tasks (Id, ProjectId, Title, Description, DueDate, CategoryIssue, Priority, Status)
VALUES
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Alpha'), 'Implement New Feature', 'Develop and implement a new feature for the product.', '2024-06-30', 0, 2, 1),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Alpha'), 'Fix Bug in Module X', 'Resolve the bug causing crashes in Module X.', '2024-04-15', 2, 1, 0),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Beta'), 'Upgrade Internal Tool', 'Upgrade the existing internal tool to the latest version.', '2024-09-30', 1, 1, 1),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Beta'), 'Address Security Flaw', 'Fix the security vulnerability found in the internal tool.', '2024-08-15', 3, 2, 0),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Gamma'), 'Research New Technologies', 'Conduct research on emerging technologies.', '2024-05-31', 0, 2, 1),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Gamma'), 'Prepare Tech Report', 'Prepare a comprehensive report on new technologies.', '2024-07-15', 1, 1, 1),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Delta'), 'Enhance Customer Experience', 'Implement features to improve customer experience.', '2024-08-31', 2, 2, 0),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Delta'), 'Collect Customer Feedback', 'Gather feedback from customers on recent changes.', '2024-09-30', 1, 1, 1),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Epsilon'), 'Develop Financial Tool', 'Create a new financial management tool.', '2024-11-30', 0, 2, 1),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Epsilon'), 'Test Financial Tool', 'Conduct testing on the new financial tool.', '2024-12-15', 3, 2, 0),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Zeta'), 'Market Expansion Plan', 'Develop a strategy for expanding market reach.', '2024-10-31', 1, 2, 1),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Zeta'), 'Implement Marketing Strategy', 'Execute the marketing expansion plan.', '2024-11-30', 1, 1, 1),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Eta'), 'System Enhancements', 'Enhance internal systems and processes.', '2024-09-30', 2, 1, 0),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Eta'), 'Internal Testing', 'Test system enhancements internally.', '2024-10-15', 0, 2, 1),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Theta'), 'Develop Marketing Campaign', 'Create and launch a new marketing campaign.', '2024-12-01', 1, 2, 1),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Theta'), 'Review Campaign Performance', 'Analyze and review the performance of the campaign.', '2024-12-15', 2, 1, 1),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Iota'), 'Improve HR System', 'Upgrade and improve the HR system.', '2024-11-15', 0, 2, 1),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Iota'), 'Train HR Staff', 'Provide training for HR staff on new system features.', '2024-12-01', 1, 1, 0),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Kappa'), 'QA Testing of New Features', 'Conduct QA testing on new product features.', '2024-12-30', 2, 1, 0),
    (NEWID(), (SELECT Id FROM Projects WHERE Name = 'Project Kappa'), 'Bug Fixes and Reporting', 'Fix identified bugs and prepare testing reports.', '2024-12-15', 3, 2, 1);

INSERT INTO AssignedTasks (TaskId, UserId)
VALUES
    ((SELECT Id FROM Tasks WHERE Title = 'Implement New Feature'), (SELECT Id FROM Users WHERE FullName = 'John Doe')),
    ((SELECT Id FROM Tasks WHERE Title = 'Fix Bug in Module X'), (SELECT Id FROM Users WHERE FullName = 'John Doe')),
    ((SELECT Id FROM Tasks WHERE Title = 'Upgrade Internal Tool'), (SELECT Id FROM Users WHERE FullName = 'Jane Smith')),
    ((SELECT Id FROM Tasks WHERE Title = 'Address Security Flaw'), (SELECT Id FROM Users WHERE FullName = 'Emily Davis')),
    ((SELECT Id FROM Tasks WHERE Title = 'Research New Technologies'), (SELECT Id FROM Users WHERE FullName = 'Michael Johnson')),
    ((SELECT Id FROM Tasks WHERE Title = 'Prepare Tech Report'), (SELECT Id FROM Users WHERE FullName = 'Michael Johnson')),
    ((SELECT Id FROM Tasks WHERE Title = 'Enhance Customer Experience'), (SELECT Id FROM Users WHERE FullName = 'Sarah Wilson')),
    ((SELECT Id FROM Tasks WHERE Title = 'Collect Customer Feedback'), (SELECT Id FROM Users WHERE FullName = 'Sarah Wilson')),
    ((SELECT Id FROM Tasks WHERE Title = 'Develop Financial Tool'), (SELECT Id FROM Users WHERE FullName = 'James Taylor')),
    ((SELECT Id FROM Tasks WHERE Title = 'Test Financial Tool'), (SELECT Id FROM Users WHERE FullName = 'James Taylor')),
    ((SELECT Id FROM Tasks WHERE Title = 'Market Expansion Plan'), (SELECT Id FROM Users WHERE FullName = 'Robert Miller')),
    ((SELECT Id FROM Tasks WHERE Title = 'Implement Marketing Strategy'), (SELECT Id FROM Users WHERE FullName = 'Robert Miller')),
    ((SELECT Id FROM Tasks WHERE Title = 'System Enhancements'), (SELECT Id FROM Users WHERE FullName = 'Paul Harris')),
    ((SELECT Id FROM Tasks WHERE Title = 'Internal Testing'), (SELECT Id FROM Users WHERE FullName = 'Paul Harris')),
    ((SELECT Id FROM Tasks WHERE Title = 'Develop Marketing Campaign'), (SELECT Id FROM Users WHERE FullName = 'Sophia Adams')),
    ((SELECT Id FROM Tasks WHERE Title = 'Review Campaign Performance'), (SELECT Id FROM Users WHERE FullName = 'Sophia Adams')),
    ((SELECT Id FROM Tasks WHERE Title = 'Improve HR System'), (SELECT Id FROM Users WHERE FullName = 'Nina Martinez')),
    ((SELECT Id FROM Tasks WHERE Title = 'Train HR Staff'), (SELECT Id FROM Users WHERE FullName = 'Nina Martinez')),
    ((SELECT Id FROM Tasks WHERE Title = 'QA Testing of New Features'), (SELECT Id FROM Users WHERE FullName = 'Emily Davis')),
    ((SELECT Id FROM Tasks WHERE Title = 'Bug Fixes and Reporting'), (SELECT Id FROM Users WHERE FullName = 'Emily Davis'));