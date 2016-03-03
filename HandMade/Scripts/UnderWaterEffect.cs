using UnityEngine;
using System.Collections;

public class UnderWaterEffect : MonoBehaviour {

    public float waterLevel;
    private bool isUnderwater = false;
    private Color normalColor;
    private Color underwaterColor;

	void Start () {
        normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        underwaterColor = new Color(0.22f, 0.65f, 0.77f, 0.5f);
	}
	

	void Update () {
	if((transform.position.y < waterLevel != isUnderwater))
        {
            isUnderwater = transform.position.y < waterLevel;
            if(isUnderwater) SetUnderWater();
            if (!isUnderwater) SetNormal();
        }
	}
   public void SetNormal()
    {
        RenderSettings.fogColor = normalColor;
        RenderSettings.fogDensity = 0.002f;
    }
    public void SetUnderWater()
    {
        RenderSettings.fogColor = underwaterColor;
        RenderSettings.fogDensity = 0.01f;
    }
}
