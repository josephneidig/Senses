using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform cam;
    [SerializeField] private AudioClip shotSFX;
    [SerializeField] private AudioClip reloadSFX;
    [SerializeField] private AudioClip clickSFX;

    LineRenderer tracerRenderer;
    float timeSinceLastShot;

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;

        tracerRenderer = gameObject.AddComponent<LineRenderer>();
        Vector3[] initTracerPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        tracerRenderer.SetPositions(initTracerPositions);
        tracerRenderer.startWidth = 0.1f;
        tracerRenderer.endWidth = 0.1f;
        tracerRenderer.enabled = true;
        Color white = Color.white;
        tracerRenderer.SetColors(white, white);
        Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
        tracerRenderer.material = whiteDiffuseMat;
    }

    private void OnDisable()
    {
        gunData.reloading = false;
    }

    public void StartReload()
    {
        if (!gunData.reloading && this.gameObject.activeSelf)
        {
            AudioManager.audioManager.PlaySound(reloadSFX);
            //reloadSFX.Play();
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    public void Shoot()
    {
        if (gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                DrawTracer(cam.position, transform.forward);

                if (Physics.Raycast(cam.position, transform.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(gunData.damage);
                }

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunshot();
            }
        }
        else
        {
            if (CanShoot())
            {
                AudioManager.audioManager.PlaySound(clickSFX);
                timeSinceLastShot = 0;
            }
            //clickSFX.Play();
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (tracerRenderer.enabled && CanShoot())
        {
            tracerRenderer.enabled = false;
        }
    }

    private void DrawTracer(Vector3 startPosition, Vector3 direction)
    {
        tracerRenderer.enabled = true;
        Ray ray = new Ray(startPosition, direction);
        RaycastHit raycastHit;
        Vector3 endPosition = startPosition + (gunData.maxDistance * direction);

        if (Physics.Raycast(ray, out raycastHit, gunData.maxDistance))
        {
            endPosition = raycastHit.point;
        }

        tracerRenderer.SetPosition(0, startPosition);
        tracerRenderer.SetPosition(1, endPosition);
    }

    private void OnGunshot()
    {
        AudioManager.audioManager.PlaySound(shotSFX);
        //shotSFX.Play();
    }
}