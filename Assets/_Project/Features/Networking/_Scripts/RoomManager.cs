using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

namespace Multiplatform
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        Transform spawnPosition;

        #region Unity
        private void Start()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                string prefab = "Prefabs/";
#if UNITY_WEBGL
                prefab += "WebPrefab";
#elif UNITY_STANDALONE_WIN
                prefab += "PCPrefab";
#endif
                PhotonNetwork.Instantiate(prefab, spawnPosition.position, spawnPosition.rotation);
            }
        }
#endregion

#region Photon

#endregion
    }
}