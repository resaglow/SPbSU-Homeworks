--Примеры с простыми запросами:

-- 1.	Выбрать всю информацию обо всех а/м.
SELECT * FROM [Car]

-- 2.	Выбрать легковые а/м.
SELECT * FROM [Car]
WHERE [Type] <> N'Грузовик' and [Type] <> N'Автобус'

-- 3.	Выбрать номера а/м, в которых число мест >= 5 и упорядочить их по количеству мест.
SELECT [Number] FROM Car 
WHERE FreePlacesCount > 5
ORDER BY FreePlacesCount

-- 4.	Выбрать номера и фамилии водителей всех белых автомобилей  BMV, у которых в номере встречается цифра 2. Список упорядочить по фамилии водителя.
SELECT [Number], [Driver] FROM [Car]
WHERE CHARINDEX('2', [Number]) > 0
ORDER BY [Driver]

-- 5.	Выдать номера автомобилей, время заказа и фамилии водителей, принимавших заказы с 12:00 до 19:00 1 сентября 2005 года.
SELECT o.[CarNumber], o.[Time], c.[Driver] FROM [Order] o, [Car] c
WHERE o.[CarNumber] = c.[Number] AND o.[Date] = '2005-09-01' AND o.[Time] BETWEEN '12:00:00' AND '19:00:00'

--Примеры со сложными запросами:

-- 1.	Посчитать общую сумму оплаты по всем выполненным заказам
SELECT SUM([Cost]) FROM [Order]

-- 2.	Получить список водителей, отсортированный по количеству выполненных заказов 
SELECT c.[Driver] FROM [Car] c, (
	SELECT [CarNumber], COUNT([CarNumber]) AS CarOrderCount FROM [Order]
	GROUP BY [CarNumber]
) t WHERE c.[Number] = t.[CarNumber]
ORDER BY CarOrderCount

-- 3.	Выбрать постоянных клиентов (не менее 5 выполненных заказов)
SELECT cl.[Name] FROM [Client] cl, (
	SELECT [ClientId], COUNT([ClientId]) AS ClientOrderCount FROM [Order]
	GROUP BY [ClientId]
) t WHERE cl.[Id] = t.[ClientId] AND t.ClientOrderCount >= 5

--Примеры на редактирование:

-- 1.	Удалить клиента Сидорова.
DELETE FROM [Client]
WHERE [Name] = N'Сидоров'
 
-- 2.	Удалить клиента Сидорова и все его заказы.
DELETE FROM [Order]
WHERE [ClientId] IN (
	SELECT [Id] FROM [Client] WHERE [Name] = N'Сидоров'
)

DELETE FROM [Client]
WHERE [Name] = N'Сидоров'

-- 3.	Заменить номер автомобиля ‘C404HM78’ на ‘C405HM78’.
UPDATE [Car]
SET [Number] = N'С405HM78'
WHERE [Number] = N'C404HM78'