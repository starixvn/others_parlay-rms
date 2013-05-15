function GetClassByLevel(level) {
    var className = 'levelNoneColor';
    if (level > 0) {
        className = 'level' + level;
    }

    return className;
}

function CreateChangedLevelCell(userid, username, level) {
    var className = GetClassByLevel(level);
    return '<td class="' + className + '"><span>' + username + '</span> <span class="arrowDown_table" title="Click to change level" username="' + username + '" custid="' + userid + '"></span>';
}

function CreateLevelCell(username, level) {
    return '<td class="' + GetClassByLevel(level) + '">' + username + '</td>';
}

function CreateLinkLevelCell(username, level, callback) {
    return '<td class="' + GetClassByLevel(level) + '">' + '<a href="#" onclick="' + callback + '">' + username + '</a>' + '</td>';
}