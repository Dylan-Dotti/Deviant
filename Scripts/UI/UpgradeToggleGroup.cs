using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(ToggleGroup))]
public class UpgradeToggleGroup : MonoBehaviour
{
    [System.Serializable]
    private struct UpgradeGroup
    {
        public Toggle toggleButton;
        public GameObject upgradesObject;
        public GameObject statsObject;
    }

    [SerializeField]
    private List<UpgradeGroup> upgradeGroups;
    [SerializeField]
    private List<AudioClip> activatedSounds;

    private ToggleGroup tGroup;
    private AudioSource source;

    private void Awake()
    {
        tGroup = GetComponent<ToggleGroup>();
        source = GetComponent<AudioSource>();
    }

    public void OnButtonToggled(bool toggled)
    {
        if (toggled)
        {
            AudioClip randomClip = activatedSounds[
                Random.Range(0, activatedSounds.Count - 1)];
            activatedSounds.Remove(randomClip);
            activatedSounds.Add(randomClip);
            source.PlayOneShot(randomClip);
        }
        foreach (UpgradeGroup group in upgradeGroups)
        {
            group.upgradesObject.SetActive(group.toggleButton.isOn);
            group.statsObject.SetActive(group.toggleButton.isOn);
        }
    }
}
