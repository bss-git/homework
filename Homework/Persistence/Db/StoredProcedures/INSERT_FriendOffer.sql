DROP procedure IF EXISTS `INSERT_FriendOffer`;

CREATE PROCEDURE `INSERT_FriendOffer`
(
	IN id BINARY(16),
	IN `from` BINARY(16),
 	IN `to` BINARY(16)   
)
BEGIN
	INSERT INTO FriendOffer
    VALUES (id, `from`, `to`);
END