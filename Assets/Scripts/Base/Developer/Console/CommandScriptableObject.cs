using UnityEngine;
using UnityEngine.Events;

namespace Base.Developer.Console
{
    [CreateAssetMenu(fileName = "CommandScriptableObject", menuName = "Development/Console/CommandScriptableObject")]
    public class CommandScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string CommandName { get; private set; } = "command";
        [field: SerializeField] public string CommandBack { get; private set; } = "Running";
        [field: SerializeField] public UnityEvent CommandEvent { get; private set; } 

    }
}