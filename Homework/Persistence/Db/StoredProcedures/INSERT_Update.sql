CREATE PROCEDURE `INSERT_Update`
(
	IN userId BINARY(16),
	IN userName VARCHAR(100),
	IN timestamp DATETIME,
	IN message VARCHAR(200)
)
BEGIN
	INSERT INTO Updates
    VALUES
		(userId, userName, timestamp, message);
END
