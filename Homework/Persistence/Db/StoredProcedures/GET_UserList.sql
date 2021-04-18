﻿DROP procedure IF EXISTS `GET_UserList`;

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
END