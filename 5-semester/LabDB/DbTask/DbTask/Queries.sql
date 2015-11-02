-- Запросы

-- 1. Единиицы работы ученых в кабинетах, относещееся к даному исследованию
SELECT ResCab.id, ResCab.resCabDate, Researcher.firstName, Researcher.lastName FROM
ResearcherCabinet ResCab 
JOIN Researcher ON ResCab.researcherId = Researcher.id
JOIN Research ON 
	Researcher.researchId = Research.id AND
    Research.name = 'Slow recombination in quantum dot solid solar cell using p–i–n architecture with organic p-type hole transport material'

-- 2. Сколько инвестировали в каждую область, отсортировано по приоритету области
SELECT ResearchScope.name, SUM(Sponsorship.price) amountSponsored FROM
Sponsorship
JOIN Research ON Sponsorship.researchId = Research.id
JOIN ResearchScope ON Research.scopeId = ResearchScope.id
GROUP BY ResearchScope.name, ResearchScope.scopePriority
ORDER BY ResearchScope.scopePriority

-- 3. Количество оборудования, используемого в данный день для различных ислледований
SELECT Research.id, SUM(EquipmentUsageUnit.equipmentCount) totalEquipmentCount FROM (
SELECT ResearcherCabinet.researcherId, COUNT(Equipment.id) equipmentCount FROM
	ResearcherCabinet
	JOIN Cabinet ON ResearcherCabinet.cabinetId = Cabinet.id
	JOIN Equipment ON Cabinet.id = Equipment.cabinetId
	GROUP BY ResearcherCabinet.researcherId
) EquipmentUsageUnit
JOIN Researcher ON EquipmentUsageUnit.researcherId = Researcher.id
LEFT JOIN Research ON Researcher.researchId = Research.id
GROUP BY Research.id
HAVING SUM(EquipmentUsageUnit.equipmentCount) > 2

-- 4. Суммарный вес материалов для исследований, отсорченный (ну по весу)
SELECT Research.name, SUM(RawMaterial.materialWeight) totalMaterialWeight FROM
Researcher
JOIN RawMaterial ON Researcher.id = RawMaterial.researcherId
JOIN Research ON Researcher.id = Research.id
GROUP BY Research.name
ORDER BY SUM(RawMaterial.materialWeight) DESC

-- 5. Список ученых с какой-то степенью в важных есследованиях
SELECT Researcher.firstName, Researcher.lastName FROM
Researcher
JOIN Research ON Researcher.researchId = Research.id
WHERE Research.name IN (	
	SELECT Research.name FROM
	Research
	JOIN ResearchScope ON 
		Research.scopeId = ResearchScope.id AND
		ResearchScope.scopePriority = 1
)

-- 6. Кабинеты, задействованные в исследованиях
SELECT DISTINCT Cabinet.id, Research.name FROM
Cabinet, ResearcherCabinet, Researcher, Research
WHERE 
	Cabinet.id = ResearcherCabinet.cabinetId AND 
	ResearcherCabinet.researcherId = Researcher.id AND 
	Researcher.researchId = Research.id
ORDER BY Research.name, Cabinet.id

-- 7. Различные виды оборудования в каждом кабинете, вместе с кабинетом
SELECT DISTINCT Cabinet.id, EquipmentType.name FROM
Cabinet
JOIN Equipment ON Cabinet.id = Equipment.cabinetId
JOIN EquipmentType ON Equipment.equipmentTypeId = EquipmentType.id
WHERE EquipmentType.name LIKE '%ge'
ORDER BY Cabinet.id, EquipmentType.name

-- 8. Лучшие спонсоры
SELECT Sponsor.name sponsorName, SUM(Sponsorship.price) totalSponsored FROM
Sponsor
JOIN Sponsorship ON Sponsor.id = Sponsorship.sponsorId
JOIN Research ON Sponsorship.researchId = Research.id
GROUP BY Sponsor.name
HAVING SUM(Sponsorship.price) >= 50
ORDER BY SUM(Sponsorship.price) DESC

-- 9. Единицы работы ученых в кабинетах с данным видом оборудования
SELECT researcherId, resCabDate FROM
ResearcherCabinet WHERE
EXISTS (
	SELECT * FROM Cabinet 
	JOIN Equipment ON Cabinet.id = Equipment.cabinetId
	JOIN EquipmentType ON Equipment.equipmentTypeId = EquipmentType.id
	WHERE 
		ResearcherCabinet.cabinetId = Cabinet.id AND 
		EquipmentType.name = 'Ultracentrifuge'
)

-- 10. Единицы работы ученых с данным видом материала, упорядоченные по материалу
SELECT ResearcherCabinet.* FROM
ResearcherCabinet
JOIN Researcher ON ResearcherCabinet.researcherId = Researcher.id
JOIN RawMaterial ON Researcher.id = RawMaterial.researcherId
WHERE RawMaterial.name = 'Borage Seed Essential Oil'
ORDER BY RawMaterial.name

-- 11. Для каждой области материал, которого максимальное количество для данной области, сорчено по скоупу
; WITH ScopeTotalMaterial AS (
	SELECT ResearchScope.name scopeName, RawMaterial.name materialName, SUM(RawMaterial.materialWeight) totalMaterialCount FROM
	ResearchScope
	JOIN Research ON ResearchScope.id = Research.scopeId
	JOIN Researcher ON Research.id = Researcher.researchId
	JOIN RawMaterial ON Researcher.id = RawMaterial.researcherId
	GROUP BY ResearchScope.name, RawMaterial.name
)
SELECT ScopeTotalMaterial.scopeName, ScopeTotalMaterial.materialName, ScopeTotalMaterial.totalMaterialCount FROM
ScopeTotalMaterial
JOIN (
	SELECT MAX(_ScopeTotalMaterial.totalMaterialCount) totalMaterialCount
	FROM ScopeTotalMaterial _ScopeTotalMaterial
	GROUP BY _ScopeTotalMaterial.scopeName
) MaxTotalCount ON MaxTotalCount.totalMaterialCount = ScopeTotalMaterial.totalMaterialCount

-- 12. Материалы, испольюзующиеся в кабинетах с различными видами оборудования
SELECT EquipmentType.name, RawMaterial.name FROM 
Equipment
JOIN EquipmentType ON Equipment.equipmentTypeId = EquipmentType.id
JOIN Cabinet ON Equipment.cabinetId = Cabinet.id
JOIN ResearcherCabinet ON Cabinet.id = ResearcherCabinet.cabinetId
JOIN Researcher ON ResearcherCabinet.researcherId = Researcher.id
LEFT JOIN RawMaterial ON Researcher.id = RawMaterial.researcherId
WHERE RawMaterial.name IS NOT NULL
GROUP BY EquipmentType.name, RawMaterial.name

GO

-- [End] Запросы
-- Модификаторы

-- 1. Удалить спонсора и все его единицы спонсорства
DELETE FROM Sponsorship
WHERE Sponsorship.sponsorId IN (
	SELECT Sponsor.id FROM Sponsor WHERE Sponsor.name = '3M'
)
DELETE FROM Sponsor WHERE Sponsor.name = '3M'

-- 2. Обновить вместимость кабинетов для всех кабинетов, где больше находится данного уровня оборудования
UPDATE Cabinet
SET Cabinet.personCapacity = 
	CASE 
		WHEN 0 < Cabinet.personCapacity - 5 THEN Cabinet.personCapacity - 5
		ELSE 0 
	END
FROM (
	SELECT Cabinet.id, COUNT(Equipment.id) equipmentCount FROM
	Cabinet
	JOIN Equipment ON Cabinet.id = Equipment.cabinetId
	GROUP BY Cabinet.id
) EquipmentPerCabinet
WHERE
	Cabinet.id = EquipmentPerCabinet.id AND 
	EquipmentPerCabinet.equipmentCount > 2

-- 3. Уменьшить приоритет области в ислледованиях которой в среднем работают не более 2 человек
UPDATE ResearchScope
SET ResearchScope.scopePriority = ResearchScope.scopePriority + 1
FROM (
	SELECT ResearchPeopleCount.scopeId, AVG(ResearchPeopleCount.researchersCount) avgResearchers FROM (
		SELECT Research.scopeId, COUNT(Researcher.id) researchersCount FROM
		Research
		JOIN Researcher ON Research.id = Researcher.researchId
		GROUP BY Research.id, Research.scopeId
	) ResearchPeopleCount
	JOIN ResearchScope ON ResearchPeopleCount.scopeId = ResearchScope.id
	GROUP BY ResearchPeopleCount.scopeId
) ScopeAvgResearchers
WHERE 
	ResearchScope.id = ScopeAvgResearchers.scopeId AND
	ScopeAvgResearchers.avgResearchers <= 2

-- 4. Лишить материалов всех ученых, не задействованных в исследованиях
DELETE RM
FROM RawMaterial RM
JOIN Researcher ON RM.researcherId = Researcher.id
WHERE Researcher.researchId IS NULL

-- 5. Добавление записи через max + 1
SET IDENTITY_INSERT Research ON;
INSERT INTO Research(id, name, scopeId) VALUES ((SELECT MAX(Research.id) FROM Research) + 1, 'New Research', NULL)
SET IDENTITY_INSERT Research OFF;

GO

-- [End] Модификации
-- Триггеры

-- Нельзя добавлять материалы без владельца
CREATE TRIGGER RemoveHomelessMaterials
ON RawMaterial
AFTER INSERT AS
BEGIN
	DECLARE @resId INT
	SELECT @resId = researcherId FROM inserted
	IF (@resId IS NULL) ROLLBACK
END

GO

INSERT INTO RawMaterial(name, materialWeight, researcherId) VALUES ('New Material', 1001, NULL)

GO

-- Присвоить степень ученому у которого >=5 единиц работы
CREATE TRIGGER GiveDegree
ON ResearcherCabinet
AFTER INSERT, UPDATE AS
BEGIN
	DECLARE @researcherId INT
	SELECT @researcherId = researcherId FROM inserted
	DECLARE @researchCount INT
	SELECT @researchCount = COUNT(ResearcherCabinet.id) FROM
		ResearcherCabinet
		WHERE ResearcherCabinet.researcherId = @researcherId
	IF (@researchCount >= 5)
		UPDATE Researcher
		SET Researcher.degree = 1
		WHERE Researcher.id = @researcherId
END

GO

INSERT INTO ResearcherCabinet(researcherId, cabinetId, resCabDate) VALUES (5, 8, '2005-09-01');
INSERT INTO ResearcherCabinet(researcherId, cabinetId, resCabDate) VALUES (5, 8, '2005-10-14');
INSERT INTO ResearcherCabinet(researcherId, cabinetId, resCabDate) VALUES (5, 8, '2005-09-21');
-- На добавлении 4-го присвоится степень
INSERT INTO ResearcherCabinet(researcherId, cabinetId, resCabDate) VALUES (5, 8, '2005-09-20');

GO

-- [End] Триггеры
-- Индексы

CREATE UNIQUE INDEX UniqueMaterialOwnerIndex
ON RawMaterial (name, researcherId)

INSERT INTO RawMaterial(name, materialWeight, researcherId) VALUES ('_Name', 111, 2)
INSERT INTO RawMaterial(name, materialWeight, researcherId) VALUES ('_Name', 111, 2)

CREATE INDEX Researcher_ResearchId_Index
ON Researcher (researchId)

CREATE INDEX Equipment_CabinetId_Index
ON Equipment (cabinetId)

-- Не настолько важно, просто показывает производительность индекса по нескольким полям
CREATE INDEX ResearchScope_PN_Index
ON ResearchScope (scopePriority, name)

GO

-- [End] Триггеры
-- Процедура

CREATE PROCEDURE punishNotoriousResearchers(
	@day DATE, @researchName NVARCHAR(256)) 
AS BEGIN
	BEGIN TRANSACTION

	DECLARE @notoriousResearcher TABLE (
		id INT UNIQUE NOT NULL
	)

	INSERT INTO @notoriousResearcher
	SELECT ResearcherEquipmentCount.researcherId FROM (
		SELECT RCForGivenResearch.researcherId, COUNT(Equipment.id) equipmentCount FROM (
			SELECT ResearcherCabinet.* FROM
			Research 
			JOIN Researcher ON 
				Research.id = Researcher.researchId AND 
				Research.name = @researchName
			JOIN ResearcherCabinet ON Researcher.id = ResearcherCabinet.researcherId
		) RCForGivenResearch
		JOIN Cabinet ON 
			RCForGivenResearch.resCabDate = @day AND
			RCForGivenResearch.cabinetId = Cabinet.id
		JOIN Equipment ON Cabinet.id = Equipment.cabinetId
		GROUP BY RCForGivenResearch.researcherId
	) ResearcherEquipmentCount
	WHERE ResearcherEquipmentCount.equipmentCount > 2

	UPDATE RM
	SET RM.researcherId = NULL
	FROM RawMaterial RM
	JOIN Researcher ON RM.researcherId = Researcher.id
	JOIN @notoriousResearcher notRes ON Researcher.id = notRes.id

	IF @@ERROR != 0 ROLLBACK TRANSACTION
	ELSE COMMIT TRANSACTION
END

GO

EXEC punishNotoriousResearchers '2005-09-02', 'Regulators of Mouse and Human Beta Cell Proliferation'

GO