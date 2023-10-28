using Riptide;
using Riptide.Utils;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private Client client = new();

    static public bool IsGyroActivate = true;

    static public Vector3 GyroAngle = new Vector3 (0,0,0);

    static public string UserAction = "null";

    // Codes for initial setups and sending data are from:
    // https://riptide.tomweiland.net/manual/overview/getting-started.html

    void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        client.Connect($"{LoadSceneManager.IP}:7777");
    }

    private void FixedUpdate()
    {
        client.Update();

        Message message1 = Message.Create(MessageSendMode.Unreliable, 1);
        message1.AddBool(IsGyroActivate);
        client.Send(message1);

        Message message2 = Message.Create(MessageSendMode.Unreliable, 2);
        message2.AddVector3(GyroAngle);
        client.Send(message2);

        Message message3 = Message.Create(MessageSendMode.Unreliable, 3);
        if (UserAction != null)
            message3.AddString(UserAction);
        client.Send(message3);
    }

    private void OnApplicationQuit()
    {
        client.Disconnect();
    }
}
