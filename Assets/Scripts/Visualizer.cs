
using GoogleARCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visualizer : MonoBehaviour
{
    /// <summary>
    /// The AugmentedImage to visualize.
    /// </summary>
    public AugmentedImage Image;

    public Text Text;
    public GameObject Figure;
    public Material mat1;
    public Material mat2;
    private bool isMat1 = true;
 
    /// <summary>
    /// The Unity Update method.
    /// </summary>
    public void Update()
    {   
      
        if (Image == null || Image.TrackingState != TrackingState.Tracking)
        {
            Figure.SetActive(false);
            return;
        }
        float halfWidth = Image.ExtentX / 2;
        float halfHeight = Image.ExtentZ / 2;
        Debug.Log("LEFT " + (halfWidth * Vector3.left) + (halfHeight * Vector3.back));
        Debug.Log("RIGHT" + (halfWidth * Vector3.right) + (halfHeight * Vector3.back));
        Debug.Log("MINUS" + ((halfWidth * Vector3.right) + (halfHeight * Vector3.back) - (halfWidth * Vector3.left) - (halfHeight * Vector3.back)));
        Debug.Log(Image.Name);
        Text.text = Image.Name;
        Figure.transform.localPosition = Vector3.zero;
        Figure.SetActive(true);
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {  
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {          
                if (raycastHit.collider.name == "Cube")
                {
                    Renderer render = raycastHit.transform.gameObject.GetComponent<Renderer>();
                    if(isMat1)
                    {
                       render.material.color = Color.blue;
                    } else
                    {
                        render.material.color = Color.red;
                    }
                    isMat1 = !isMat1;
                }
            }
        }
    }
}
