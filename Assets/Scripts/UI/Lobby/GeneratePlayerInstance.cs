using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class GeneratePlayerInstance : MonoBehaviour
{
    public TextMeshProUGUI playerNameUI;

    public GameObject namePlateOwner;

    private void Update()
    {
        // temp solution
        playerNameUI.text = namePlateOwner.name;
    }
}
