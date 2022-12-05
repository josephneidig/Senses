using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSwitching : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] guns;

    [Header("Controls")]
    [SerializeField] private KeyCode[] keys;

    [Header("Settings")]
    [SerializeField] private float switchTime;

    private int selectedGun;
    private float timeSinceLastSwitch;

    private void Start()
    {
        SetGuns();
        Select(selectedGun);

        timeSinceLastSwitch = 0f;
    }

    private void SetGuns()
    {
        guns = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            guns[i] = transform.GetChild(i);
        }

        if (keys == null)
        {
            keys = new KeyCode[guns.Length];
        }
    }

    private void Update()
    {
        int previousSelectedGun = selectedGun;

        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]) && timeSinceLastSwitch >= switchTime)
            {
                selectedGun = i;
            }
        }

        if (previousSelectedGun != selectedGun)
        {
            Select(selectedGun);
        }

        timeSinceLastSwitch += Time.deltaTime;
    }

    private void Select(int gunIndex)
    {
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].gameObject.SetActive(i == gunIndex);
        }

        timeSinceLastSwitch = 0f;

        OnGunSelected();
    }

    private void OnGunSelected()
    {
        Debug.Log("Selected new gun.");
    }
}
