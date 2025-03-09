CREATE DATABASE Commerce

USE Commerce
GO
CREATE TABLE Rol (
    Rol_Id INT IDENTITY(1,1) PRIMARY KEY, 
    Rol_Description NVARCHAR(30) NOT NULL 
);

CREATE TABLE [User] (
    Use_Id INT IDENTITY(1,1) PRIMARY KEY, 
    Use_First_Name NVARCHAR(20) NOT NULL, 
    Use_Middle_Name NVARCHAR(20) NULL, 
    Use_First_Last_Name NVARCHAR(20) NOT NULL, 
    Use_Middle_Last_Name NVARCHAR(20) NULL, 
    Use_Email NVARCHAR(40) NOT NULL, 
    Use_Password NVARCHAR(40) NOT NULL, 
    Use_Rol INT NOT NULL, 
    
    CONSTRAINT FK_User_Rol FOREIGN KEY (Use_Rol) REFERENCES Rol(Rol_Id) 
);

CREATE TABLE Businessman_Status (
    Bst_Id CHAR(1) PRIMARY KEY, 
    Bst_Description NVARCHAR(10) NOT NULL 
);

CREATE TABLE Municipality (
    Mun_Id CHAR(8) PRIMARY KEY, 
    Mun_Nombre NVARCHAR(30) NOT NULL 
);

CREATE TABLE Businessman (
    Bus_Id INT IDENTITY(1,1) PRIMARY KEY,
    Bus_Name NVARCHAR(40) NOT NULL, 
    Bus_Phone_Number NVARCHAR(20) NULL,
    Bus_Email NVARCHAR(40) NULL, 
    Bus_Date_Registration DATE NOT NULL, 
    Bus_Date_Update DATE NULL, 
    Bus_Status CHAR(1) NOT NULL,
    Bus_Municipality CHAR(8) NOT NULL,
    Bus_User_Update NVARCHAR(30), 

    
    CONSTRAINT FK_Businessman_Status FOREIGN KEY (Bus_Status) REFERENCES Businessman_Status(Bst_Id),
    CONSTRAINT FK_Businessman_Municipality FOREIGN KEY (Bus_Municipality) REFERENCES Municipality(Mun_Id)
    
);


CREATE TABLE Establishment (
    Est_Id INT IDENTITY(1,1) PRIMARY KEY, 
    Est_Nombre NVARCHAR(30) NOT NULL, 
    Est_Money_Income DECIMAL(18,2) NOT NULL, 
    Est_Employees INT NOT NULL, 
    Est_Businessman INT NOT NULL, 
    Est_Date_Update DATE NULL, 
    Est_User_Update NVARCHAR(30), 

    
    CONSTRAINT FK_Establishment_Businessman FOREIGN KEY (Est_Businessman) REFERENCES Businessman(Bus_Id)
    
);


CREATE TRIGGER trg_Businessman_Update
ON Businessman
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE B
    SET 
        B.Bus_Date_Update = GETDATE(), 
        B.Bus_User_Update = SUSER_NAME()
    FROM Businessman B
    INNER JOIN inserted I ON B.Bus_Id = I.Bus_Id;
END;


CREATE TRIGGER trg_Establishment_Update
ON Establishment
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE E
    SET 
        E.Est_Date_Update = GETDATE(), 
        E.Est_User_Update = SUSER_NAME() 
    FROM Establishment E
    INNER JOIN inserted I ON E.Est_Id = I.Est_Id;
END;


INSERT INTO Rol VALUES ('Administrador'), ('Auxiliar de registro')
INSERT INTO Businessman_Status VALUES ('A','ACTIVO'), ('I','INACTIVO')

INSERT INTO Municipality VALUES ('CAL','CALI'),
('BOG','BOGOTA'),
('MED','MEDELLIN'),
('BUC','BUCARAMANGA'),
('BAR','BARRANQUILLA'),
('MAN','MANIZALES')

INSERT INTO [User] (Use_First_Name, Use_Middle_Name, Use_First_Last_Name, Use_Middle_Last_Name, Use_Email, Use_Password, Use_Rol)
VALUES 
('Carlos', 'Andrés', 'Gómez', 'López', 'carlos.gomez@example.com', 'Password123', 1), 
('María', NULL, 'Pérez', 'Rodríguez', 'maria.perez@example.com', 'SecurePass456', 2);


INSERT INTO Businessman (Bus_Name, Bus_Phone_Number, Bus_Email, Bus_Date_Registration, Bus_Status, Bus_Municipality)
VALUES 
('Tienda El Sol', '3011234567', 'contacto@tiendaelsol.com', GETDATE(), 'A', 'CAL'),
('Panadería Delicias', '3129876543', 'info@panaderiadelicias.com', GETDATE(), 'A', 'BOG'),
('Ferretería El Tornillo', NULL, 'ventas@tornillo.com', GETDATE(), 'I', 'MED'),
('Supermercado Ahorro', '3006543210', NULL, GETDATE(), 'A', 'MED'),
('Tecnología Express', '3224567890', 'soporte@tecnoexpress.com', GETDATE(), 'I', 'CAL');



INSERT INTO Establishment (Est_Nombre, Est_Money_Income, Est_Employees, Est_Businessman)
VALUES 
('Café Delicias', 1500000.50, 5, 1),
('Restaurante Sabores', 3200000.75, 10, 2),
('Ferretería El Constructor', 2100000.25, 7, 3),
('Boutique Elegancia', 1250000.00, 3, 4),
('Gimnasio FitLife', 4500000.80, 15, 5),
('Pizzería La Italiana', 2750000.60, 8, 1),
('Papelería Escolar', 950000.30, 4, 2),
('Librería Central', 1850000.90, 6, 3),
('Supermercado Económico', 5200000.45, 20, 4),
('Veterinaria Amigos', 2300000.70, 5, 5);



CREATE PROCEDURE sp_GetActiveBusinessmenReport
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        B.Bus_Name AS Nombre_Comerciante,
        M.Mun_Nombre AS Municipio,
        B.Bus_Phone_Number AS Telefono,
        B.Bus_Email AS Correo_Electronico,
        B.Bus_Date_Registration AS Fecha_Registro,
        BS.Bst_Description AS Estado,
        COUNT(E.Est_Id) AS Cantidad_Establecimientos,
        COALESCE(SUM(E.Est_Money_Income), 0) AS Total_Ingresos,
        COALESCE(SUM(E.Est_Employees), 0) AS Cantidad_Empleados
    FROM Businessman B
    INNER JOIN Municipality M ON B.Bus_Municipality = M.Mun_Id
    INNER JOIN Businessman_Status BS ON B.Bus_Status = BS.Bst_Id
    LEFT JOIN Establishment E ON B.Bus_Id = E.Est_Businessman
    WHERE B.Bus_Status = 'A'  
    GROUP BY 
        B.Bus_Id, B.Bus_Name, M.Mun_Nombre, B.Bus_Phone_Number, 
        B.Bus_Email, B.Bus_Date_Registration, BS.Bst_Description
    ORDER BY Cantidad_Establecimientos DESC;
END;

exec sp_GetActiveBusinessmenReport


