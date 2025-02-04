using System;
using System.Collections.Generic;
using Discord;
using Network;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DiscordGameSDK
{
    public class DiscordManager: MonoBehaviour
    {
        [SerializeField] private long discordAppId = 1331652824592814110;
        [SerializeField] private List<SceneDetails> scenesDetailsList;
        private Activity _activity;
        private Discord.Discord _discord;
        private ActivityManager _activityManager;
        private ActivityParty _lobbyParty;
        private ActivitySecrets _secrets;
        private long _time;
        private string _details;
        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            _time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            _discord = new Discord.Discord(discordAppId, (UInt64) CreateFlags.NoRequireDiscord);
            _activityManager = _discord.GetActivityManager();
            _activityManager.OnActivityJoin += secret =>
            {
                SteamId lobbyId = new SteamId();
                lobbyId.Value = Convert.ToUInt64(secret);
                MatchmakingManager.JoinLobbyById(lobbyId);
            };
        }
        private void Update()
        {
            try
            {
                UpdateRichPresence();
                _activityManager.UpdateActivity(_activity, (result) => {});
                _discord.RunCallbacks();
            }
            catch (Exception e)
            {
                Debug.Log("Discord doesn't initialized: " + e.Message);
                gameObject.SetActive(false);
            }
        }
        private void OnApplicationQuit()
        {
            _discord.Dispose();
        }
        private void UpdateRichPresence()
        {
            foreach (SceneDetails deatils in scenesDetailsList)
            {
                foreach (char scene in deatils.ScenesForDescription)
                {
                    if (scene.ToString() == SceneManager.GetActiveScene().name) _details = deatils.Description;
                }
                break;
            }
            if (MatchmakingManager.IsInLobby)
            {
                _lobbyParty = new ActivityParty
                {
                    Id = MatchmakingManager.CurrentLobby.Owner.Id.ToString(),
                    Size =
                    {
                        CurrentSize = MatchmakingManager.CurrentLobby.MemberCount,
                        MaxSize = MatchmakingManager.CurrentLobby.MaxMembers,
                    },
                };
                _secrets = new ActivitySecrets
                {
                    Join = MatchmakingManager.CurrentLobbyId.ToString(),
                };
                _activity = new Activity
                {
                    Details = _details,
                    Assets =
                    {
                        LargeImage = "uo_logo",
                    },
                    Timestamps =
                    {
                        Start = _time,
                    },
                    Party = _lobbyParty,
                    Secrets = _secrets,
                };
            }
            else
            {
                _activity = new Activity
                {
                    Details = _details,
                    Assets =
                    {
                        LargeImage = "uo_logo",
                    },
                    Timestamps =
                    {
                        Start = _time,
                    },
                };
            }
        }
    }
    [Serializable]
    public class SceneDetails
    {
        [SerializeField] private string scenesForDescription;
        [SerializeField] private string sceneDescription;
        public string ScenesForDescription => scenesForDescription;
        public string Description => sceneDescription;
    }
}