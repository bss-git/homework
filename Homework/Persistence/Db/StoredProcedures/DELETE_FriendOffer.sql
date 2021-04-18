CREATE PROCEDURE `DELETE_FriendOffer` 
(
	IN id BINARY(16)
)
BEGIN
	DELETE FROM FriendOffer fo
    WHERE fo.Id = id;
END
