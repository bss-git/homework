use `soc`;

DROP procedure IF EXISTS `DELETE_FriendOffer`;

DELIMITER $$
USE `soc`$$
CREATE PROCEDURE `DELETE_FriendOffer` 
(
	IN id BINARY(16)
)
BEGIN
	DELETE FROM FriendOffer fo
    WHERE fo.Id = id;
END;$$

DELIMITER ;

use `soc`;

DROP procedure IF EXISTS `GET_FriendLinkFriendIdList`;

DELIMITER $$
USE `soc`$$
CREATE PROCEDURE `GET_FriendLinkFriendIdList`
(
	IN userId BINARY(16)
)
BEGIN
	SELECT fl.FriendId friendId
    FROM FriendLink fl
    WHERE fl.UserId = userId;
END;$$

DELIMITER ;

use `soc`;

DROP procedure IF EXISTS `GET_FriendOffer`;

DELIMITER $$
USE `soc`$$
CREATE PROCEDURE `GET_FriendOffer`
(
	IN `from` BINARY(16),
	IN `to` BINARY(16)
)
BEGIN
	SELECT
		fo.Id id,
        fo.From `from`,
        fo.To `to`
	FROM FriendOffer fo
    WHERE fo.From = `from`
		AND fo.To = `to`;
END;$$

DELIMITER ;

use `soc`;

DROP procedure IF EXISTS `GET_UpdatesList`;

DELIMITER $$
USE `soc`$$
CREATE PROCEDURE `GET_UpdatesList`
(
	IN userId BINARY(16)
)
BEGIN
	WITH MeAndFriends AS (
		SELECT userId
        UNION
        SELECT fl.FriendId
        FROM FriendLink fl
        WHERE fl.UserId = userid
    )
	SELECT
		u.UserId UserId,
        u.UserName UserName,
        u.Timestamp Timestamp,
        u.Message Message
	FROM Updates u
	INNER JOIN MeAndFriends ids ON ids.userId = u.UserId
	ORDER BY Timestamp DESC
    LIMIT 1000;
END;$$

DELIMITER ;

use `soc`;

DROP procedure IF EXISTS `GET_UserById`;

DELIMITER $$
USE `soc`$$
CREATE PROCEDURE `GET_UserById`
(
	IN id BINARY(16)
)
BEGIN
	SELECT
		u.Id Id,
        u.Login Login,
        u.Name Name,
        u.Surname Surname,
        u.City City,
        u.Gender Gender,
        u.BirthDate BirthDate,
        u.Interest Interest
	FROM User u
    WHERE u.Id = id;
END;$$

DELIMITER ;

use `soc`;

DROP procedure IF EXISTS `GET_UserByLogin`;

DELIMITER $$
USE `soc`$$
CREATE PROCEDURE `GET_UserByLogin`
(
	IN login VARCHAR(200)
)
BEGIN
	SELECT
		u.Id Id,
        u.Login Login,
        u.Name Name,
        u.Surname Surname,
        u.City City,
        u.Gender Gender,
        u.BirthDate BirthDate,
        u.Interest Interest
	FROM User u
    WHERE u.Login = login;
END;$$

DELIMITER ;

use `soc`;

DROP procedure IF EXISTS `GET_UserFriends`;

DELIMITER $$
USE `soc`$$
CREATE PROCEDURE `GET_UserFriends`
(
	IN userId BINARY(16)
)
BEGIN
	SELECT
		u.Id Id,
        u.Login Login,
        u.Name Name,
        u.Surname Surname,
        u.City City
	FROM User u
    INNER JOIN FriendLink fl ON fl.FriendId = u.Id
    WHERE fl.UserId = userId;
END;$$

DELIMITER ;

use `soc`;

DROP procedure IF EXISTS `GET_UserList`;

DELIMITER $$
USE `soc`$$
CREATE PROCEDURE `GET_UserList`
(
	IN `offset` INT,
    IN `limit` TINYINT
)
BEGIN
	SELECT
		u.Id Id,
        u.Login Login,
        u.Name Name,
        u.Surname Surname,
        u.City City
	FROM User u
    LIMIT `limit`
    OFFSET `offset`;
END;$$

DELIMITER ;

use `soc`;

DROP procedure IF EXISTS `GET_UserOfferedFriends`;

DELIMITER $$
USE `soc`$$
CREATE PROCEDURE `GET_UserOfferedFriends`
(
	IN userId BINARY(16)
)
BEGIN
	SELECT
		u.Id Id,
        u.Login Login,
        u.Name Name,
        u.Surname Surname,
        u.City City
	FROM User u
    INNER JOIN FriendOffer fo ON u.Id = fo.From
    WHERE fo.To = userId;
END;$$

DELIMITER ;

use `soc`;

DROP procedure IF EXISTS `GET_UserPassword`;

DELIMITER $$
USE `soc`$$
CREATE PROCEDURE `GET_UserPassword`
(
	IN login varchar(200)
)
BEGIN
	SELECT
		u.PasswordHash PasswordHash,
        u.Salt Salt
	FROM UserPassword u
    WHERE u.Login = login;
END;$$

DELIMITER ;


use `soc`;

DROP procedure IF EXISTS `GET_UserSearch`;

DELIMITER $$
USE `soc`$$
CREATE PROCEDURE `GET_UserSearch`
(
	IN `name` varchar(100),
    IN `surname` varchar(100)
)
BEGIN
	SELECT
		u.Id Id,
        u.Login Login,
        u.Name Name,
        u.Surname Surname,
        u.City City
	FROM User u
    WHERE u.Name LIKE concat(`name`, '%')
        AND u.Surname LIKE concat(`surname`, '%')
    ORDER BY u.Id;
END;$$

DELIMITER ;

use `soc`;

DROP procedure IF EXISTS `INSERT_FriendLink`;

DELIMITER $$
USE `soc`$$
CREATE PROCEDURE `INSERT_FriendLink`
(
	IN friend1 BINARY(16),
	IN friend2 BINARY(16)
)
BEGIN
	INSERT INTO FriendLink
    VALUES
		(friend1, friend2),
        (friend2, friend1);
END;$$

DELIMITER ;


use `soc`;

DROP procedure IF EXISTS `INSERT_FriendOffer`;

DELIMITER $$
USE `soc`$$
CREATE PROCEDURE `INSERT_FriendOffer`
(
	IN id BINARY(16),
	IN `from` BINARY(16),
 	IN `to` BINARY(16)   
)
BEGIN
	INSERT INTO FriendOffer
    VALUES (id, `from`, `to`);
END;$$

DELIMITER ;

use `soc`;

DROP procedure IF EXISTS `INSERT_Update`;

DELIMITER $$
USE `soc`$$
CREATE PROCEDURE `INSERT_Update`
(
	IN userId BINARY(16),
	IN userName VARCHAR(100),
	IN timestamp DATETIME,
	IN message VARCHAR(200)
)
BEGIN
	INSERT INTO Updates
    VALUES
		(userId, userName, timestamp, message);
END;$$

DELIMITER ;

use `soc`;

DROP procedure IF EXISTS `INUPD_User`;

DELIMITER $$
USE `soc`$$
CREATE PROCEDURE `INUPD_User`
(
	IN id binary(16),
    IN login varchar(200),
    IN name varchar(100),
    IN surname varchar(100),
    IN city varchar(100),
    IN gender tinyint(1),
    IN birthDate date,
    IN interest varchar(4000)
)
BEGIN
	REPLACE INTO User
    VALUES (id, login, name, surname, city, gender, birthDate, interest);
END;$$

DELIMITER ;

use `soc`;

DROP procedure IF EXISTS `INUPD_UserPassword`;

DELIMITER $$
USE `soc`$$
CREATE PROCEDURE `INUPD_UserPassword`
(
	IN login VARCHAR(200),
	IN passwordHash binary(32),
    IN salt CHAR(36)
)
BEGIN
	REPLACE INTO UserPassword
    VALUES (login, passwordHash, salt);
END;$$

DELIMITER ;