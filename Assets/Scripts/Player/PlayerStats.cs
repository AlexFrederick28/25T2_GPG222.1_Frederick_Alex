using System.Collections;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// Assigns the player with their unique index number, as well as sends a signal to the multiplayer lobby for joining and leaving. Sets the colour of the player pilot through the multiplayer lobby colours. Sets spawn position through PlayerPositioning.cs upon entering the game scene.
/// </summary>
public class PlayerStats : NetworkBehaviour
{
    public int playerIndex;
    [SerializeField] private MeshRenderer pilotMeshRenderer;
    [SerializeField] private MeshRenderer[] spaceshipObjects;
    
    private Material material;

    private bool isInSpawnPosition = false;

    [SerializeField] private GameObject changeNameCanvas;
    public NetworkVariable<FixedString32Bytes> nameText = new NetworkVariable<FixedString32Bytes>("");
    public string clientName;
    public TextMeshProUGUI displayNameText;
    public TextMeshProUGUI inputNameText;

    public NetworkVariable<bool> collidedWithAsteroid = new NetworkVariable<bool>();

    [SerializeField] private TextMeshPro playerNameTag;
    [SerializeField] private GameObject deadText;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        MultiplayerLobby.playerJoin.Invoke(gameObject);

        nameText.OnValueChanged += OnNameChanged_RPC;

        playerIndex = (int)OwnerClientId;
        SetPilotColour(MultiplayerLobby.instance.GetPlayerColour(playerIndex)); // setting the colour of the pilot using the player index in relation to the colour from the multiplayer lobby colour list

        MultiplayerLobby.instance.AddPlayerNameToLobby(gameObject);

        if (IsOwner)
        {
            changeNameCanvas.SetActive(true);
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        nameText.OnValueChanged -= OnNameChanged_RPC;

        MultiplayerLobby.playerLeave.Invoke(gameObject);
    }

    //[Rpc(SendTo.Server, Delivery = RpcDelivery.Reliable, RequireOwnership = false)] // Thsi was used for buttons to update player name -- Instead the names are updated with OnValueChanged event
    //public void SubmitNewNameServer_RPC()
    //{
    //    if (IsServer)
    //    {
    //        Debug.Log("Input name: " + inputNameText.text);
    //        nameText.Value = inputNameText.text;
    //        Debug.Log("New name: " + nameText.Value);
    //    }
    //}

    [Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable, RequireOwnership = false)]
    private void OnNameChanged_RPC(FixedString32Bytes oldName, FixedString32Bytes newName)
    {
        if (displayNameText != null)
        {
            displayNameText.text = newName.ToString();
            ChangeNameServer_RPC(displayNameText.text);
        }
    }

    [Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable, RequireOwnership = false)]
    public void ChangeNameServer_RPC(string newName)
    {
        if (IsServer)
        {
            nameText.Value = newName;
        }

        playerNameTag.text = newName;
        gameObject.name = newName;
    }

    [Rpc(SendTo.Server, Delivery = RpcDelivery.Reliable, RequireOwnership = true)]
    public void SendClientNameToServer_RPC(string name)
    {
        nameText.Value = name;
    }

    private void Awake()
    {
        material = new Material(pilotMeshRenderer.material);
        pilotMeshRenderer.material = material;
    }

    public void SetPilotColour(Color color)
    {
        material.color = color;
    }

    private bool updateName = false;

    public void UpdateNameInput()
    {
        if (IsOwner) // TODO: fix it so that it doesnt update per frame smh
        {
            if (clientName != inputNameText.text)
            {
                updateName = true;
                clientName = inputNameText.text;
            }
            else if (clientName == inputNameText.text && updateName == true)
            {
                Debug.Log("Updated Name");
                SendClientNameToServer_RPC(clientName);

                updateName = false;
            }
        }
    }

    private void Update()
    {
        SetPlayerSpawnPosition();

        UpdateNameInput();

        if (collidedWithAsteroid.Value == true && IsOwner)
        {
            deadText.SetActive(true);
        }
    }

    private void SetPlayerSpawnPosition()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Scene gameScene = SceneManager.GetSceneByBuildIndex(1);

        if (currentScene == gameScene && isInSpawnPosition == false)
        {
            PlayerPositioning.SetSpawnPositionEvent.Invoke(gameObject);

            isInSpawnPosition = true;
        }
    }

    [Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable, RequireOwnership = false)]
    public void PlayerDeath_RPC(int _playerIndex)
    {
        if (_playerIndex != playerIndex)
        {
            Debug.Log("Dead");

            foreach (MeshRenderer renderer in spaceshipObjects)
            {
                renderer.enabled = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");

        if (collision.gameObject.GetComponent<AsteroidStats>() != null)
        {
            AsteroidStats stats = collision.gameObject.GetComponent<AsteroidStats>();
            Debug.Log("Collided with Asteroid");

            if (collision.gameObject.GetComponent<AsteroidStats>().ownerID.Value != playerIndex)
            {
                collidedWithAsteroid.Value = true;
            }

            PlayerDeath_RPC(stats.ownerID.Value);
        }
    }
}
