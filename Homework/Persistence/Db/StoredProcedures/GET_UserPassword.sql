DROP procedure IF EXISTS `GET_UserPassword`;

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
END