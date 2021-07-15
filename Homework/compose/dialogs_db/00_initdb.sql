create database `dialogs`;

CREATE TABLE `dialogs`.`Messages` (
  `Id` binary(16) NOT NULL,
  `From` binary(16) NOT NULL,
  `To` binary(16) NOT NULL,
  `Text` TEXT NOT NULL,
  `Timestamp` Date NOT NULL,
PRIMARY KEY (`Id`));

DROP procedure IF EXISTS `GET_MessagesList`;

CREATE PROCEDURE `GET_MessagesList`
(
	IN user1 binary(16),
	IN user2 binary(16)
)
BEGIN
	SELECT
		m.Id Id,
        m.From From,
        m.To To,
        m.Text Text,
        m.Timestamp Timestamp,
	FROM Messages m
    WHERE m.From = user1 AND m.To = user2;
	UNION ALL
	SELECT
		m.Id Id,
        m.From From,
        m.To To,
        m.Text Text,
        m.Timestamp Timestamp,
	FROM Messages m
    WHERE m.From = user2 AND m.To = user1;
END

DROP procedure IF EXISTS `INSERT_Messages`;

CREATE PROCEDURE `INSERT_Messages`
(
    IN `id` binary(16),
    IN `from` binary(16),
    IN `to` binary(16),
    IN `text` TEXT,
    IN `timestamp` Date
)
BEGIN
	INSERT INTO Messages
	VALUES (`id`, `from`, `to`, `text`, `timestamp`)
END