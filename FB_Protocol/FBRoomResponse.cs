struct FBRoomCreateResponse
{
    int roomNum;
}

struct FBRoomListResponse
{
}

struct FBRoomJoinResponse
{
}

struct FBRoomLeaveResponse
{
}

struct FBRoomJoinRedirectResponse
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
    char[] ip;
    int port;
}
