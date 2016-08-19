struct CFSignupResponse
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    char[] cookie;
}
struct CFDeleteUserResponse
{
}
struct CFUpdateUserResponse
{
}

struct CFDummySigninResponse
{
}
//Login and ConnectPassing
struct CFSigninResponse
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
    char[] ip;
    int port;
    ushort socketType; 

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    char[] cookie;
}

struct CFSignoutResponse
{
}
