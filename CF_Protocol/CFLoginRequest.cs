struct CFLoginRequest
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        char[] user;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
        char[] password;
    }
