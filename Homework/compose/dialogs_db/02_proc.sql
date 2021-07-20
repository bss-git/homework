use `dialogs`;

DROP procedure IF EXISTS `GET_MessagesList`;

DELIMITER $$
USE `dialogs`$$
CREATE PROCEDURE `GET_MessagesList`
(
	IN user1 binary(16),
	IN user2 binary(16)
)
BEGIN
	SELECT
		m.Id Id,
        m.From `From`,
        m.To `To`,
        m.Text `Text`,
        m.Timestamp `Timestamp`
	FROM Messages m
    WHERE m.From = user1 AND m.To = user2
	UNION ALL
	SELECT
		m.Id Id,
        m.From `From`,
        m.To `To`,
        m.Text `Text`,
        m.Timestamp `Timestamp`
	FROM Messages m
    WHERE m.From = user2 AND m.To = user1;
END;$$

DELIMITER ;

USE `dialogs`;
DROP procedure IF EXISTS `INSERT_Messages`;

DELIMITER $$
USE `dialogs`$$
CREATE PROCEDURE `INSERT_Messages`
(
    IN `id` binary(16),
    IN `from` binary(16),
    IN `to` binary(16),
    IN `text` TEXT,
    IN `timestamp` Datetime,
    IN `hashCode` int
)
BEGIN
	REPLACE INTO Messages
	VALUES (`id`, `from`, `to`, `text`, `timestamp`, `hashCode`);
END;$$

DELIMITER ;

USE `dialogs`;
DROP procedure IF EXISTS `Get_MessagesHashCodeNotIn`;

DELIMITER $$
USE `dialogs`$$
CREATE PROCEDURE `Get_MessagesHashCodeNotIn`
(
    IN `min` int,
    IN `max` int
)
BEGIN
	SELECT
		m.Id Id,
        m.From `From`,
        m.To `To`,
        m.Text `Text`,
        m.Timestamp `Timestamp`
	FROM Messages m
    WHERE m.HashCode < `min`
	UNION ALL
	SELECT
		m.Id Id,
        m.From `From`,
        m.To `To`,
        m.Text `Text`,
        m.Timestamp `Timestamp`
	FROM Messages m
    WHERE m.HashCode > `max`;
END;$$

DELIMITER ;

USE `dialogs`;
DROP procedure IF EXISTS `Delete_MessagesHashCodeNotIn`;

DELIMITER $$
USE `dialogs`$$
CREATE PROCEDURE `Delete_MessagesHashCodeNotIn`
(
    IN `min` int,
    IN `max` int
)
BEGIN
	DELETE
	FROM Messages m
    WHERE m.HashCode < `min`;

    DELETE
	FROM Messages m
    WHERE m.HashCode > `max`;
END;$$

DELIMITER ;