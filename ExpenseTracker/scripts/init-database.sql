CREATE TABLE Account (
    userID SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(150) NOT NULL UNIQUE,
    passwordHash VARCHAR(255) NOT NULL,
    realName VARCHAR(100),
    realSurname VARCHAR(100),
    phoneNumber VARCHAR(30),
    createdAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    lastLoginAt TIMESTAMP NULL,
    isActive BOOLEAN NOT NULL DEFAULT TRUE,
    refreshToken TEXT NULL,
    refreshTokenExpiryTime TIMESTAMP NULL
);

CREATE INDEX idx_account_email ON Account(email);
CREATE INDEX idx_account_username ON Account(username);
CREATE INDEX idx_account_refresh_token ON Account(refreshToken);
CREATE INDEX idx_account_isactive ON Account(isActive);

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
    reason VARCHAR(20) NOT NULL CHECK (reason IN ('Income', 'Expense'))
);

CREATE TABLE Transactions (
    transactionID SERIAL PRIMARY KEY,
    walletID INT NOT NULL,
    categoryID INT NOT NULL,
    currencyID INT NOT NULL,
    transactionTime TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    transactionDate DATE NOT NULL,
    transactionType VARCHAR(30) CHECK (transactionType IN ('Cash','Card')),
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
    frequency VARCHAR(30) CHECK (frequency IN ('Monthly','Weekly','Yearly','Daily')),
    nextDate DATE NOT NULL,
    FOREIGN KEY (walletID) REFERENCES Wallet(walletID) ON DELETE CASCADE
);

INSERT INTO Account (username, email, passwordHash, realName, realSurname, phoneNumber, createdAt, isActive) VALUES
('mujo_mujic', 'mujo.mujic@gmail.com', '$2a$12$NT61C5Z5wA2OqwsiuqNB/OacRPZKMnqGbomLy2rgyyCrxyAGUrbQa', 'Mujo', 'Mujic', '+38761234567', CURRENT_TIMESTAMP, TRUE),
('imran_vlajcic', 'imran.vlajcic@gmail.com', '$2a$12$NT61C5Z5wA2OqwsiuqNB/OacRPZKMnqGbomLy2rgyyCrxyAGUrbQa', 'Imran', 'Vlajcic', '+38762345678', CURRENT_TIMESTAMP, TRUE);

INSERT INTO Currency(currencyCode, currencyName, rateToEuro) VALUES
('EUR','Euro',1.0),
('USD','United States Dollar', 1.63);

INSERT INTO Wallet (userID, currencyID, balance, purpose) VALUES
(1, 1, 2500.00, 'Main wallet'),
(1, 1, 800.00, 'Savings wallet'),
(2, 2, 2000.00, 'Main wallet');

INSERT INTO Category (categoryName, reason) VALUES
('Salary', 'Income'),
('Freelance', 'Income'),
('Gift', 'Income'),
('Grocery', 'Expense'),
('Rent', 'Expense'),
('Bills', 'Expense'),
('Service', 'Expense'),
('Traveling', 'Expense'),
('Clothes', 'Expense'),
('Health', 'Expense'),
('Fun','Expense'),
('Reccuring','Expense');

INSERT INTO Transactions (walletID, categoryID, currencyID, transactionDate, transactionType, amount, description) VALUES
(1, 1, 1,'2024-11-01', 'Card', 1800.00, 'Mjesecna plata'),
(1, 4, 1,'2024-11-03', 'Card',  -120.00, 'Kupovina u Bingu'),
(1, 5, 1,'2024-11-01', 'Card',  -600.00, 'Stanarina za novembar'),
(2, 8, 1,'2024-11-08', 'Cash',  -45.00, 'Monthly gorivo'),
(2, 9, 1,'2024-11-10', 'Cash',   -60.00, 'Kino'),
(2, 1, 1,'2024-11-01', 'Card',  2000.00, 'Mjesecna plata'),
(3, 2, 1,'2024-11-15', 'Cash',   450.00, 'Izrada web sajta'),
(3, 4, 1,'2024-11-12', 'Cash',   -95.00, 'Kupovina u Konzumu'),
(3, 3, 1,'2024-11-14', 'Cash',   75.00, 'Poklon od prijatelja');

INSERT INTO StandardExpense (walletID, reason, description, amount, frequency, nextDate) VALUES
(1, 'Stanarina', 'Placanje stanarina', 600.00, 'Monthly', '2025-11-14'),
(1, 'Internet', 'Internet paket', 35.00, 'Monthly', '2025-11-14'),
(1, 'Namirnice', 'Prosjecna sedmicna potrosnja', 120.00, 'Weekly', '2026-12-01'),
(3, 'Teretana', 'Mjesecna clanarina', 60.00, 'Monthly', '2025-03-17'),
(2, 'Osiguranje', 'Osiguranje automobila', 80.00, 'Yearly', '2025-09-10'),
(1, 'Telefon', 'Mobilna pretplata', 25.00, 'Monthly', '2025-06-27');

--For stronger graph showcase

INSERT INTO Account (username, email, passwordHash, realName, realSurname, phoneNumber, createdAt, isActive)
VALUES 
('demo_user', 'demo.user@gmail.com', '$2a$12$NT61C5Z5wA2OqwsiuqNB/OacRPZKMnqGbomLy2rgyyCrxyAGUrbQa', 'Demo', 'User', '+38761111222', CURRENT_TIMESTAMP, TRUE);

INSERT INTO Wallet (userID, currencyID, balance, purpose) VALUES
(3, 1, 3000.00, 'Main Wallet'),
(3, 1, 1500.00, 'Savings'),
(3, 2, 500.00, 'USD Wallet');

INSERT INTO Transactions (walletID, categoryID, currencyID, transactionDate, transactionType, amount, description) VALUES

-- INCOME
(4, 1, 1, '2024-11-01', 'Card', 2500.00, 'Monthly salary'),
(4, 2, 1, '2024-11-10', 'Card', 600.00, 'Freelance project'),
(4, 3, 1, '2024-11-18', 'Cash', 150.00, 'Gift from friend'),

-- GROCERIES
(4, 4, 1, '2024-11-02', 'Card', -85.50, 'Bingo shopping'),
(4, 4, 1, '2024-11-06', 'Card', -60.20, 'Konzum groceries'),
(4, 4, 1, '2024-11-14', 'Cash', -45.00, 'Local market'),

-- RENT & BILLS
(4, 5, 1, '2024-11-01', 'Card', -700.00, 'Apartment rent'),
(4, 6, 1, '2024-11-05', 'Card', -120.00, 'Electricity & water'),
(4, 6, 1, '2024-11-20', 'Card', -60.00, 'Internet & phone'),

-- TRANSPORT & TRAVEL
(4, 8, 1, '2024-11-07', 'Cash', -50.00, 'Fuel'),
(4, 8, 1, '2024-11-15', 'Cash', -70.00, 'Fuel refill'),
(4, 8, 1, '2024-11-25', 'Card', -120.00, 'Weekend trip'),

-- CLOTHES
(4, 9, 1, '2024-11-09', 'Card', -150.00, 'New jacket'),
(4, 9, 1, '2024-11-22', 'Card', -90.00, 'Shoes'),

-- HEALTH
(4, 10, 1, '2024-11-11', 'Cash', -40.00, 'Pharmacy'),
(4, 10, 1, '2024-11-27', 'Card', -120.00, 'Doctor check'),

-- FUN
(4, 11, 1, '2024-11-12', 'Cash', -30.00, 'Cinema'),
(4, 11, 1, '2024-11-19', 'Card', -75.00, 'Dinner out'),
(4, 11, 1, '2024-11-28', 'Cash', -50.00, 'Night out'),

-- SAVINGS TRANSFER (simulate income to savings wallet)
(5, 1, 1, '2024-11-03', 'Card', 500.00, 'Transfer to savings'),

-- USD WALLET ACTIVITY
(6, 2, 2, '2024-11-13', 'Card', 300.00, 'Freelance USD'),
(6, 4, 2, '2024-11-16', 'Cash', -50.00, 'Groceries USD'),
(6, 11, 2, '2024-11-21', 'Card', -40.00, 'Entertainment USD');