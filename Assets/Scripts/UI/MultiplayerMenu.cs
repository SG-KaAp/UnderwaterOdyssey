using System;
using Network;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MultiplayerMenu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField lobbyNameInputField;
        [SerializeField] private TMP_InputField lobbyMaxMembersInputField;
        [SerializeField] private TMP_Dropdown lobbyTypeDropdown;
        [SerializeField] private UnityEvent onLobbyCreated;
        [SerializeField] private UnityEvent onLobbyEntered;
        private MatchmakingManager.LobbyType _currentLobbyType;
        public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
        public void Start()
        {
            lobbyNameInputField.text = SteamClient.Name + "'s Lobby";
        }
        private void OnEnable()
        {
            SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
            SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
        }
        private void OnDisable()
        {
            SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
            SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
        }
        public void CreateLobby()
        {
            MatchmakingManager.CreateLobby(_currentLobbyType, Convert.ToInt32(lobbyMaxMembersInputField.text));
            MatchmakingManager.CurrentLobby.SetData("LobbyName",lobbyNameInputField.text);
        }

        public void GameQuit() => Application.Quit();
        public void SetLobbyTypeVar()
        {
            switch (lobbyTypeDropdown.value)
            {
                case 0:
                    _currentLobbyType = MatchmakingManager.LobbyType.Private;
                    break;
                case 1:
                    _currentLobbyType = MatchmakingManager.LobbyType.FriendsOnly;
                    break;
                case 2:
                    _currentLobbyType = MatchmakingManager.LobbyType.Public;
                    break;
            }
            Debug.Log("Lobby type changed. New type: " + _currentLobbyType);
        }
        private void OnLobbyCreated(Result result, Lobby lobby)
        {
            onLobbyCreated.Invoke();
        }
        private void OnLobbyEntered(Lobby lobby)
        {
            onLobbyEntered.Invoke();
        }
    }
}