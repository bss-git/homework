DROP procedure IF EXISTS `GET_UserFriends`;

CREATE PROCEDURE `GET_UserFriends`
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