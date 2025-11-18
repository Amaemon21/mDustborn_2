using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public static class WeaponUtilities
{
    public static AudioClip GetRandomAudioClip(List<AudioClip> audioClips)
    {
        int index = Random.Range(0, audioClips.Count - 1);
        return audioClips[index];
    }
    
    public static Vector3 GetDirection(Vector3 defaultDirection, bool applySpread, Vector3 spreadVariance)
    {
        if (applySpread)
        {
            var x = Random.Range(-spreadVariance.x, spreadVariance.x);
            var y = Random.Range(-spreadVariance.y, spreadVariance.y);
            var z = Random.Range(-spreadVariance.z, spreadVariance.z);

            defaultDirection += new Vector3(x, y, z);

            defaultDirection.Normalize();
        }

        return defaultDirection;
    }

    public static TrailRenderer CreateTrail(TrailRenderer bulletTrail, Transform barrelTransform, AnimationCurve widthCurve, float duration, float minVertexDistance, Gradient trailColor, Material material)
    {
        Vector3 spawnPosition = barrelTransform.position + barrelTransform.forward * 2;
        var trail = GameObject.Instantiate(bulletTrail, spawnPosition, Quaternion.identity);

        trail.widthCurve = widthCurve;
        trail.time = duration;
        trail.minVertexDistance = minVertexDistance;
        trail.colorGradient = trailColor;
        trail.material = material;

        return trail;
    }
    
    public static void CreateMuzzleFlash(bool enableMuzzle, GameObject[] muzzlePrefabs, Transform barrelTransform, float scaleFactor, float destroyTime)
    {
        if (enableMuzzle)
        {
            GameObject currentMuzzle = muzzlePrefabs[Random.Range(0, muzzlePrefabs.Length)]; ;
            GameObject muzzleFlash = Object.Instantiate(currentMuzzle, barrelTransform.position, barrelTransform.rotation, barrelTransform);

            muzzleFlash.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            Object.Destroy(muzzleFlash.gameObject, destroyTime);
        }
    }

    public static IEnumerator CreateMag(int currentAmmo, float magSpawnDelay, float magEmptySpawnDelay, float magDropForce, GameObject magPrefab, GameObject magEmptyPrefab, Transform magTransform)
    {
        if (currentAmmo > 0)
        {
            yield return new WaitForSeconds(magSpawnDelay);

            var mag = Object.Instantiate(magPrefab, magTransform.position, magTransform.rotation);
            var magRigidbody = mag.GetComponent<Rigidbody>();
            magRigidbody.AddForce(Vector3.forward * magDropForce);
            magRigidbody.AddTorque(Vector3.forward * magDropForce);

            Object.Destroy(mag.gameObject, 10.0f);
        }
        else if (currentAmmo == 0)
        {
            yield return new WaitForSeconds(magEmptySpawnDelay);

            var mag = Object.Instantiate(magEmptyPrefab, magTransform.position, magTransform.rotation);
            var magRigidbody = mag.GetComponent<Rigidbody>();
            magRigidbody.AddForce(Vector3.forward * magDropForce);
            magRigidbody.AddTorque(Vector3.forward * magDropForce);

            Object.Destroy(mag.gameObject, 10.0f);
        }
    }
}