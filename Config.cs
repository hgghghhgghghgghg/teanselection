using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Rocket.API;
using Steamworks;
using UnityEngine;
using static TeamSelection.Config;

namespace TeamSelection
{
    public class Config : IRocketPluginConfiguration, IDefaultable
    {
        public List<Command> Commands;

        public void LoadDefaults()
        {
            Commands = new List<Command>();
        }
    }

    public class Command
    {
        public CSteamID PlayerID;
        public Type.ECommandType _Type;
    }
}
