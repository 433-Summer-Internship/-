//Login and ConnectPassing
struct CFLoginResponse
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
    char[] ip;
    int port;
    ushort socketType; 
    
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    char[] cookie;

}
