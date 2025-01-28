using System;
using System.Collections.Generic;
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
        [SerializeField] private GameObject lobbyLinePrefab;
        [SerializeField] private GameObject viewport;
        [SerializeField] private TMP_InputField lobbyNameInputField;
        [SerializeField] private TMP_InputField lobbyMaxMembersInputField;
        [SerializeField] private TMP_Dropdown lobbyTypeDropdown;
        [SerializeField] private UnityEvent onLobbyCreated;
        [SerializeField] private UnityEvent onLobbyEntered;
        private MatchmakingManager.LobbyType _currentLobbyType;
        private Lobby[] _lobbies;
        private List<GameObject> _lobbyList = new List<GameObject>();
        public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
        public void SetLobbyName() => MatchmakingManager.CurrentLobby.SetData("LobbyName",lobbyNameInputField.text);
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
        }
        public async void RefreshLobbyList()
        {
            foreach (GameObject lobbyLine in _lobbyList)
            {
                Destroy(lobbyLine);
            }
            //Lobby[] lobbies = await SteamMatchmaking.LobbyList.WithKeyValue("GameVersion", Application.version).RequestAsync();
            LobbyQuery lobbyQuery = new LobbyQuery();
            lobbyQuery.WithKeyValue("GameVersion", Application.version);
            _lobbies = await lobbyQuery.RequestAsync();
            if (_lobbies != null)
            {
                foreach (Lobby lobby in _lobbies)
                {
                    var lobbyLine = Instantiate(lobbyLinePrefab, viewport.transform);
                    var lobbyLineComponent = lobbyLine.GetComponent<LobbyLine>();
                    lobbyLineComponent.SetLobby(lobby);
                    _lobbyList.Add(lobbyLine);
                }
            }
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