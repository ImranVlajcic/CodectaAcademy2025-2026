CREATE TABLE Account (
    userID SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(150) NOT NULL UNIQUE,
    accPassword VARCHAR(255) NOT NULL,
    realName VARCHAR(100),
    realSurname VARCHAR(100),
    phoneNumber VARCHAR(30)
);

CREATE TABLE Currency(
    currencyID  SERIAL PRIMARY KEY,
    currencyCode VARCHAR(3) NOT NULL UNIQUE,
    currencyName VARCHAR(50) NOT NULL UNIQUE,
    rateToEuro DECIMAL(20,10) NOT NULL
);

CREATE TABLE Wallet (
    walletID SERIAL PRIMARY KEY,
    userID INT NOT NULL,
    currencyID INT NOT NULL,
    balance DECIMAL(15, 4) DEFAULT 0.00,
    purpose VARCHAR(100),
    FOREIGN KEY (userID) REFERENCES Account(userID) ON DELETE CASCADE,
    FOREIGN KEY (currencyID) REFERENCES Currency(currencyID) ON DELETE RESTRICT
);

CREATE TABLE Category (
    categoryID SERIAL PRIMARY KEY,
    categoryName VARCHAR(100) NOT NULL,
    reason VARCHAR(20) NOT NULL CHECK (reason IN ('prihod', 'rashod'))
);

CREATE TABLE Transactions (
    transactionID SERIAL PRIMARY KEY,
    walletID INT NOT NULL,
    categoryID INT NOT NULL,
    currencyID INT NOT NULL,
    transactionTime TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    transactionDate DATE NOT NULL,
    transactionType VARCHAR(30) CHECK (transactionType IN ('gotovina','kartica')),
    amount DECIMAL(15, 4) NOT NULL,
    description VARCHAR(255),
    FOREIGN KEY (walletID) REFERENCES Wallet(walletID) ON DELETE CASCADE,
    FOREIGN KEY (categoryID) REFERENCES Category(categoryID) ON DELETE RESTRICT,
    FOREIGN KEY (currencyID) REFERENCES Currency(currencyID) ON DELETE RESTRICT
);

CREATE TABLE StandardExpense (
    expenseID SERIAL PRIMARY KEY,
    walletID INT NOT NULL,
    reason VARCHAR(30) NOT NULL,
    description VARCHAR(255),
    amount DECIMAL(15, 4) NOT NULL,
    frequency VARCHAR(30) CHECK (frequency IN ('mjesecno','sedmicno','godisnje','dnevno')),
    nextDate DATE NOT NULL,
    FOREIGN KEY (walletID) REFERENCES Wallet(walletID) ON DELETE CASCADE
);

INSERT INTO Account (username, email, accPassword, realName, realSurname, phoneNumber) VALUES
('mujo_mujic', 'mujo.mujic@gmail.com', 'hash_lozinka_123', 'Mujo', 'Mujic', '+38761234567'),
('imran_vlajcic', 'imran.vlajcic@gmail.com', 'hash_lozinka_456', 'Imran', 'Vlajcic', '+38762345678');

INSERT INTO Currency(currencyCode, currencyName, rateToEuro) VALUES
('EUR','Euro',1.0),
('USD','United States Dollar', 1.63);

INSERT INTO Wallet (userID, currencyID, balance, purpose) VALUES
(1, 1, 2500.00, 'Glavni racun'),
(1, 1, 800.00, 'Stednja'),
(2, 2, 2000.00, 'Glavni racun');

INSERT INTO Category (categoryName, reason) VALUES
('Plata', 'prihod'),
('Freelance', 'prihod'),
('Poklon', 'prihod'),
('Namirnice', 'rashod'),
('Stanarina', 'rashod'),
('Rezije', 'rashod'),
('Servis', 'rashod'),
('Putovanje', 'rashod'),
('Odjeca', 'rashod'),
('Zdravstvo', 'rashod'),
('Zabava','rashod');

INSERT INTO Transactions (walletID, categoryID, currencyID, transactionDate, transactionType, amount, description) VALUES
(1, 1, 1,'2024-11-01', 'kartica', 1800.00, 'Mjesecna plata'),
(1, 4, 1,'2024-11-03', 'kartica',  -120.00, 'Kupovina u Bingu'),
(1, 5, 1,'2024-11-01', 'kartica',  -600.00, 'Stanarina za novembar'),
(2, 8, 1,'2024-11-08', 'gotovina',  -45.00, 'Mjesecno gorivo'),
(2, 9, 1,'2024-11-10', 'gotovina',   -60.00, 'Kino'),
(2, 1, 1,'2024-11-01', 'kartica',  2000.00, 'Mjesecna plata'),
(3, 2, 1,'2024-11-15', 'gotovina',   450.00, 'Izrada web sajta'),
(3, 4, 1,'2024-11-12', 'gotovina',   -95.00, 'Kupovina u Konzumu'),
(3, 3, 1,'2024-11-14', 'gotovina',   75.00, 'Poklon od prijatelja');

INSERT INTO StandardExpense (walletID, reason, description, amount, frequency, nextDate) VALUES
(1, 'Stanarina', 'Placanje stanarina', 600.00, 'mjesecno', '2025-11-14'),
(1, 'Internet', 'Internet paket', 35.00, 'mjesecno', '2025-11-14'),
(1, 'Namirnice', 'Prosjecna sedmicna potrosnja', 120.00, 'sedmicno', '2026-12-01'),
(3, 'Teretana', 'Mjesecna clanarina', 60.00, 'mjesecno', '2025-03-17'),
(2, 'Osiguranje', 'Osiguranje automobila', 80.00, 'godisnje', '2025-09-10'),
(1, 'Telefon', 'Mobilna pretplata', 25.00, 'mjesecno', '2025-06-27');