using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class EndOfChase : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector director = null;

    [SerializeField]
    private TimelineAsset crashLandingCutscene = null;
    [SerializeField]
    private TimelineAsset endChaseCutscene = null;

    [SerializeField]
    private UnityEvent onChaseEnd = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            director.playableAsset = endChaseCutscene;
            director.Play();
            onChaseEnd?.Invoke();
        }
    }
}
