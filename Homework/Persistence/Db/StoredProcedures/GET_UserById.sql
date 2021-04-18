DROP procedure IF EXISTS `GET_UserById`;

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
	FROM User u;
END
