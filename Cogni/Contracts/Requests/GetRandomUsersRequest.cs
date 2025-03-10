namespace Cogni.Contracts.Requests
{
    public record GetRandomUsersRequest
    (
        int startsFrom,//скольких пользователей нужно пропустить 
        int limit//скольких пользователей вернуть
    );
}
