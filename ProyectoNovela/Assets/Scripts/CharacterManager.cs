using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts;
using System;
using System.Xml.Serialization;
using UnityEngine.UIElements;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    private List<Character> _characters;

    [SerializeField]
    private GameObject _charaterPrefab;

    [SerializeField]
    private CharacterMoods _narradorMoods;


    void Start()
    {
        _characters = new List<Character>();
    }

    public void CreateCharacter(string name, string position, string mood)
    {

        // try pase es para intentar combertir traducuir 
        if (!Enum.TryParse(name, out CharacterName nameEnum))
        {
            Debug.LogWarning($"Fallo al parsear el nombre de el personaje a enum: {name}");
            return;
        }
        if (!Enum.TryParse(position, out CharacterPosition positionEnum))
        {
            Debug.LogWarning($"Fallo al parsear el nombre de el personaje a enum: {position}");
            return;
        }
        if (!Enum.TryParse(mood, out CharacterMood moodEnum))
        {
            Debug.LogWarning($"Fallo al parsear el nombre de el personaje a enum: {mood}");
            return;
        }

        CreateCharacter(nameEnum, positionEnum, moodEnum);
    }

public void CreateCharacter (CharacterName name, CharacterPosition position, CharacterMood mood)
    {
   
        var character = _characters.FirstOrDefault(x => x.Name == name);//si el primero de la lista o el por defecto es igal al nombre que tengo que crear
     
        if (character == null)//si no esta en la lista que acavas de mirar
        {
           
            var CharacterObject = Instantiate(_charaterPrefab, gameObject.transform, false);// crea el personaje
     
            character = CharacterObject.GetComponent<Character>();// coge su script character

            _characters.Add(character);//y metelo en la lista de personajes creados
           
        }
        else if (character.IsShowing)
        {
            Debug.LogWarning($"Error al mostrar el personaje {name}. El personaje ya esta en pantalla");
                return;
        }

        //inicializa el personaje
        character.Init(name, position, mood, GetMoodSetForCharacter(name)); //coje el mood de cada personaje llamado name.
    }

    public void HideCharacter(string name)
    {
        if (!Enum.TryParse(name , out CharacterName nameEnum))
        {
            Debug.Log($"Fallo al parsear el nombre del personaje a enum:{name}");
        }
        HideCharacter(nameEnum);
    }

    public void HideCharacter(CharacterName name)
    {
        var character = _characters.FirstOrDefault(_x => _x.Name == name);

        if (character?.IsShowing != true)//si el personaje existe y is showing no es igual a true
        {
            Debug.LogWarning($"Character {character.Name} no esta siendo mostrado. No se puede esconder");
        }

        else
        {
            character?.Hide();
        }
    }

    public void ChangeMood(string name, string mood)
    {
        if (!Enum.TryParse(name, out CharacterName nameEnum))
        {
            Debug.LogWarning($"Fallo al parsear el nombre del personaje a enum: {name}");
            return;
        }
        if (!Enum.TryParse(mood, out CharacterMood moodEnum))
        {
            Debug.LogWarning($"Fallo al parsear el mood del personaje a enum: {mood}");
            return;
        }
        ChangeMood(nameEnum, moodEnum);
    }
    public void ChangeMood(CharacterName name, CharacterMood mood)
    {
        var Character = _characters.FirstOrDefault(x => x.Name == name);

        if (Character?.IsShowing != true)
        {
            Debug.LogWarning($"Character {name} no esta en pantalla. No se puede cambiar su mood");
            return;
        }

        else
        {
            Character?.ChangeMood(mood);
        }
    }

    private CharacterMoods GetMoodSetForCharacter(CharacterName name)
    {
        switch (name)
        {
            case CharacterName.Narrador:
                return _narradorMoods;
            default:
                Debug.LogError($"No se pudo encontrar el moodset de {name}");
                return null;
        }
    }
}