CREATE TABLE Cabinet
(
id INT NOT NULL IDENTITY,
personCapacity INT NOT NULL,
CONSTRAINT PK_Cabinet PRIMARY KEY (id)
);

CREATE TABLE EquipmentType
(
id INT NOT NULL IDENTITY,
name NVARCHAR(256),
CONSTRAINT PK_EquipmentType PRIMARY KEY (id)
);

CREATE TABLE Equipment
(
id INT NOT NULL IDENTITY,
equipmentTypeId INT,
cabinetId INT,
CONSTRAINT PK_Equipment PRIMARY KEY (id)
);

CREATE TABLE Researcher
(
researchId INT,
id INT NOT NULL IDENTITY,
firstName NVARCHAR(256) NOT NULL,
lastName NVARCHAR(256) NOT NULL,
degree BIT,
CONSTRAINT PK_Researcher PRIMARY KEY (id)
);

CREATE TABLE RawMaterial
(
id INT NOT NULL IDENTITY,
name NVARCHAR(256) NOT NULL,
materialWeight INT NOT NULL,
researcherId INT,
CONSTRAINT PK_RawMaterial PRIMARY KEY (id)
);

CREATE TABLE ResearcherCabinet
(
id INT NOT NULL IDENTITY,
researcherId INT,
cabinetId INT,
resCabDate DATE NOT NULL,
CONSTRAINT PK_ResearcherCabinet PRIMARY KEY (id)
);

CREATE TABLE ResearchScope
(
id INT NOT NULL IDENTITY,
name NVARCHAR(256) NOT NULL,
scopePriority INT NOT NULL,
CONSTRAINT ResearchScope_ScopePriority CHECK (scopePriority >= 1),
CONSTRAINT PK_ResearchScope PRIMARY KEY (id)
);

CREATE TABLE Sponsor
(
id INT NOT NULL IDENTITY,
name NVARCHAR(256) NOT NULL,
CONSTRAINT PK_Sponsor PRIMARY KEY (id)
);

CREATE TABLE Research
(
id INT NOT NULL IDENTITY,
name NVARCHAR(256) NOT NULL,
scopeId INT,
CONSTRAINT PK_Research PRIMARY KEY (id)
);

CREATE TABLE Sponsorship
(
id INT NOT NULL IDENTITY,
sponsorId INT NOT NULL,
researchId INT NOT NULL,
price DECIMAL(16, 2) NOT NULL,
CONSTRAINT PK_Sponsorship PRIMARY KEY (id)
);

ALTER TABLE Equipment ADD FOREIGN KEY (equipmentTypeId) REFERENCES EquipmentType (id) ON DELETE SET NULL;

ALTER TABLE Equipment ADD CONSTRAINT FK_Equipment_Cabinet FOREIGN KEY (cabinetId) REFERENCES Cabinet (id) ON DELETE CASCADE;

ALTER TABLE Researcher ADD CONSTRAINT FK_Researcher_Research FOREIGN KEY (researchId) REFERENCES Research (id) ON DELETE SET NULL;

ALTER TABLE RawMaterial ADD CONSTRAINT FK_RawMaterial_Researcher FOREIGN KEY (researcherId) REFERENCES Researcher (id) ON DELETE SET NULL;

ALTER TABLE ResearcherCabinet ADD CONSTRAINT FK_ResearcherCabinet_Researcher FOREIGN KEY (researcherId) REFERENCES Researcher (id) ON DELETE CASCADE;

ALTER TABLE ResearcherCabinet ADD CONSTRAINT FK_ReseacherCabinet_Cabinet FOREIGN KEY (cabinetId) REFERENCES Cabinet (id) ON DELETE CASCADE;

ALTER TABLE Research ADD CONSTRAINT FK_Research_ResearchScope FOREIGN KEY (scopeId) REFERENCES ResearchScope (id) ON DELETE SET NULL;

ALTER TABLE Sponsorship ADD CONSTRAINT FK_Sponsorship_Sponsor FOREIGN KEY (sponsorId) REFERENCES Sponsor (id) ON DELETE CASCADE;

ALTER TABLE Sponsorship ADD CONSTRAINT FK_Sponsorship_Research FOREIGN KEY (researchId) REFERENCES Research (id) ON DELETE CASCADE;