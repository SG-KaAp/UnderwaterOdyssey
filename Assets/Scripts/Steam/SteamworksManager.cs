using UnityEngine;
using UnityEngine.Events;
using Steamworks;
using System.IO;

namespace Steam
{
    public class SteamworksManager : MonoBehaviour
    {
        [SerializeField] private uint steamAppIdForInit = 480;
        [SerializeField] private bool steamInitOnAwake = true;
        [SerializeField] private bool dontDestroyOnLoad = true;
        [SerializeField] private bool invokeEventAfterSteamInit = true;
        [SerializeField] private bool invokeEventAfterSteamInitCausedAnError;
        [SerializeField] private UnityEvent unityEventOnSteamInit;
        [SerializeField] private UnityEvent unityEventOnSteamInitCausedAnError;

        private void Start()
        {
            if (steamInitOnAwake && !SteamClient.IsValid)
                SteamInit(steamAppIdForInit);
            if (dontDestroyOnLoad) 
                DontDestroyOnLoad(gameObject);
            if (invokeEventAfterSteamInit && SteamClient.IsValid)
                unityEventOnSteamInit.Invoke();
            else if (invokeEventAfterSteamInitCausedAnError)
                Application.Quit();
        }
        public void SteamInit(uint steamAppId)
        {
            try
            {
                SteamClient.Init(steamAppId);
                Debug.Log("Steam Initialized");
            }
            catch (System.Exception e)
            {
                if (invokeEventAfterSteamInitCausedAnError) unityEventOnSteamInitCausedAnError.Invoke();
                Debug.Log("Steam initialization caused an error. Error: " + e.Message);
            }
        }
        private void OnDestroy()
        {
            try
            {
                SteamClient.Shutdown();
                Debug.Log("Steam application shutdown");
            }
            catch (System.Exception e)
            {
                Debug.Log("Steam shutdown caused an error. Error: " + e.Message);
            }
        }
    }
}