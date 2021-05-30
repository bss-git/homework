CREATE PROCEDURE `GET_UpdatesList`
(
	IN userId BINARY(16)
)
BEGIN
	WITH MeAndFriends AS (
		SELECT userId
        UNION
        SELECT fl.FriendId
        FROM FriendLink fl
        WHERE fl.UserId = userid
    )
	SELECT
		u.UserId UserId,
        u.UserName UserName,
        u.Timestamp Timestamp,
        u.Message Message
	FROM Updates u
	INNER JOIN MeAndFriends ids ON ids.userId = u.UserId
	ORDER BY Timestamp DESC
    LIMIT 1000;
END