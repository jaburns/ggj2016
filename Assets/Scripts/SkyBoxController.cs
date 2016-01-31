using UnityEngine;

public class SkyBoxController : Singleton<SkyBoxController>
{
    public SpriteRenderer Sun;
    public SpriteRenderer Moon;
    public SpriteRenderer NightMountains;
    public SpriteRenderer SunMountains;
    public SpriteRenderer NightSky;
    public SpriteRenderer SunSky;
    public SpriteRenderer NightGround;
    public SpriteRenderer SunGround;

    public Vector2 SkySphereStart;
    public Vector2 SkySphereEnd;

    public float DayTimeStart = .6f;
    public float DayTimeScale = 1f;
    public float DuskLength = .1f;
    public float NightTimeScale = 1f;

    public GameEvent OnDusk;
    public GameEvent OnNightEnd;

    bool _skyIsNight;
    float _skyStateT;
    bool _paused;

    static Color vis(float t)
    {
        return new Color(1f, 1f, 1f, t);
    }

    void Awake()
    {
        _skyIsNight = false;
        _paused = false;
        _skyStateT = DayTimeStart;

        OnDusk = new GameEvent();
        OnNightEnd = new GameEvent();
    }

    void Update()
    {
        if (_paused) return;

        var scale = _skyIsNight ? NightTimeScale : DayTimeScale;
        _skyStateT += Time.deltaTime * scale;

        if (_skyStateT >= 1f) {
            _skyStateT = 1f;
        }

        updateGraphics();

        if (_skyStateT >= 1f) {
            if (_skyIsNight) {
                Debug.Log("End of night");
                OnNightEnd.Broadcast();
            } else {
                OnDusk.Broadcast();
            }
            _paused = true;
            _skyStateT = 0f;
            _skyIsNight = true;
        }
    }

    public void StartNight()
    {
        _paused = false;
    }

    void updateGraphics()
    {
        if (!_skyIsNight && _skyStateT > 1f - DuskLength) {
            var duskT = (_skyStateT - (1f - DuskLength)) / DuskLength;
            SunMountains.material.color =
                SunGround.material.color =
                SunSky.material.color =
                    vis(1 - duskT);
        }

        var pos = Vector2.Lerp(SkySphereStart, SkySphereEnd, _skyStateT);
        Sun.transform.position = pos.AsVector3(Sun.transform.position.z);
        Moon.transform.position = pos.AsVector3(Moon.transform.position.z);

        Moon.material.color = vis(_skyIsNight ? 1 : 0);
        Sun .material.color = vis(_skyIsNight ? 0 : 1);
    }
}
