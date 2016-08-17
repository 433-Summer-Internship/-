```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace client
{
    public struct Protocol
    {
        public ushort command;
        public ushort length;
        public byte[] data;
    }

    public class function
    {
        public static byte[] ProtocolToByteArray(Protocol input)
        {
            byte[] buffer = new byte[(sizeof(ushort) * 2) + input.data.Length];
            BitConverter.GetBytes(input.command).CopyTo(buffer, 0);
            BitConverter.GetBytes(input.length).CopyTo(buffer, sizeof(ushort));
            input.data.CopyTo(buffer, sizeof(ushort) * 2);
            return buffer;
        }

        public static Protocol bytearraytoprotocol(byte[] input)
        {
            Protocol output = new Protocol();

            output.command = BitConverter.ToUInt16(input, 0);
            output.length = BitConverter.ToUInt16(input, sizeof(ushort));

            if (output.length <= 0)
                output.data = new byte[0];
            else
            {
                output.data = new byte[output.length];
                Array.ConstrainedCopy(input, sizeof(ushort) * 2, output.data, 0, output.length);
            }

            return output;
        }
    }

    public class Commands
    {
        public const ushort SIGNUP = 100;
        public const ushort SIGNUP_FAIL = 102;
        public const ushort SIGNUP_SUCCESS = 105;

        public const ushort DELETE_USER = 110;
        public const ushort DELETE_USER_FAIL = 112;
        public const ushort DELETE_USER_SUCCESS = 115;

        public const ushort UPDATE_USER = 120;
        public const ushort UPDATE_USER_USER_FAIL = 122;
        public const ushort UPDATE_USER_SUCCESS = 125;

        public const ushort SIGNIN = 200;
        public const ushort SIGNIN_FAIL = 202;
        public const ushort SIGNIN_SUCCESS = 205;

        public const ushort AUTH_TO_CHAT = 210;
        public const ushort AUTH_TO_CHAT_FAIL = 212;
        public const ushort AUTH_TO_CHAT_SUCCESS = 215;

        public const ushort DUMMY_SIGNIN = 220;
        public const ushort DUMMY_SIGNIN_FAIL = 222;
        public const ushort DUMMY_SIGNIN_SUCCESS = 225;

        public const ushort LOGOUT = 300;
        public const ushort LOGOUT_FAIL = 302;
        public const ushort LOGOUT_SUCCESS = 305;

        public const ushort ROOM_LIST = 400;
        public const ushort ROOM_LIST_FAIL = 402;
        public const ushort ROOM_LIST_SUCCESS = 405;

        public const ushort CREATE_ROOM = 500;
        public const ushort CREATE_ROOM_FAIL = 502;
        public const ushort CREATE_ROOM_SUCCESS = 505;

        public const ushort JOIN_ROOM = 600;
        public const ushort JOIN_ROOM_FAIL = 602;
        public const ushort JOIN_ROOM_SUCCESS = 605;
        public const ushort ROOM_FULL = 615;
        public const ushort WRONG_SERVER = 625;
        public const ushort NO_ROOM = 635;


        public const ushort LEAVE_ROOM = 700;
        public const ushort LEAVE_ROOM_FAIL = 702;
        public const ushort LEAVE_ROOM_SUCCESS = 705;

        public const ushort DESTROY_ROOM = 800;
        public const ushort DESTROY_ROOM_FAIL = 802;
        public const ushort DESTROY_ROOM_SUCCESS = 805;

        public const ushort MSG = 900;
        public const ushort MSG_FAIL = 902;
        public const ushort MSG_SUCCESS = 905;

        public const ushort HEARTBEAT = 1000;
        public const ushort HEARTBEAT_FAIL = 1002;
        public const ushort HEARTBEAT_SUCCESS = 1005;
    }
}
    ```
