use `soc`;

CREATE TABLE `UserPassword` (
  `Login` varchar(200) NOT NULL,
  `PasswordHash` binary(32) NOT NULL,
  `Salt` char(36) NOT NULL,
  PRIMARY KEY (`Login`));


CREATE TABLE `User` (
  `Id` binary(16) NOT NULL,
  `Login` varchar(200) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Surname` varchar(100) NOT NULL,
  `City` varchar(100) NOT NULL,
  `Gender` tinyint(1) NOT NULL,
  `BirthDate` date NOT NULL,
  `Interest` varchar(4000) NOT NULL,
  PRIMARY KEY (`Id`));

CREATE TABLE `FriendOffer` (
  `Id` BINARY(16) NOT NULL,
  `From` BINARY(16) NULL,
  `To` BINARY(16) NULL,
  PRIMARY KEY (`Id`));

CREATE TABLE `soc`.`FriendLink` (
`UserId` BINARY(16) NOT NULL,
`FriendId` BINARY(16) NOT NULL);

CREATE TABLE `soc`.`Updates` (
`UserId` BINARY(16),
`UserName` VARCHAR(100),
`Timestamp` DATETIME,
`Message` VARCHAR(200));