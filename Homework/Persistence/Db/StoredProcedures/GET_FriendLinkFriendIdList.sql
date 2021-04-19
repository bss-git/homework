CREATE PROCEDURE `GET_FriendLinkFriendIdList`
(
	IN userId BINARY(16)
)
BEGIN
	SELECT fl.FriendId friendId
    FROM FriendLink fl
    WHERE fl.UserId = userId;
END
