struct FBSignupResponse
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    char[] cookie;
}
struct FBDeleteUserResponse
{
}
struct FBUpdateUserResponse
{
}

struct FBDummySigninResponse
{
}
//Login and ConnectPassing
struct FBSigninResponse
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
    char[] ip;
    int port;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    char[] cookie;
}

struct FBSignoutResponse
{
}
