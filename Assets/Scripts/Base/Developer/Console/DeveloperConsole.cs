using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Base.Developer.Console
{
    public class DeveloperConsole : MonoBehaviour
    {
        [SerializeField] private GameObject developerConsolePanel;
        [SerializeField] private TextMeshProUGUI logText;
        [SerializeField] private bool lockMouseCursor;
        [SerializeField] private List<CommandScriptableObject> commands;
        [SerializeField] private TMP_InputField inputField;
        private bool _developerConsoleShow;
        private DefaultAction _input;

        private void Awake()
        {
            _input = new DefaultAction();
            DontDestroyOnLoad(this);
            Application.logMessageReceived += PrintConsoleLogVar2;
            _input.Developer.Console.performed += context => Switch();
            _input.Developer.SendCommand.performed += context => RunCommand();
            Debug.Log("SGTeam's Unity Base, 2025\nGame Version: " + Application.version + "\nUnity Version: " + Application.unityVersion + "\n");
        }
        private void OnEnable() { _input.Enable(); }
        private void OnDisable() { _input.Disable(); }
        public void PrintConsoleLog(string logString)
        {
            logText.text += logString + "\n";
        }
        public void PrintConsoleLogVar2(string logString, string stackTrace, LogType type)
        {
            logText.text += logString + "\n";
        }
        public void RunCommand()
        {
            for (int i = 0; i+1 <= commands.Count; i++)
            {
                if (inputField.text.ToLower() == commands[i].CommandName)
                {
                    Debug.Log(commands[i].CommandBack);
                }
            }
            inputField.text = "";
        }
        private void Switch()
        {
            if (!developerConsolePanel.activeSelf)
            {
                developerConsolePanel.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                if (developerConsolePanel.activeSelf)
                {
                    developerConsolePanel.SetActive(false);
                    if (SceneManager.GetActiveScene().name == "Inbound") lockMouseCursor = true;
                    if (lockMouseCursor) Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }
}