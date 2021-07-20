use `dialogs`;

CREATE TABLE `dialogs`.`Messages` (
  `Id` binary(16) NOT NULL,
  `From` binary(16) NOT NULL,
  `To` binary(16) NOT NULL,
  `Text` TEXT NOT NULL,
  `Timestamp` Datetime NOT NULL,
  `HashCode` int NOT NULL,
PRIMARY KEY (`Id`));

CREATE INDEX `idx_Messages_FromTo` ON `dialogs`.`Messages` (`From`, `To`) COMMENT '' ALGORITHM DEFAULT LOCK DEFAULT;
CREATE INDEX `idx_Messages_ToFrom` ON `dialogs`.`Messages` (`To`, `From`) COMMENT '' ALGORITHM DEFAULT LOCK DEFAULT;
CREATE INDEX `idx_Messages_HashCode` ON `dialogs`.`Messages` (`HashCode`) COMMENT '' ALGORITHM DEFAULT LOCK DEFAULT;