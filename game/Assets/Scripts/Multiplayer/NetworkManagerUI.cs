using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Relay.Models;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using UnityEngine.SceneManagement;

public class NetworkManagerUI : MonoBehaviour
{
    private const int MaxPlayers = 4;
    Allocation hostAllocation;
    JoinAllocation playerAllocation;
    bool isHost;

    [SerializeField]private Button hostButton;
    [SerializeField]private TMP_InputField codeInput;
    [SerializeField]private Button clientButton;
    
    
    string playerId = "Not signed in";
    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        hostButton.onClick.AddListener(Host);
        clientButton.onClick.AddListener(Join);
        DontDestroyOnLoad(gameObject);
    }

    private async void Host()
    {
        isHost = true;
        Debug.Log("Signing in...");
        await SignIn();
        Debug.Log("Allocating...");
        await Allocate();
        Debug.Log("Generating code...");
        var code = await GenerateCode();
        Debug.Log("Hosting, code: " + code);
        // change scene
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Multiplayer");
        StartCoroutine(Server(code));
        Debug.Log("Started coroutine");
    }

    private async void Join()
    {
        isHost = false;
        Debug.Log("Signing in...");
        await SignIn();
        var code = codeInput.text;
        Debug.Log("Joining, code: " + code);
        await Join(code);
        Debug.Log("Joined");
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Multiplayer");
        StartCoroutine(Server(code));
    }
    
    private IEnumerator Server(string code)
    {
        while (!SceneManager.GetActiveScene().name.Equals("Multiplayer"))
        {
            yield return null;
        }
        
        var serverRelayUtilityTask = isHost
            ? Task.FromResult(new RelayServerData(hostAllocation, "dtls"))
            : Task.FromResult(new RelayServerData(playerAllocation, "dtls"));
        
        while (!serverRelayUtilityTask.IsCompleted)
        {
            yield return null;
        }
        // Once the server started
        if (serverRelayUtilityTask.IsFaulted)
        {
            Debug.LogError("Exception thrown when attempting to start Relay Server. Server not started. Exception: " + serverRelayUtilityTask.Exception.Message);
            yield break;
        }

        var relayServerData = serverRelayUtilityTask.Result;

        
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        if (isHost) NetworkManager.Singleton.StartHost();
        else        NetworkManager.Singleton.StartClient();
        Debug.Log($"Server started, join with code: {code}");
        yield return null;
    }

    private async Task SignIn()
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerId = AuthenticationService.Instance.PlayerId;
        Debug.Log($"Signed in. Player ID: {playerId}");
    }
    
    private async Task Allocate()
    {
        Debug.Log("Host - Creating an allocation.");
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(MaxPlayers);
        hostAllocation = allocation;
        Debug.Log($"Host Allocation ID: {allocation.AllocationId}, region: {allocation.Region}");
    }

    private async Task<string> GenerateCode()
    {
        Debug.Log("Host - Getting a join code for my allocation. I would share that join code with the other players so they can join my session.");
        
        try
        {
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(hostAllocation.AllocationId);
            Debug.Log("Host - Got join code: " + joinCode);
            return joinCode;
        }
        catch (RelayServiceException ex)
        {
            Debug.LogError(ex.Message + "\n" + ex.StackTrace);
        }
        return "n/a";
    }
    
    private async Task Join(string joinCode)
    {
        Debug.Log("Player - Joining host allocation using join code.");

        try
        {
            playerAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            Debug.Log("Player Allocation ID: " + playerAllocation.AllocationId);
        }
        catch (RelayServiceException ex)
        {
            Debug.LogError(ex.Message + "\n" + ex.StackTrace);
        }
    }
}
