using Riptide;
using Riptide.Utils;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : MonoBehaviour
{
    private Server server = new();

    [SerializeField]
    private TMP_Text serverState;
    [SerializeField]
    private TMP_Text gyroInfo;
    [SerializeField]
    private TMP_Text userAction;

    static public bool IsGyroActivate;

    static public Vector3 GyroAngle;

    static public string UserAction;

    // Codes for initial setup and sending data are from:
    // https://riptide.tomweiland.net/manual/overview/getting-started.html

    void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        server.Start(7777, 10);
    }

    private void FixedUpdate()
    {
        server.Update();

        if (Server.ClientID != 0)
        {
            serverState.text = $"Client {Server.ClientID} connected";
            if (!IsGyroActivate)
                gyroInfo.text = "Inactive";
            else
                gyroInfo.text = GyroAngle.y.ToString();
            userAction.text = UserAction;
        }

        else if (Server.ClientID == 0)
        {
            serverState.text = $"Disconnected";
            gyroInfo.text = "Inactive";
            userAction.text = null;
        }
    }

    private void OnApplicationQuit()
    {
        server.Stop();
    }

    [MessageHandler(1)]
    private static void HandleMessage1FromServer(ushort i, Message message)
    {
        IsGyroActivate = message.GetBool();
    }

    [MessageHandler(2)]
    private static void HandleMessage2FromServer(ushort i, Message message)
    {
        GyroAngle = message.GetVector3();
    }

    [MessageHandler(3)]
    private static void HandleMessage3FromServer(ushort i, Message message)
    {
        UserAction = message.GetString();
    }
}
