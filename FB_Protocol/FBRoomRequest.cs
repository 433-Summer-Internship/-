struct FBRoomCreateRequest
{
}

struct FBRoomListRequest
{
}

struct FBRoomJoinRequest
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    char[] cookie;
    int roomNum;
}

struct FBRoomLeaveRequest
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    char[] cookie;
}
