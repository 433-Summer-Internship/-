struct FBConnectionPassResponse
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    char[] cookie;
}
