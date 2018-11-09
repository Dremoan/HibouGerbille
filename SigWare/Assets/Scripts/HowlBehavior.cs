using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HibouGerbille
{
    [System.Serializable]
    public class HowlBehavior : MonoBehaviour
    {

        public enum HowlActions {IdleDefault, ChangeIdle, Turn, Attack}
        public HowlActions howlActionsEnum;
        public AudioTrackHowl[] audioTracks;
        [SerializeField] private Animator animHowl;
        [SerializeField] private float randomWait;
        [SerializeField] private int countIdle = 0;

	    void Update ()
        {
            Behaviour();
        }

     void Behaviour()
        {
            switch (howlActionsEnum)
            {
            case HowlActions.IdleDefault:
                    WaitIdle();
                break;
            case HowlActions.ChangeIdle:
                    PickRandomIdle();
                break;
            case HowlActions.Turn:
                break;
            case HowlActions.Attack:
                break;
            default:
                break;
            }
        }

        void ChangeState(HowlActions nextAction)
        {
            howlActionsEnum = nextAction;
        }

        void WaitIdle()
        {
            StartCoroutine(WaitIdleCoroutine());
        }

        void PickRandomIdle()
        {
            countIdle = Random.Range(1, 4);
            animHowl.SetInteger("CountIdle", countIdle);
        }

        IEnumerator WaitIdleCoroutine()
        {
            randomWait = Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(randomWait);
            ChangeState(HowlActions.ChangeIdle);
        }




     }

    [System.Serializable]
    public class AudioTrackHowl : MonoBehaviour
    {
        public AudioClip[] audioClips;
        private float Bjr;
    }
}
