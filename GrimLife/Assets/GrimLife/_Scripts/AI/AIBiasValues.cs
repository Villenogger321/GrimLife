using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class AIBiasValues : MonoBehaviour
{
    [Header("BaseValue")]
    [SerializeField] int baseBiasValue;
    [Header("HpBias")]
    [SerializeField] int hpBiasValue;
    [SerializeField] AnimationCurve hpBiasCurve;
    [Header("DistanceBias")]
    [SerializeField] int distanceBiasValue;
    [SerializeField] Vector2 distanceBiasRange;
    [SerializeField] AnimationCurve distanceBiasCurve;
    [Header("AgressionBias")]
    /// <summary>
    /// time since last hit, the shorter the time the higher the bias
    /// </summary>
    [SerializeField] int aggressionBiasValue;
    [SerializeField] Vector2 aggressionBiasRange;
    [SerializeField] AnimationCurve aggressionBiasCurve;
    [SerializeField] float attackTimeStamp; // set to time.time when attacked

    Health health;

    void Awake()
    {
        health = GetComponent<Health>();
    }
    
    public int CalculateBias()
    {
        int bias = baseBiasValue;

        bias += (int)(hpBiasValue * hpBiasCurve.Evaluate(health.GetCurrentHealth() / health.GetMaxHealth()));

        float distance = Vector3.Distance(transform.position, PlayerStats.Player.transform.position);
        bias += CalculateBiasFromCurve(distance, distanceBiasCurve, distanceBiasRange, distanceBiasValue);
        
        bias += CalculateBiasFromCurve(Time.time - attackTimeStamp, aggressionBiasCurve, aggressionBiasRange, aggressionBiasValue);

        return bias;
    }

    int CalculateBiasFromCurve(float _input, AnimationCurve _curve, Vector2 _range, int _biasValue)
    {
        _input.Clamp(_range.x, _range.y);

        _input -= _range.x;
        _input /= _range.y - _range.x;

        return (int)(_biasValue * _curve.Evaluate(_input));
    }
    public void SetAttackTimestamp(float _time)
    {
        attackTimeStamp = _time;
    }
}
public static class Helper
{
    public static float Clamp(this ref float _clampee, float _min, float _max)
    {
        _clampee = Mathf.Clamp(_clampee, _min, _max);
        return _clampee;
    }
}