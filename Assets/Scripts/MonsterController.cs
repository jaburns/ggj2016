using System.Collections;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    // Music clip lengths
    // Puzzle1 37
    // Puzzle2 29
    // Puzzle3 38
    // Puzzle4 49
    // Puzzle5 63
    //
    Rigidbody2D _rb;
    bool _startedWriting;
    bool _done;
    Animator _anim;
    GameObject _whiteboard;
    GameObject _fence;

    void Start ()
    {
        GnomeSelector.gnomesEnabled = false;
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();

        SkyBoxController.Instance.OnDusk.Sub(gameObject, OnDusk);
        SkyBoxController.Instance.OnNightEnd.Sub(gameObject, OnNightEnd);

        _fence = GameObject.Find("Fence_Animated");
        _whiteboard = GameObject.Find("WhiteboardStuff");
        if (_whiteboard != null) _whiteboard.SetActive(false);

        SoundPlayer.Instance.Play("DayMusic");
    }

    void OnDusk()
    {
        _rb.isKinematic = false;
        SoundPlayer.Instance.Play("MonsterFallMusic");
    }

    void OnNightEnd()
    {
        if (_done) return;
        if (_whiteboard != null) _whiteboard.SetActive(false);
        SoundPlayer.Instance.Play("MonsterUnhappy");
        _done = true;
        StartCoroutine(angryRoutine());
    }

    public void GnomesWin()
    {
        if (_done) return;
        if (_whiteboard != null) _whiteboard.SetActive(false);
        SoundPlayer.Instance.Play("MonsterLaughing");
        _done = true;

        StartCoroutine(happyRoutine());
    }

    IEnumerator happyRoutine()
    {
        transform.localScale *= 0.5f;
        GnomeSelector.gnomesEnabled = false;
        MusicPlayer.Instance.Stop();
        SoundPlayer.Instance.Play("WinMusic");
        yield return new WaitForSeconds(5f);
        SceneLoader.NextLevel();
    }

    IEnumerator writeRoutine()
    {
        yield return new WaitForSeconds(3f);
        _anim.SetBool("Scribbling", false);
        SkyBoxController.Instance.StartNight(FindObjectOfType<LevelTime>().LevelLength);
        GnomeSelector.gnomesEnabled = true;
        if (_whiteboard != null) _whiteboard.SetActive(true);
        MusicPlayer.Instance.PlayTrack("Puzzle"+(SceneLoader.CurLevel+1), false);
        StartCoroutine(fenceRoutine());
    }

    IEnumerator angryRoutine()
    {
        MusicPlayer.Instance.Stop();
        SoundPlayer.Instance.Play("LoseMusic");
        transform.localScale *= 1.5f;
        GnomeSelector.gnomesEnabled = false;
        yield return new WaitForSeconds(5f);
        SceneLoader.ReloadLevel();
    }

    IEnumerator fenceRoutine()
    {
        while (true) {
            _fence.transform.position -= Vector3.up * 12f * Time.deltaTime;
            if (_fence.transform.position.y < -100) break;
            yield return new WaitForEndOfFrame();
        }
    }

    void OnCollisionEnter2D()
    {
        if (!_startedWriting) {
            _startedWriting = true;
            _anim.SetBool("Scribbling", true);
            SoundPlayer.Instance.Play("MonsterLanding");
            SoundPlayer.Instance.Play("MonsterScribble");
            SkyBoxController.Instance.ShowCracks();
            StartCoroutine(writeRoutine());
        }
    }
}
