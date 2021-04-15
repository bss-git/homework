DROP procedure IF EXISTS `INUPD_User`;

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
END