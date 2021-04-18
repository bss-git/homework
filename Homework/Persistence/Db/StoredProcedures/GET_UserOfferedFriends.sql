DROP procedure IF EXISTS `GET_UserOfferedFriends`;

CREATE PROCEDURE `GET_UserOfferedFriends`
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
	FROM User u
    INNER JOIN FriendOffer fo ON u.Id = fo.From
    WHERE fo.To = userId;
END
