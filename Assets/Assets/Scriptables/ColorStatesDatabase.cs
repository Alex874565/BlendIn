using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorStatesDatabase", menuName = "ScriptableObjects/ColorStatesDatabase", order = 1)]
public class ColorStatesDatabase : ScriptableObject
{
    [SerializeField] private ColorClass[] colors;

    [Serializable]
    public class ColorClass
    {
        [SerializeField] private ColorType color;
        [SerializeField] private Sprite sprite;

        public ColorType Color => color;
        public Sprite Sprite => sprite;
    }

    public Sprite GetColorSprite(ColorType color)
    {
        foreach (var c in colors)
        {
            if (c.Color == color)
            {
                return c.Sprite;
            }
        }
        Debug.LogError($"Color sprite for {color} not found in ColorStatesDatabase.");
        return null;
    }

}
