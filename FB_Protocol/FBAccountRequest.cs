struct FBSignupRequest
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
    char[] user;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
    char[] password;
}

struct FBDeleteUserRequest
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    char[] cookie;
}

struct FBUpdateUserRequest
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    char[] cookie;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
    char[] password;
}

struct FBDummySigninRequest
{
}

struct FBSigninRequest
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
    char[] user;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
    char[] password;
}

struct FBSignoutRequest
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    char[] cookie;
}
