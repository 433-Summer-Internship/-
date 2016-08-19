struct CFInitializeRequest
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    char[] cookie;
}
