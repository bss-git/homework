box.cfg {
    listen = 3301;
 }

if not box.space.mysql_users then
    t = box.schema.space.create('mysql_users')
    t:create_index('primary',
    {type = 'tree', parts = {1, 'string'}, if_not_exists = true})
end

function get_user_by_login(login)
	return box.space.mysql_users:get({login})
end

function insert_user(login, id, name, surname, birthDate, city, gender, interest)
	box.space.mysql_users:replace({login, id, name, surname, birthDate, city, gender, interest})
end