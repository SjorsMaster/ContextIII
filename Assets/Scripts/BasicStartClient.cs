using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

namespace ContextIII
{
    public class BasicStartClient : MonoBehaviour
    {
        [SerializeField] private bool autoConnectOnStartup;
        [SerializeField] private NetworkManager networkManager;
        [SerializeField] string networkAddress;
        [SerializeField] private TMP_Text[] ipTextFields;

        private void Start()
        {
            string[] networkAddressSplit = networkAddress.Split('.');
            for (int i = 0; i < ipTextFields.Length; i++)
                ipTextFields[i].text = networkAddressSplit[i];

            if (autoConnectOnStartup)
                StartClient();
        }

        public void StartClient()
        {
            string ipAddress = "";
            for (int i = 0; i < ipTextFields.Length; i++)
            {
                ipAddress += ipTextFields[i].text;
                if (i < ipTextFields.Length - 1)
                    ipAddress += ".";
            }
            networkManager.networkAddress = ipAddress;

            networkManager.StartClient();
        }

        public void IncreaseIpValue(TMP_Text textField)
        {
            int currentIpValue = int.Parse(textField.text);
            currentIpValue++;
            textField.text = currentIpValue.ToString();
        }

        public void DecreaseIpValue(TMP_Text textField)
        {
            int currentIpValue = int.Parse(textField.text);
            currentIpValue--;
            textField.text = currentIpValue.ToString();
        }
    }
}
