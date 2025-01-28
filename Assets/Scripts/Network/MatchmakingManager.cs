using UnityEngine;
using Steamworks;
using Steamworks.Data;

namespace Network
{
    public class MatchmakingManager : MonoBehaviour
    {
        [SerializeField] private FishyFacepunch.FishyFacepunch fishyFacepunch;
        public enum LobbyType {Private, FriendsOnly, Public}
        public static Lobby CurrentLobby;
        public static SteamId CurrentLobbyId;
        public static LobbyType CurrentLobbyType;
        private void OnEnable()
        {
            SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
            SteamFriends.OnGameLobbyJoinRequested += LobbyJoinRequest;
            SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
        }
        private void OnDisable()
        {
            SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
            SteamFriends.OnGameLobbyJoinRequested -= LobbyJoinRequest;
            SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
        }

        public async static void CreateLobby(LobbyType lobbyType, int maxPlayers)
        {
            try
            {
                var result = await SteamMatchmaking.CreateLobbyAsync(maxPlayers);
                CurrentLobby = result.Value;
                CurrentLobbyId = CurrentLobby.Id;
                CurrentLobbyType = lobbyType;
                switch (CurrentLobbyType)
                {
                    case LobbyType.Private:
                        CurrentLobby.SetPrivate();
                        break;
                    case LobbyType.FriendsOnly:
                        CurrentLobby.SetFriendsOnly();
                        break;
                    case LobbyType.Public:
                        CurrentLobby.SetPublic();
                        break;
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("Steam creating lobby caused an error. Error: " + e.Message);
            }
        }
        public static void SetLobbyType(LobbyType lobbyType)
        {
            CurrentLobbyType = lobbyType;
            switch (lobbyType)
            {
                case LobbyType.Private:
                    CurrentLobby.SetPrivate();
                    break;
                case LobbyType.FriendsOnly:
                    CurrentLobby.SetFriendsOnly();
                    break;
                case LobbyType.Public:
                    CurrentLobby.SetPublic();
                    break;
            }
        }
        private async void LobbyJoinRequest(Lobby lobby, SteamId friendId)
        {
            await lobby.Join();
        }
        private void OnLobbyCreated(Result result, Lobby lobby)
        {
            if (result != Result.OK) return;
            lobby.SetData("HostAddress", SteamClient.SteamId.ToString());
            lobby.SetData("GameVersion", Application.version);
            fishyFacepunch.SetClientAddress(SteamClient.SteamId.ToString());
            fishyFacepunch.StartConnection(true);
            Debug.Log("The lobby has been created successfully! HostAddress: " + lobby.GetData("Hostaddress") + ". Lobby name: " + lobby.GetData("LobbyName") + ". Lobby ID: " + lobby.Id + ". Max players: " + lobby.MaxMembers + ". Lobby type: " + CurrentLobbyType + ". Owner: " + lobby.Owner);
        }
        private void OnLobbyEntered(Lobby lobby)
        {
            CurrentLobby = lobby;
            CurrentLobbyId = lobby.Id;
            fishyFacepunch.SetClientAddress(CurrentLobby.GetData("HostAddress"));
            fishyFacepunch.StartConnection(false);
            Debug.Log("Entered the lobby");
        }
    }
}