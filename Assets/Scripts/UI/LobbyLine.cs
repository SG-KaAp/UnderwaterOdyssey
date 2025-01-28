using TMPro;
using UnityEngine;
using Steamworks.Data;

namespace UI
{
    public class LobbyLine : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lobbyName;
        [SerializeField] private TextMeshProUGUI lobbyMembers;
        public Lobby thisLobby { get; private set; }
        public void SetLobby(Lobby lobby)
        {
            thisLobby = lobby;
            SetLobbyName(thisLobby.GetData("LobbyName"));
            SetLobbyMembers(thisLobby.MemberCount, thisLobby.MaxMembers);
        }
        public void EnterLobby()
        {
            thisLobby.Join();
        }
        private void SetLobbyName(string name)
        {
            lobbyName.text = name;
        }
        private void SetLobbyMembers(int members, int maxMembers)
        {
            lobbyMembers.text = members.ToString() + "/" + maxMembers.ToString();
        }
    }
}