using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] NotesAudioClip;

    /// <summary>
    /// Play note according to the id
    /// </summary>
    /// <param name="num">Number.</param>
    public void PlayNote(GameObject id)
    {
        AudioSource.PlayClipAtPoint(NotesAudioClip [int.Parse(id.name)], Vector3.zero);
    }

}
