using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public static class Utility
{

    public static float CenterAngle(float _angle)
    {
        float start = _angle * 0.5f - 180;
        return _angle + Mathf.FloorToInt((_angle - start) / 360) * 360;
    }
    
    
    public static bool IsAngleInBounds(float _angle, float _min, float _max) {
        float start = (_min + _max) * 0.5f - 180;
        float floor = Mathf.FloorToInt((_angle - start) / 360) * 360;
        return _angle > _min + floor && _angle < _max + floor;
    }
    
    public static float ClampAngle(float _angle, float _min, float _max) {
        float start = (_min + _max) * 0.5f - 180;
        float floor = Mathf.FloorToInt((_angle - start) / 360) * 360;
        return Mathf.Clamp(_angle, _min + floor, _max + floor);
    }

}