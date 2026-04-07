using UnityEngine;

public class OutlineSimple : MonoBehaviour
{
    public Material outlineMaterial;
    private Renderer[] renderers;
    private Material[][] originalMaterials;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }
    }

    public void EnableOutline()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] mats = new Material[renderers[i].materials.Length + 1];

            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                mats[j] = renderers[i].materials[j];
            }

            mats[mats.Length - 1] = outlineMaterial;
            renderers[i].materials = mats;
        }
    }

    public void DisableOutline()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = originalMaterials[i];
        }
    }
}