DROP procedure IF EXISTS `INUPD_UserPassword`;

CREATE PROCEDURE `INUPD_UserPassword`
(
	IN login VARCHAR(200),
	IN passwordHash binary(32),
    IN salt CHAR(36)
)
BEGIN
	REPLACE INTO UserPassword
    VALUES (login, passwordHash, salt);
END
