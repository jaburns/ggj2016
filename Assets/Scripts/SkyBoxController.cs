using UnityEngine;

public class SkyBoxController : Singleton<SkyBoxController>
{
    public SpriteRenderer Sun;
    public SpriteRenderer Moon;
    public SpriteRenderer NightMountains;
    public SpriteRenderer SunMountains;
    public SpriteRenderer NightSky;
    public SpriteRenderer SunSky;

    public Vector2 SkySphereStart;
    public Vector2 SkySphereEnd;

    public float DayTimeStart = .6f;
    public float DayTimeScale = 1f;
    public float DuskLength = .1f;
    public float NightTimeScale = 1f;

    bool _skyIsNight;
    float _skyStateT;

    static Color vis(float t)
    {
        return new Color(1f, 1f, 1f, t);
    }

    void Awake()
    {
        _skyIsNight = false;
        _skyStateT = DayTimeStart;
    }

    void Update()
    {
        var scale = _skyIsNight ? NightTimeScale : DayTimeScale;
        _skyStateT += Time.deltaTime * scale;

        if (_skyStateT >= 1f) {
            _skyStateT = 1f;
        }

        updateGraphics();

        if (_skyStateT >= 1f) {
            _skyStateT = 0f;
            _skyIsNight = true;
        }

    }

    void updateGraphics()
    {
        if (!_skyIsNight && _skyStateT > 1f - DuskLength) {
            var duskT = (_skyStateT - (1f - DuskLength)) / DuskLength;
            SunMountains.material.color = SunSky.material.color = vis(1 - duskT);
        }

        var pos = Vector2.Lerp(SkySphereStart, SkySphereEnd, _skyStateT);
        Sun.transform.position = pos.AsVector3(Sun.transform.position.z);
        Moon.transform.position = pos.AsVector3(Moon.transform.position.z);

        Moon.material.color = vis(_skyIsNight ? 1 : 0);
        Sun .material.color = vis(_skyIsNight ? 0 : 1);
    }
}
