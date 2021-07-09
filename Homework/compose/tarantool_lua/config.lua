box.cfg {
    listen = 3301;
}

if not box.space.mysql_users then
    t = box.schema.space.create('mysql_users')
    t:create_index('primary', {
        type = 'tree',
        parts={2, 'string'},
        unique=false
    })
end

function get_user_by_login(login)
    return box.space.mysql_users:get({login})
end

function insert_user(id, login, name, surname, birthDate, city, gender, interest)
    box.space.mysql_users:replace({id, login, name, surname, birthDate, city, gender, interest})
end