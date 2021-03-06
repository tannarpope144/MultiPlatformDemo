using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
//using Photon.Pun.UtilityScipts;

namespace Multiplatform
{
        public class PlayerList : MonoBehaviour
        {
            [Header("UI References")]
            public Text PlayerNameText;
            public Image PlayerColorImage;
            public Button PlayerReadyButton;
            public Image PlayerReadyImage;

            private int ownerId;
            private bool isPlayerReady;

            #region UNITY

            public void OnEnable()
            {
                //PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
            }

            public void Start()
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
                {
                    PlayerReadyButton.gameObject.SetActive(false);
                }
                else
                {
                    Hashtable initialProps = new Hashtable() { { "IsPlayerReady", isPlayerReady }, { "PlayerLives", 8 } };
                    PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);

                    PlayerReadyButton.onClick.AddListener(() =>
                    {
                        isPlayerReady = !isPlayerReady;
                        SetPlayerReady(isPlayerReady);

                        Hashtable props = new Hashtable() { { "IsPlayerReady", isPlayerReady } };
                        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                        if (PhotonNetwork.IsMasterClient)
                        {
                            FindObjectOfType<PUNManager>().LocalPlayerPropertiesUpdated();
                        }
                    });
                }
            }

            public void OnDisable()
            {
                //PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
            }

            #endregion

            public void Initialize(int playerId, string playerName)
            {
                ownerId = playerId;
                PlayerNameText.text = playerName;
            }

            private void OnPlayerNumberingChanged()
            {
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    if (p.ActorNumber == ownerId)
                    {
                        //PlayerColorImage.color = AsteroidsGame.GetColor(p.GetPlayerNumber());
                    }
                }
            }

            public void SetPlayerReady(bool playerReady)
            {
                PlayerReadyButton.GetComponentInChildren<Text>().text = playerReady ? "Ready!" : "Ready?";
                PlayerReadyImage.enabled = playerReady;
            }
        }
}