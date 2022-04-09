using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

namespace Multiplatform
{
    public class PlayerCountController : MonoBehaviour
    {
        [SerializeField]
        TMP_Text text;

        string startingText;

        #region Unity
        void Start()
        {
            startingText = text.text;
        }
        void SetPlayerCount(int _count)
        {
            text.text = startingText + _count;
        }
        private void FixedUpdate()
        {
            SetPlayerCount(PhotonNetwork.CountOfPlayers);
        }
        #endregion
    }

}