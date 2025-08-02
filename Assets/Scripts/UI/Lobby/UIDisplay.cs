using UnityEngine;

public class UIDisplay : MonoBehaviour
{
    public static UIDisplay Instance { get; private set; }

    private void OnEnable()
    {
        Instance = this;
    }

    public GameObject lobbySearchUI;
    public GameObject inLobbyUI;
}
