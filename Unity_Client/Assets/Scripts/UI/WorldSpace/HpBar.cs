using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : UI_Base
{
    private enum Sliders
    {
        Slider,
    }

    public override void Init()
    {
        Bind<Slider>(typeof(Sliders));
    }

    private void FixedUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }

    public void SetHpBarRatio(float ratio)
    {
        ratio = Mathf.Clamp(ratio, 0, 1);
        GetSlider((int)Sliders.Slider).value = ratio;
    }
}
