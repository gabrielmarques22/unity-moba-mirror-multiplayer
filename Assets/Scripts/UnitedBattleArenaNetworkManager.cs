using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitedBattleArenaNetworkManager : NetworkManager
{

    public GameObject jonas, goku;

    private GameObject selectedHero;

    public GameObject UI;

    public void SetPlayer(string heroName)
    {
        PlayerPrefs.SetString("hero", heroName);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<PlayerSelection>(OnCreateCharacter);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        // you can send the message here, or wherever else you want
        PlayerSelection characterMessage = new PlayerSelection
        {
            heroName = PlayerPrefs.GetString("hero")
        };
        UI.SetActive(false);
        conn.Send(characterMessage);
    }

    void OnCreateCharacter(NetworkConnection conn, PlayerSelection message)
    {
        
        // playerPrefab is the one assigned in the inspector in Network
        // Manager but you can use different prefabs per race for example
        GameObject gameobject = Instantiate(ConnectWithSelectedCharacter(message.heroName));

        // call this to use this gameobject as the primary controller
        NetworkServer.AddPlayerForConnection(conn, gameobject);

        

    }

    // @TODO resolver bug de hero entrando e sobrepondo a UI, também não está tirando a UI de seleção de personagem

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        UI.SetActive(true);

        base.OnClientDisconnect(conn);

    }

    public GameObject ConnectWithSelectedCharacter(string heroName)
    {
        if (heroName.Equals("Goku"))
        {
            selectedHero = goku;

        } else if (heroName.Equals("Jonas"))
        {
            selectedHero = jonas;
        }

        return selectedHero;
    }

}
