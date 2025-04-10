using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class CharacterMoods : MonoBehaviour
{
    public CharacterName Name;
    public Sprite Happy;
    public Sprite Sad;
    public Sprite Angry;
    public Sprite Freak;

    public Sprite GetMoodSrite(CharacterMood mood)
    {
        // ?? = if else porsi algo no funciona
        switch (mood)
        {
            case CharacterMood.Happy:
                return Happy;
            case CharacterMood.Sad:
                return Sad ?? Happy;
            case CharacterMood.Angry:
                return Angry ?? Happy;
            case CharacterMood.Freak:
                return Freak ?? Happy;
            default:
                Debug.Log($"No se encontro el Sprite para el personaje: {Name}, mood : {mood}");
                return Happy;

        }
    }

}
