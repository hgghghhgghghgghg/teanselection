using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Commands;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Events;
using Rocket.Unturned.Items;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using static TeamSelection.Config;
using static TeamSelection.Type;

namespace TeamSelection
{
    public class Plugin : RocketPlugin<Config>
    {
        public static Plugin Instance;

        protected override void Load()
        {
            Plugin.Instance = this;

            U.Events.OnPlayerConnected += OnPlayerConnected;
            EffectManager.onEffectButtonClicked += OnButtonClicked;
        }



        private void OnButtonClicked(Player player, string buttonName)
        {
            UnturnedPlayer uplayer = UnturnedPlayer.FromPlayer(player);

            if (buttonName == "VSRFButton")
            {
                // Выдача пермишена для команды VSRF
                string groupName = "vsrf";
                R.Permissions.AddPlayerToGroup(groupName, uplayer);

                player.quests.ServerAssignToGroup(new CSteamID(300), EPlayerGroupRank.MEMBER, true);

                // Установите координаты
                uplayer.Teleport(new Vector3(100, 100, 100), uplayer.Rotation);
                Instance.Configuration.Instance.Commands.Add(new Command { PlayerID = uplayer.CSteamID, _Type = ECommandType.VSRF });
                Plugin.Instance.Configuration.Save();
                UnturnedChat.Say(uplayer.CSteamID, Translate("join_team_1"));
            }
            if (buttonName == "CHRIButton")
            {
                // Выдача пермишена для команды CHRI
                string groupName = "chri";
                R.Permissions.AddPlayerToGroup(groupName, uplayer);


                player.quests.ServerAssignToGroup(new CSteamID(400), EPlayerGroupRank.MEMBER, true);

                // Устанавливаем координаты
                uplayer.Teleport(new Vector3(100, 100, 100), uplayer.Rotation);
                Instance.Configuration.Instance.Commands.Add(new Command { PlayerID = uplayer.CSteamID, _Type = ECommandType.CHRI });
                Plugin.Instance.Configuration.Save();
                UnturnedChat.Say(uplayer.CSteamID, Translate("join_team_2"));
            }

            EffectManager.askEffectClearByID(7500, uplayer.CSteamID);
            uplayer.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);
        }

        public bool isTeamCreated;


        private void OnPlayerConnected(UnturnedPlayer uplayer)
        {
            var CheckPlayer = Instance.Configuration.Instance.Commands.Find(p => p.PlayerID == uplayer.CSteamID);
            if (CheckPlayer == null)
            {
                //телепорт в лобби
                //Установи координаты
                uplayer.Teleport(new Vector3(100, 100, 100), uplayer.Rotation);
                EffectManager.sendUIEffect(7500, 7500, uplayer.CSteamID, true);
                uplayer.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);
            }
            try
            {
                if (!isTeamCreated)
                {
                    GroupManager.addGroup(new CSteamID(300), "VSRF");
                    GroupManager.addGroup(new CSteamID(400), "CHRI");
                    Console.WriteLine("Группы успешно созданы!");
                    isTeamCreated = true;
                }
            }
            catch { }


        }
  

        public override TranslationList DefaultTranslations => new TranslationList()
{
{ "plugin_name", "Plugin TeamSelection by Villir" },
{ "join_team_1", "Вы зашли в команду VSRF" },
{ "join_team_2", "Вы зашли в команду CHRI" }
};



        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= OnPlayerConnected;
            EffectManager.onEffectButtonClicked -= OnButtonClicked;
            Console.WriteLine("Plugin TeamSelection unloaded! by Villir");
        }
    }
}