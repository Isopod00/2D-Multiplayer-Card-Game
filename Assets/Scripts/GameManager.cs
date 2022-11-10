using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameObject[] clients;

    // Start is called before the first frame update
    void Start()
    {
        clients = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        clients = GameObject.FindGameObjectsWithTag("Player");
    }

    public static void changeTurns()
    {
        foreach (GameObject player in clients)
        {
            PlayerManager script = player.GetComponent<PlayerManager>();
            script.toggleMyTurn();
        }
    }
}