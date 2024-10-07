using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetMovement : MonoBehaviour
{
    public GameObject Sun;
    public Vector3 axis;
    public float angle;
    public Texture2D defaultTextureMouse;

    private void Update()
    {
        transform.RotateAround(Sun.transform.position, axis, angle);
        transform.Rotate(0, 100 * Time.deltaTime, 0);
    }

    private void OnMouseDown()
    {
        switch(transform.gameObject.tag)
        {
            case "Planet_1":
                {
                    PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                    SceneManager.LoadScene("DesertPlanet");
                    break;
                }
            case "Planet_2":
                {
                    if (SaveGameDesert.GetCurrentState() == SaveGameDesert.GameTransitionState.PaidToAlien)
                    {
                        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
                        SceneManager.LoadScene("GreenPlanet");
                    }
                    break;
                }
            case "Planet_3":
                {
                    break;
                }
        }
    }
}
