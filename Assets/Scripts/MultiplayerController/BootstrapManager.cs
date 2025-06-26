using Unity.Netcode;
using UnityEngine;

public class BootstrapManager : MonoBehaviour
{
    [SerializeField]
    private float buttonWidth = 70f;

    [SerializeField]
    private float buttonHeight = 40f;

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 100, 400));

        var networkManager = NetworkManager.Singleton;
        if (!networkManager.IsClient && !networkManager.IsServer)
        {
            GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
            myButtonStyle.fontSize = 20;
            myButtonStyle.fixedWidth = buttonWidth;
            myButtonStyle.fixedHeight = buttonHeight;

            if (GUILayout.Button("Host", myButtonStyle))
            {
                networkManager.StartHost();
            }

            if (GUILayout.Button("Client", myButtonStyle))
            {
                networkManager.StartClient();
            }

            if (GUILayout.Button("Server", myButtonStyle))
            {
                networkManager.StartServer();
            }
        }

        GUILayout.EndArea();
    }
}

