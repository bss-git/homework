function get_user_by_login(login)
	return box.space.mysql_users:get({login})
end