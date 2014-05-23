using UnityEngine;

[ExecuteInEditMode]
public class DisableFogForCamera : MonoBehaviour 
{
    private bool revertFogState = false;
	
    void OnPreRender() {
        revertFogState = RenderSettings.fog;
        RenderSettings.fog = false;
    }
    void OnPostRender() {
        RenderSettings.fog = revertFogState;
    }
}