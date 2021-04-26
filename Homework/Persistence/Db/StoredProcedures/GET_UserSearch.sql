DROP procedure IF EXISTS `GET_UserSearch`;

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
        AND u.Surname LIKE concat(`surname`, '%');
END