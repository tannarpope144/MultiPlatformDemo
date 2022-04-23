using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplatform.Podcasts
{
    public class AudioUIController : MonoBehaviour
    {
        [SerializeField]
        AudioClipController clipController;

        [Space(10), SerializeField]
        Text title;

        [SerializeField]
        Button play, pause, stop;

        [SerializeField]
        CustomSliderControl playbackControl;

        bool playbackControlSelected;

        void Start()
        {
            if (clipController != null)
            {
                play?.onClick.AddListener(clipController.Play);
                pause?.onClick.AddListener(clipController.Pause);
                stop?.onClick.AddListener(clipController.Stop);
                playbackControl?.onValueChanged.AddListener(clipController.SetTime);

                clipController.onEnable.AddListener(SetButtonInteractable);
                clipController.onUpdate.AddListener(playbackControl.SetValue);
            }
        }

        void SetButtonInteractable(string _title)
        {
            play.interactable = true;
            pause.interactable = true;
            stop.interactable = true;

            playbackControl.SetInteractable(true);

            title.text = _title;
        }
    }
}