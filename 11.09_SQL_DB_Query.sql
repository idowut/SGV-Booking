CREATE TABLE Users (
	userID INT IDENTITY(10000,1),
	userType INT NOT NULL,
	firstName VARCHAR(100) NOT NULL,
    lastName VARCHAR(100) NOT NULL,
	email VARCHAR(200) NOT NULL,
	bookingsCount INT NOT NULL,
	phoneNumber VARCHAR(15) NOT NULL,
	password VARCHAR(MAX) NOT NULL,
	CONSTRAINT pk_userID PRIMARY KEY (userID)
);

CREATE TABLE Addresses(
    addressID INT IDENTITY,
    addressLine VARCHAR(100) NOT NULL,
    suburb VARCHAR(50) NOT NULL,
    postcode VARCHAR(10)NOT NULL,
    region VARCHAR(50) NOT NULL,
    country VARCHAR(50) NOT NULL,
    CONSTRAINT pk_addressID PRIMARY KEY (addressID)
);

CREATE TABLE Restaurants(
    restaurantID INT IDENTITY,
    restaurantName VARCHAR(100) NOT NULL,
    restaurantAddressID INT,
    restaurantPhoneNumber VARCHAR(15), 
    CONSTRAINT pk_restaurantID PRIMARY KEY (restaurantID),
    CONSTRAINT fk_addressID FOREIGN KEY (restaurantAddressID) REFERENCES Addresses (addressID)
);

CREATE TABLE Bookings(
    bookingID INT IDENTITY(10000,1),
    restaurantID INT NOT NULL,
    bookingTime datetime NOT NULL,
    customerID INT NOT NULL,
	bookingNotes VARCHAR(500) NULL,
	banquetOption INT NULL,
	numGuest INT NOT NULL,
    CONSTRAINT pk_bookingID PRIMARY KEY (bookingID),
    CONSTRAINT fk_restaurantID FOREIGN KEY (restaurantID) REFERENCES Restaurants (restaurantID),
    CONSTRAINT fk_customerID FOREIGN KEY (customerID) REFERENCES Users (userID)
);

CREATE TABLE Banquets(
	banquetID INT IDENTITY,
	restaurantID INT NOT NULL,
	banquetName VARCHAR(100) NOT NULL,
	banquetMinPeople INT NOT NULL,
	banquetPrice INT NOT NULL,
	CONSTRAINT pk_banquetID PRIMARY KEY (banquetID),
	CONSTRAINT fk_restaurantBanquetID FOREIGN KEY (restaurantID) REFERENCES Restaurants (restaurantID),
);

CREATE TABLE BanquetItems(
	banquetItemID INT IDENTITY,
	banquetID INT NOT NULL,
	banquetItem VARCHAR(100) NOT NULL,
	banquetDesc VARCHAR(200),
	CONSTRAINT pk_banquetItemID PRIMARY KEY (banquetItemID),
	CONSTRAINT fk_banquetID FOREIGN KEY (banquetID) REFERENCES Banquets (banquetID),
);


INSERT INTO Addresses (addressLine, suburb, postcode, region, country) VALUES('25 Gouger Street', 'Adelaide', '5000', 'South Australia', 'Australia');
INSERT INTO Addresses (addressLine, suburb, postcode, region, country) VALUES('40/120 North Terrace', 'Adelaide', '5000', 'South Australia', 'Australia');
INSERT INTO Addresses (addressLine, suburb, postcode, region, country) VALUES('40 Fullarton Road', 'Norwood', '5067', 'South Australia', 'Australia');

INSERT INTO Restaurants (restaurantName, restaurantAddressID) VALUES('Bamboo Leaf', (SELECT addressID FROM Addresses WHERE addressID = 1));
INSERT INTO Restaurants (restaurantName, restaurantAddressID) VALUES('La Oeste De La Mar', (SELECT addressID FROM Addresses WHERE addressID = 2));
INSERT INTO Restaurants (restaurantName, restaurantAddressID) VALUES('Mexikana', (SELECT addressID FROM Addresses WHERE addressID = 3));

INSERT INTO Users (userType, firstName, lastName, email, phoneNumber, password, bookingsCount) VALUES(0, 'Anthony', 'Chung', 'jason.tinle@hotmail.com', '0415212334', 'password', 2);
INSERT INTO Users (userType, firstName, lastName, email, phoneNumber, password, bookingsCount) VALUES(1, 'Peter', 'Bui', 'Billy.Siam@yahoo.com', '0432 471 523', '12345', 2);
INSERT INTO Users (userType, firstName, lastName, email, phoneNumber, password, bookingsCount) VALUES(2, 'Jason', 'Le', 'Brigade.Bui@yahoo.com', '+61 432 471 523', 'tintinistintin12', 2);

INSERT INTO Banquets (restaurantID, banquetName, banquetMinPeople, banquetPrice) VALUES(1, 'Share Banquet', 4, 55);
INSERT INTO Banquets (restaurantID, banquetName, banquetMinPeople, banquetPrice) VALUES(1, 'Feast Banquet', 4, 70);
INSERT INTO Banquets (restaurantID, banquetName, banquetMinPeople, banquetPrice) VALUES(3, 'Share Banquet', 4, 35);

INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (1, 'Satay Chicken');
INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (1, 'Steam Prawn Dumplings');
INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (1, 'S+P Squid');
INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (1, 'Roast Pork');
INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (1, 'Sticky Pork');
INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (1, 'Asian Green');

INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (2, 'Crispy beef');
INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (2, 'Satay Chicken');
INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (2, 'S+P Eggplant');
INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (2, 'Crying Tiger Salad');
INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (2, 'Steam Prawn Dumplings');
INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (2, 'Ribs');
INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (2, 'Baramundi Curry');

INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (3, 'Papas Fritas');
INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (3, 'Korean Fried Chicken');
INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (3, 'Smokey chicken');
INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (3, 'Taquitos dorados');
INSERT INTO BanquetItems (banquetID, banquetItem) VALUES (3, 'Nachos');

INSERT INTO Bookings (restaurantID, bookingTime, customerID, bookingNotes, numGuest) VALUES(2, '2023-09-30 19:00:00', (SELECT userID from Users WHERE userID = 10000), 'This is a booking for La Oeste De La Mar at 7:00pm with 3 guests and no banquet', 3);
INSERT INTO Bookings (restaurantID, bookingTime, customerID, bookingNotes, banquetOption, numGuest) VALUES(1, '2023-09-29 18:00:00', (SELECT userID from Users WHERE userID = 10001), 'This is a booking for Bamboo Leaf at 6:00pm with 5 guests and Share Banquet', 1, 5);
INSERT INTO Bookings (restaurantID, bookingTime, customerID, bookingNotes, banquetOption, numGuest) VALUES(3, '2023-09-28 20:00:00', (SELECT userID from Users WHERE userID = 10002), 'This is a booking for Mexikana at 8:00pm with 8 guests and Share Banquet', 3, 8);
INSERT INTO Bookings (restaurantID, bookingTime, customerID, bookingNotes, numGuest) VALUES(2, '2023-09-27 18:30:00', (SELECT userID from Users WHERE userID = 10002), 'This is a booking for La Oeste De La Mar at 6:30pm with 2 guests and no banquet', 2);
INSERT INTO Bookings (restaurantID, bookingTime, customerID, bookingNotes, banquetOption, numGuest) VALUES(1, '2023-09-26 20:30:00', (SELECT userID from Users WHERE userID = 10001), 'This is a booking for Bamboo Leaf at 8:30pm with 4 guests and Feast Banquet', 2, 4);
INSERT INTO Bookings (restaurantID, bookingTime, customerID, bookingNotes, numGuest) VALUES(3, '2023-09-28 19:45:00', (SELECT userID from Users WHERE userID = 10002), 'This is a booking for Mexikana at 7:45pm with 7 guests and no banquet',7);


SELECT * FROM Users;
SELECT * FROM Restaurants;
SELECT * FROM Bookings;
SELECT * FROM Addresses;
SELECT * FROM BanquetItems;
SELECT * FROM Banquets;