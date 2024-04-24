using System.Text.RegularExpressions;
using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField _tmpIpInput, _tmpPortInput;
    [SerializeField] private Button _tmpConnectBtn, _tmpCloseBtn;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }


    private bool SetUpNetworkPassport()
    {
        var ip = _tmpIpInput.text;
        var port = _tmpPortInput.text;

        var portRegex = new Regex(@"[0-9]{3,5}");
        var ipRegex = new Regex(@"^[0-9\.]+$");

        var portMatch = portRegex.Match(port);
        var ipMatch = ipRegex.Match(ip);

        if (!portMatch.Success || !ipMatch.Success)
        {
            Debug.LogError("올바르지 못한 아이피 또는 포트번호입니다.");
            return false;
        }

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            ip,
            (ushort)int.Parse(port)
        );
        return true;
    }

}
