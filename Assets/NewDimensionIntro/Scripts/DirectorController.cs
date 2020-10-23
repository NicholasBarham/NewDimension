using UnityEngine;
using UnityEngine.Playables;

public class DirectorController : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector director = null;

    private void Awake()
    {
        if (director == null)
            director = GetComponent<PlayableDirector>();
    }
}
