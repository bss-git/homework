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
END
