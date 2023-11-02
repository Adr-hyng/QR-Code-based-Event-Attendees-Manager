CREATE DATABASE qeamDB;
USE qeamDB;

-- Initialize table content
CREATE TABLE Attendees (
	id INT AUTO_INCREMENT,
    uid  VARCHAR(25) NOT NULL,
    fn VARCHAR (24) NOT NULL,
    mi VARCHAR (2),
    ln VARCHAR (16) NOT NULL,
    membership BIT NOT NULL,
    position BIT NOT NULL,
    institution VARCHAR(64),
    pn VARCHAR (10),
    
    amd1 DATETIME,
    lunchd1 DATETIME,
    pmd1 DATETIME,
    checkind1 DATETIME,
    checkoutd1 DATETIME,
    
    amd2 DATETIME,
    lunchd2 DATETIME,
    pmd2 DATETIME,
    checkind2 DATETIME,
    checkoutd2 DATETIME,
    
    amd3 DATETIME,
    lunchd3 DATETIME,
    pmd3 DATETIME,
    checkind3 DATETIME,
    checkoutd3 DATETIME,
    
    PRIMARY KEY (id)
);

-- Update value based on condition
UPDATE Attendees
SET amd1 = CURRENT_TIMESTAMP
WHERE id = 2;

-- Delete table content created
DROP TABLE Attendees;

-- Put Content to table
-- First Batch
INSERT INTO Attendees (uid, fn, mi, ln, membership, position, institution, pn) VALUES 
('5kdIBZbdnNdF7amvWX1sPk', 'Jelinda', 'M', 'Gulfre', 1, 1, 'ESSU-Salcedo', '9052102858'),
('HiPe6Crw1zil3mgXyEIvzn', 'Edmun Dennis', 'S', 'Antivo', 1, 1, 'NORTHWEST SAMAR STATE UNIVERSITY', '9060487284'),
('LIGMxh6maEGaCDuoWDCPHF', 'Russel', 'T', 'Manadong', 0, 0, 'ESSU- Salcedo Campus', '9665748387'),
('LyMy3dgWUPsloyPBTKP9NA', 'Ranzel', 'O', 'Macapanas', 0, 0, 'ESSU-Salcedo Campus', '9207272222'),
('2ul3NKWkXmXbSWX8W0REdL', 'Fernando', 'A', 'Languido', 0, 0, 'ESSU-Salcedo Campus', '9855825849'),
('kFXL7kJdjSvUbEpHAAFACG', 'Dan ', '', 'Jacinth', 0, 0, 'Eastern Samar State University - Salcedo Campus', '9565939534'),
('ap06BMRgv0bsOuVCjX6s5Z', 'Julie Ann','S','Ogana' ,0 ,0 ,'Eastern Samar State University- Salcedo Campus' ,'9757571514'),
('bOLzHK9pIGWIABmdRvDwDB' ,'Jade Kyle' ,'L' ,'Sabetan' ,0 ,0 ,'ESSU-Salcedo' ,'9815301876'),
('19EmhSRAvi4b4FEZNUrrui' ,'Pio Samuel' ,'A' ,'Macapugas' ,0 ,0 ,'Eastern Samar State University - Salcedo' ,'9603941662'),
('ZeD3RhgXY2KhQt9Krj1vpj' ,'Danny Jr.' ,'H' ,'Cartagena' ,0 ,0 ,'ESSU-Salcedo Campus' ,'9631159277'),
('82ghZwW9XGAFkkK5xtHR40' ,'John Laurence' ,'B' ,'Poncion' ,0 ,0 ,'ESSU Salcedo' ,'9351354199'),
('XL3rb4PLDTb4nBQtL7Ynd7' ,'Dahlia' ,'D' ,'Fernandez' ,1 ,0 ,'Biliran Province State University' ,'9177055800'),
('iRUDn9hGyl1OWo1BZmtzV9' ,'Hershie Niña','A','Ebajo' ,0 ,0,'Biliran Province State University' ,'9667550280'),
('hXPPUnmsFtMNGMMjYIv5jQ','Joralyn','A','Espina',1,0,'Biliran Province State University (BiPSU)','9776427488'),
('YBigsvCTddjjmXGVv1i6v5','Michael Angelito','C','Apita',1,0,'ESSU Salcedo Campus','9275417479'),
('IpwGKOw0UFuRkc9RkyRDyz','Carl Joshua','F','Jadulco',0,0,'Biliran Province State University (BiPSU)','9952576248'),
('LL2CooDTvaOCrpCTlqdayt','Mark Alfred','','Cadeon',0,0,'Biliran Province State University','9959519157'),
('yIPKeWP4ApO60o6XQ168Xp','Mark Lester','M','Mondelo',0,0,'Biliran Province State University','9639642632'),
('XdUHkr1o40u7aFqzuaw94e','Dahlia','D','Fernandez',1,1,'Biliran Province State University','9177055800'),
('5X6DLcqMw89bXCPcXumT9S','Kingsley James','J','Digman',0,0,'Biliran Province State University','9052548763');

-- Second Batch
INSERT INTO Attendees (uid, fn, mi, ln, membership, position, institution, pn) VALUES 
('dEfBelEmxXTbhGtCqrHUEV', 'Jhanos', 'G', 'Abad', 0, 0, 'Biliran Province State University', '9150516074'),
('DIPUU0Uyalu26XMgUfkGUt', 'Marie Grace', 'M', 'Lentejas', 1, 1, 'CITY TREASURER''S OFFICE CALBAYOG', '9261700865'),
('Xgcy30NJRuZOuOne6udwOw', 'Estelito Jr.', 'R', 'Medallada', 1, 1, 'Biliran Province State University', '9058723042'),
('3OiLin9fDMeZJ4UciPikgN', 'Karyll Ann', '', 'Ramirez', 0, 0, 'Biliran Province State University', '9505943009'),
('LtABuAcxO7lRT2LWO4ltH9', 'Johnuel', 'A', 'Gelizon', 0, 0, 'Biliran Province State University', '9366494874'),
('xBAIxV9R4u3R5rzEHmzw56', 'Renalyn', 'A', 'De Paz', 0, 0, 'Biliran Province State University', '9531346790'),
('NzmW19gA3ER4GNxwx8JOKh', 'Karyll Ann','', 'Ramirez' ,0 ,0 ,'SOE-ICpEP BILIRAN PROVINCE STATE UNIVERSITY' ,'9505943009'),
('hqZFJIcRbPE3kdt6N2XQM6' ,'Renalyn' ,'A' ,'De Paz' ,0 ,0 ,'SOE-ICpEP Biliran Province State University' ,'9531346790'),
('wqlDGBvNb0kSpYMzASdLOK' ,'Gerald Angelo','A','Rodrigo' ,0 ,0 ,'Biliran Province State University' ,'9666516919'),
('Z1oeYkZuTMP5uaLemAxyfN' ,'Clark' ,'R' ,'Llenado' ,0 ,0 ,'SOE ICpEP - BiPSU' ,'9704514170'),
('7U0s9FT7whI5uwSVhI9AL1' ,'Prince Charles','M','Perez' ,0 ,0 ,'SOE-ICpEP BILIRAN PROVINCE STATE UNIVERSITY' ,'9285692522'),
('M2NOmidUswDzsoLY4x83Oz' ,'Kieth Japhet','', 'Alua' ,0 ,0 ,'Biliran Province State University' ,'9855863438'),
('xQcd3wT8s2TFkmylXt6cL8' ,'Johnuel' ,'A' ,'Gelizon' ,0 ,0 ,'Biliran Province State University' ,'9366494874'),
('JO3AoiVZt3bTDgd9U515hu','Glen Nilo','P','Minlay',0,0,'Biliran Province State University','9505947725'),
('T3H5hG7BEqhAI9IAiONsS7','Krysthille Anne','O','Tambis',0,0,'Biliran Province State University','9317291867'),
('1IBaBUJH6Q8KL8W4QYr50j','John Anthony','S','Serbo',0,0,'Biliran Province State University','9856216298'),
('iMMPBM3v3QkIdJfb02QKHZ','Dean Dominique','G','Cabillan',0,0,'Biliran Province State University','9454366291'),
('ATKXEQSUTNs7VuzW7SNK2u','Mark Lester','M','Mondelo',0,0,'Biliran Province State University','9639642632'),
('Bz3Amg7WaCdYSsuZmLYzod','Jeffrey','T','Espina',1,0,'N/A','9183611094'),
('jp7j1RRfodfu7v3rSx9hBq','Vicente Jr.','G','Andrade',1,0,'Christ the King College of Calbayog City','9231315128'),
('mBG3B3GhEkVMiYV1Ly5ZNU','Maria Rosario','V','Lee',1,0,'Christ the King College of Calbayog City','9231315128'),
('jShYe88AGRaiPZ7xv9NUW3','Joseph','G','Acebo',1,0,'Eastern Samar State University Salcedo Campus','9387419785');

