struct CFRoomCreateResponse
{
    int roomNum;
}

struct CFRoomListResponse
{
}

struct CFRoomJoinResponse
{
}

struct CFRoomLeaveResponse
{
}

struct CFRoomJoinRedirectResponse
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
    char[] ip;
    int port;
}
