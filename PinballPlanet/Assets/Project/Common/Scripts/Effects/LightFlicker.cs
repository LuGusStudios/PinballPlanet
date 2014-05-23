using UnityEngine;

public class LightFlicker : MonoBehaviour {
    public float duration = 1.0F;
	public float strength = 5.0F;
	public float normal = 0.75F;
	public float amplitude = 0.25F;
    void Update() {
		float phi2 = Time.time / duration * 2 * Mathf.PI;
		float amplitude2 = Mathf.Cos(phi2) * 0.25F + 0.75F;
        float phi = Time.time / duration * 5 * Mathf.PI;
        float amplitude3 = Mathf.Cos(phi) * 0.25F + 0.75F;
        light.intensity = amplitude3*amplitude2*strength;
    }
}