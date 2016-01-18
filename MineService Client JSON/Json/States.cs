using System;

namespace MineService_JSON
{
    public class States
    {
        [Flags]
        public enum MCCommandTYPE { Start, Stop, Restart, Raw, Create, Delete };
        [Flags]
        public enum MessageTYPE { MCCommand, Login, Status, Console, Error, StatusArray, OverviewData, FTPData, Settings, Users};
        [Flags]
        public enum StatusType { Request, Send};
    }
}
