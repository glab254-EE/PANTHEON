using UnityEngine;

public static class LayerMaskUtils
{
    public static bool CompareCollisionlayers(int colliderLayerValue, LayerMask toCheck)
    {
        int colliderBit = 1<<colliderLayerValue;
        return (colliderBit&toCheck.value)>0;
    }
    public static bool CompareLayers(LayerMask layer1, LayerMask layer2)
    {
        int bitLayer1 = 1<<layer1;
        int bitLayer2 = 1<<layer2;
        return (bitLayer1&bitLayer2)>0;
    }
}
