using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaybackController : MonoBehaviour
{
    private GameObject toggleButtonObj;
    private UIButton playButton;

    private GameObject playhead;

    private float note0PosY = -80;
    private float gap = 40;

    private GameObject notePrefab;

    private List<float> timestamp = new List<float>();
    private List<GameObject> notes = new List<GameObject>();

    private GameObject RecordedNotes;
    private float lastTimestamp = 0;
    private float startTime = -1;

    private SoundManager soundManger;
    private bool isInPlayback = false;
    private bool isInTimeLimit = true;

    void Start()
    {
        toggleButtonObj = GameObject.Find("PlayStop");
        playButton = toggleButtonObj.GetComponent<UIButton>();

        playhead = GameObject.Find("Playhead");
        RecordedNotes = GameObject.Find("Recorded Notes");

        notePrefab = Resources.Load("NotePrefab") as GameObject;
    
        soundManger = GetComponent<SoundManager>();
    }

    void Update()
    {
        if (startTime != -1)
        {
            if ((Time.time - startTime) > 5f)
                isInTimeLimit = false;
        }
    }


    public void RecordNotes(GameObject note)
    {
        if (timestamp.Count == 0 && notes.Count == 0)
        {
            StartPlayhead();

            lastTimestamp = Time.time;
            startTime = Time.time;

            timestamp.Add(0);
            notes.Add(note);
            SpawnNote(note.name);
        } else if (isInTimeLimit)
        {

            timestamp.Add(Time.time - lastTimestamp);
            lastTimestamp = Time.time;

            notes.Add(note);
            SpawnNote(note.name);
        }
    }


    void SpawnNote(string note)
    {
        float YPos = note0PosY + gap * int.Parse(note);
        float XPos = playhead.transform.localPosition.x;

        GameObject noteObj = Instantiate(notePrefab) as GameObject;
        noteObj.transform.parent = RecordedNotes.transform;
        noteObj.transform.localPosition = new Vector3(XPos, YPos, 0);
        noteObj.transform.localScale = Vector3.one;
        noteObj.GetComponent<UISprite>().spriteName = "Xylo_Note0" + (int.Parse(note) + 1).ToString();
    }


    public void Reset()
    {
        StopAllCoroutines();

        startTime = -1;
        lastTimestamp = 0;
        timestamp.Clear();
        notes.Clear();
        //playhead.GetComponent<Animator>().SetBool("StartPlayhead", false);
        playhead.GetComponent<Animator>().Play(Animator.StringToHash("Idle"));
        isInTimeLimit = true;

        playButton.normalSprite = "Xylo_Play";
        playButton.pressedSprite = "Xylo_Play_active";

        GameObject[] noteObjs = GameObject.FindGameObjectsWithTag("Note");
        foreach (GameObject noteObj in noteObjs)
        {
            Destroy(noteObj);
        }

    }

    public void PlayNotes()
    {
        if (isInPlayback != true)
        {
            isInPlayback = true;

            TogglePlayButton();

            StartPlayhead();
            StartCoroutine(ReadNotesSequence());
        }
    }

    IEnumerator ReadNotesSequence()
    {
        for (int i=0; i<timestamp.Count; i++)
        {
            yield return new WaitForSeconds(timestamp [i]);
            soundManger.PlayNote(notes [i]);
        }
        isInPlayback = false;
        TogglePlayButton();
    }


    void TogglePlayButton()
    {
        if (isInPlayback)
        {
            playButton.normalSprite = "Xylo_Stop";
            playButton.pressedSprite = "Xylo_Stop_active";
        } else
        {
            playButton.normalSprite = "Xylo_Play";
            playButton.pressedSprite = "Xylo_Play_active";
        }

    }


    void StartPlayhead()
    {
        //only play the animation when it's idle
        if (playhead.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash("Base.Idle"))
        {
            playhead.GetComponent<Animator>().SetTrigger("StartPlayhead");
            //StartCoroutine(StopPlayhead());
        }
    }

    IEnumerator StopPlayhead()
    {
        while (isInTimeLimit || isInPlayback)
        {
            yield return null;
        }
        playhead.GetComponent<Animator>().SetBool("StartPlayhead", false);
    }

}











