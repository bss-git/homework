DROP procedure IF EXISTS `GET_FriendOffer`;

CREATE PROCEDURE `GET_FriendOffer`
(
	IN `from` BINARY(16),
	IN `to` BINARY(16)
)
BEGIN
	SELECT
		fo.Id id,
        fo.From `from`,
        fo.To `to`
	FROM FriendOffer fo
    WHERE fo.From = `from`
		AND fo.To = `to`;
END
