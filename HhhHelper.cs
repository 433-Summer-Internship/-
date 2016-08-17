using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.BitConverter;

namespace Joonhaehok
{
    public struct Header
    {
        public ushort code;
        public ushort size;
    }
    public struct Packet
    {
        public Header header;
        public byte[] data;
    }

    public static class Hhhhelper
    {
        public static byte[] PacketToBytes(Packet packet)
        {
            byte[] buffer = new byte[(sizeof(ushort) * 2) + packet.data.Length];
            Array.Copy(GetBytes(packet.code), 0, buffer, FieldIndex.CODE, sizeof(ushort));
            Array.Copy(GetBytes(packet.size), 0, buffer, FieldIndex.SIZE, sizeof(ushort));
            Array.Copy(packet.data, 0, buffer, FieldIndex.DATA, p.data.Length);
            return buffer;
        }

        public static Packet BytesToPacket(byte[] bytes)
        {
            Packet packet = new Packet();

            packet.header.code = ToUInt16(bytes, FieldIndex.CODE);
            packet.header.size = ToUInt16(bytes, FieldIndex.SIZE);
            Array.Copy(bytes, FieldIndex.DATA, packet.data, 0, packet.header.size);

            return packet;
        }
            
        public static Header BytesToHeader(byte[] bytes)
        {
            Header header = new Header();

            header.code = ToUint16(bytes, FieldIndex.CODE);
            header.size = ToUint16(bytes, FieldIndex.SIZE);

            return header;
        }

        public class FieldIndex
        {
            public const int CODE = 0;
            public const int SIZE = 2;
            public const int DATA = 4;
        }

        public class Code
        {
            public const ushort SIGNUP = 100;
            public const ushort SIGNUP_SUCCESS = 102;
            public const ushort SIGNUP_FAIL = 105;
            public const ushort DELETE_USER = 110;
            public const ushort DELETE_USER_SUCCESS = 112;
            public const ushort DELETE_USER_FAIL = 115;
            public const ushort UPDATE_USER = 120;
            public const ushort UPDATE_USER_SUCCESS = 122;
            public const ushort UPDATE_USER_USER_FAIL = 125;

            public const ushort SIGNIN = 200;
            public const ushort SIGNIN_SUCCESS = 202;
            public const ushort SIGNIN_FAIL = 205;
            public const ushort AUTH_TO_CHAT = 210;
            public const ushort AUTH_TO_CHAT_SUCCESS = 212;
            public const ushort AUTH_TO_CHAT_FAIL = 215;
            public const ushort DUMMY_SIGNIN = 220;
            public const ushort DUMMY_SIGNIN_SUCCESS = 222;
            public const ushort DUMMY_SIGNIN_FAIL = 225;

            public const ushort SIGNOUT = 300;
            public const ushort SIGNOUT_SUCCESS = 302;
            public const ushort SIGNOUT_FAIL = 305;

            public const ushort ROOM_LIST = 400;
            public const ushort ROOM_LIST_SUCCESS = 402;
            public const ushort ROOM_LIST_FAIL = 405;

            public const ushort CREATE_ROOM = 500;
            public const ushort CREATE_ROOM_SUCCESS = 502;
            public const ushort CREATE_ROOM_FAIL = 505;

            public const ushort JOIN = 600;
            public const ushort JOIN_SUCCESS = 602;
            public const ushort JOIN_FAIL = 605;
            public const ushort JOIN_FULL_FAIL = 615;
            public const ushort JOIN_NULL_FAIL = 625;
            public const ushort JOIN_REDIRECT = 650;
            public const ushort JOIN_REDIRECT_SUCCESS = 652;
            public const ushort JOIN_REDIRECT_FAIL = 655;

            public const ushort LEAVE_ROOM = 700;
            public const ushort LEAVE_ROOM_SUCCESS = 702;
            public const ushort LEAVE_ROOM_FAIL = 705;

            public const ushort DESTROY_ROOM = 800;
            public const ushort DESTROY_ROOM_SUCCESS = 802;
            public const ushort DESTROY_ROOM_FAIL = 805;

            public const ushort MSG = 900;
            public const ushort MSG_SUCCESS = 902;
            public const ushort MSG_FAIL = 905;

            public const ushort HEARTBEAT = 1000;
            public const ushort HEARTBEAT_SUCCESS = 1002;
            public const ushort HEARTBEAT_FAIL = 1005;
        }
    }
}
