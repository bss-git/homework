DROP procedure IF EXISTS `GET_UserByLogin`;

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
END