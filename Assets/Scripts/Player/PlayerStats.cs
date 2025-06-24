using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private void OnEnable()
    {
        MultiplayerLobby.playerJoin.Invoke(gameObject);
    }
    private void OnDisable()
    {
        MultiplayerLobby.playerLeave.Invoke(gameObject);
    }
}
