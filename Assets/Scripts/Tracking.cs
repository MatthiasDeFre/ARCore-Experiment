using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for AugmentedImage example.
/// </summary>
public class Tracking : MonoBehaviour
{
	/// <summary>
	/// A prefab for visualizing an AugmentedImage.
	/// </summary>
	public Visualizer VisualizerPrefab;

	private Dictionary<int, Visualizer> m_Visualizers
	= new Dictionary<int, Visualizer>();

	private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();
    public Text Text;
    /// <summary>
    /// The Unity Update method.
    /// </summary>
    public void Update()
	{
		// Exit the app when the 'back' button is pressed.
		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}

		// Check that motion tracking is tracking.
		if (Session.Status != SessionStatus.Tracking)
		{
			return;
		}
    
		// Get updated augmented images for this frame.
		Session.GetTrackables<AugmentedImage>(
			m_TempAugmentedImages, TrackableQueryFilter.Updated);

		// Create visualizers and anchors for updated augmented images that are tracking and do
		// not previously have a visualizer. Remove visualizers for stopped images.
		foreach (var image in m_TempAugmentedImages)
		{
			Visualizer visualizer = null;
			m_Visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
			if (image.TrackingState == TrackingState.Tracking && visualizer == null)
			{
				// Create an anchor to ensure that ARCore keeps tracking this augmented image.
				Anchor anchor = image.CreateAnchor(image.CenterPose);
				visualizer = (Visualizer)Instantiate(
                    VisualizerPrefab, anchor.transform);
				visualizer.Image = image;
                visualizer.Text = Text;
				m_Visualizers.Add(image.DatabaseIndex, visualizer);
			}
			else if (image.TrackingState == TrackingState.Stopped && visualizer != null)
			{
				m_Visualizers.Remove(image.DatabaseIndex);
				GameObject.Destroy(visualizer.gameObject);
			}
		}
	}
}