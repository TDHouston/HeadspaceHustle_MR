using System.Collections.Generic;
using UnityEngine;
using Meta.XR.BuildingBlocks;

public class AnchorManager : MonoBehaviour
{
    public SpatialAnchorCoreBuildingBlock anchorCore;
    public GameObject anchorPrefab; // usually the object you're anchoring (e.g. Memory Zone)

    void Start()
    {
        // Check if anchor already exists
        if (anchorCore != null && anchorPrefab != null)
        {
            // Load existing anchors (if saved)
            anchorCore.OnAnchorsLoadCompleted.AddListener(OnAnchorsLoaded);

            // Try loading any saved anchors
            List<System.Guid> savedUuids = new(); // leave empty to load all anchors
            anchorCore.LoadAndInstantiateAnchors(anchorPrefab, savedUuids);
        }
        else
        {
            Debug.LogError("AnchorLoader is missing references.");
        }
    }

    void OnAnchorsLoaded(List<OVRSpatialAnchor> anchors)
    {
        if (anchors.Count == 0)
        {
            // If nothing was loaded, create a new anchor at current position
            Debug.Log("No anchor found. Creating a new one.");
            anchorCore.InstantiateSpatialAnchor(anchorPrefab, anchorPrefab.transform.position, anchorPrefab.transform.rotation);
        }
        else
        {
            Debug.Log("Anchors loaded and instantiated.");
        }
    }
}