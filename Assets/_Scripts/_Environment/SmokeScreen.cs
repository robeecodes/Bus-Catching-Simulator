using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SmokeScreen : MonoBehaviour
{
    [SerializeField] private Volume volume;
    
    private DepthOfField _dof;
    private ColorAdjustments _colorAdjustments;
    
    private void Start()
    {
        if (volume.profile.TryGet<DepthOfField>(out DepthOfField tmp))
        {
            _dof = tmp;
        }
        
        if (volume.profile.TryGet<ColorAdjustments>(out ColorAdjustments cA))
        {
            _colorAdjustments = cA;
        }
    }
    
    public void SmokeUp()
    {
        GameManager.Instance.isScreenSmoke = true;
        _dof.active = true;
        _colorAdjustments.active = true;
    }
    
    public void SmokeDown()
    {
        GameManager.Instance.isScreenSmoke = false;   
        _dof.active = false;
        _colorAdjustments.active = false;
    }
}
