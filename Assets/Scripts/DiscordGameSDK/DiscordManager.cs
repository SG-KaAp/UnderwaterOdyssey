using System;
using System.Collections.Generic;
using Discord;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace DiscordGameSDK
{
    public class DiscordManager: MonoBehaviour
    {
        [SerializeField] private long discordAppId = 1331652824592814110;
        [SerializeField] private List<SceneDetails> scenesDetailsList;
        private Discord.Discord _discord;
        private ActivityManager _activityManager;
        private OverlayManager _overlayManager;
        private long _time;
        private string _details;

        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            _time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            _discord = new Discord.Discord(discordAppId, (UInt64) CreateFlags.Default);
            _activityManager = _discord.GetActivityManager();
        }

        private void Update()
        {
            try
            {
                foreach (SceneDetails deatils in scenesDetailsList)
                {
                    foreach (Object scene in deatils.ScenesForDescription)
                    {
                        if (scene.name == SceneManager.GetActiveScene().name) _details = deatils.Description;
                    }
                    break;
                }
                var activity = new Activity
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
                    Party =
                    {
                        Id = "party-abc123",
                        Size = {
                            CurrentSize = 3,
                            MaxSize = 3,
                        },
                    },
                    Secrets =
                    {
                        Join = "foo joinSecret",
                    }
                };
                _activityManager.UpdateActivity(activity, (result) => {});
                _discord.RunCallbacks();
            }
            catch
            {
                gameObject.SetActive(false);
            }
        }

        private void OnApplicationQuit()
        {
            _discord.Dispose();
        }
    }
    [Serializable]
    public class SceneDetails
    {
        [SerializeField] private List<Object> scenesForDescription;
        [SerializeField] private string sceneDescription;
        public List<Object> ScenesForDescription => scenesForDescription;
        public string Description => sceneDescription;
    }
}