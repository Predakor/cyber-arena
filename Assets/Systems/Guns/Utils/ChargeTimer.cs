using UnityEngine;

public class ChargeTimer
{
    private float _chargeStartTime;
    private float _chargeEndTime;
    private readonly float _maxChargeTime;

    private static float Now => Time.time;
    public ChargeState State { get; private set; } = ChargeState.None;

    public ChargeTimer(float maxChargeTime)
    {
        _maxChargeTime = maxChargeTime;
    }

    public ChargeTimer Start()
    {
        _chargeStartTime = Now;
        State = ChargeState.Charging;
        return this;
    }

    public ChargeTimer Stop()
    {
        _chargeEndTime = Now;
        State = ChargeState.Paused;
        return this;
    }

    public ChargeTimer Reset()
    {
        _chargeEndTime = default;
        _chargeStartTime = default;
        State = ChargeState.None;
        return this;
    }

    public float GetDuration()
    {
        return _chargeEndTime != 0
            ? _chargeEndTime - _chargeStartTime
            : Now - _chargeStartTime;
    }

    public float GetMaxChargePercentile()
    {
        if (_maxChargeTime <= 0f)
        {
            return 1f;
        }
        return Mathf.Clamp01(GetDuration() / _maxChargeTime);
    }
}
public enum ChargeState
{
    None,
    Charging,
    Paused, //add actual support
    Full,
}