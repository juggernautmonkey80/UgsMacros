﻿namespace UgsMacros.Framework
{
    public interface ICommandSender
    {
        void SendCommand(string command, bool init = true);
    }
}
