using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private Slider RenderDistanceSlider;
    [SerializeField]
    private Text RenderDistanceText;
    [SerializeField]
    private endless_generator endless_Generator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settings.activeInHierarchy)
                settings.SetActive(false);
            else
                settings.SetActive(true);
        }
    }
    private void Awake()
    {
        RenderDistanceSlider.value = endless_Generator.distanza_vista;
        RenderDistanceText.text = RenderDistanceSlider.value.ToString("0000");
    }
    public void SetRenderDistance()
    {
        RenderDistanceText.text = RenderDistanceSlider.value.ToString("0000");
        endless_Generator.distanza_vista = RenderDistanceSlider.value;
    }
    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

}
