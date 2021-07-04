function insert_user(login, id, name, surname, birthDate, city, gender, interest)
	box.space.mysql_users:replace({login, id, name, surname, birthDate, city, gender, interest})
end