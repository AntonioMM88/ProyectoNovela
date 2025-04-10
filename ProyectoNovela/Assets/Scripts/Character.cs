using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class Character : MonoBehaviour
{
    //{  get; private set; } legiible desde cualquier sitio pero no cambiable desde cualquier sitio
    public CharacterPosition Position { get; private set; } //posicion actual

    public CharacterName Name { get; private set; }//nombre actual

    public CharacterMood Mood { get; private set; }//sprite actual

    public bool IsShowing { get; private set; }//si esta en pantalla o no

    private CharacterMoods _moods;// sprites totales

    private float _offSreenX, _onSreenX;// posiciones dentro y fuera de la pantalla

    private readonly float _animationSpeed = 0.5f;//velocidad de movieneto

    public void Init(CharacterName name, CharacterPosition position, CharacterMood mood, CharacterMoods moods)
    {
        Name = name;
        Position = position;
        Mood = mood;
        _moods = moods;

        Show();
    }

    public void Show()
    {
        //busca la posicion a la que tiene que ir
        SetPositionValues();

        //personaje fuera de la pantalla para poder entrar
        transform.position = new Vector3(_offSreenX, transform.position.y, transform.localPosition.z);

        //comprobar que el sprite esta actualizado, metodo
        UpdateSprite();

        //mover el personaje a su sitio con velocidad linear y hacer que isShowin = true
        LeanTween.moveX(gameObject, _onSreenX, _animationSpeed).setEase(LeanTweenType.linear).setOnComplete(() => { IsShowing = true; });
    }

    public void Hide()
    {
        LeanTween.moveX(gameObject, _offSreenX, _animationSpeed).setEase(LeanTweenType.linear).setOnComplete(() => { IsShowing = false; });
    }

    private void SetPositionValues()
    {
        //swich = a muchos if seguidos
        switch (Position)
        {
            case CharacterPosition.Left:
                _onSreenX = Screen.width * 0.25f;
                _offSreenX = -Screen.width * 0.25f;
                break;

            case CharacterPosition.Center:
                _onSreenX = Screen.width * 0.5f;
                _offSreenX = -Screen.width * 0.25f;
                break;

            case CharacterPosition.Right:
                _onSreenX = Screen.width * 0.75f;// posicion dentro de pantalla
                _offSreenX = Screen.width * 1.25f;//para que se valla
                break;
        }
    }

    public void ChangeMood(CharacterMood mood)
    {
        Mood = mood;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
       var sprite = _moods.GetMoodSrite(Mood);
        var image = GetComponent<Image>();

        image.sprite = sprite;
        image.preserveAspect = true;
    }

}
