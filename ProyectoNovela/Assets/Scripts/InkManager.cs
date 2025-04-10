using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts;
using System;




public class InkManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset _inkJsonAsset;
    private Story _story;
    [SerializeField]
    private TMP_Text _textField;

    [SerializeField]
    private VerticalLayoutGroup _choiceButtonsContainer;

    [SerializeField]
    private Button _choiceButtonPrefab;

    [SerializeField]
    private Color _normalTextColor;
    [SerializeField]
    private Color _pensamientoTextColor;

    public float typingSpeed;

    private Coroutine _displayNextLineCoroutine;

    private CharacterManager _characterManager;

    private const string Speaker_Tag = "speaker";
    private const string Style_Tag = "pensamiento";
    [SerializeField]
    private TMP_Text textMeshProSpeaker;

    [SerializeField]
    private Image Scena1, Scena2;

    private AudioManager _audioManager;


    void Start()
    {
        _characterManager = FindObjectOfType<CharacterManager>();
        _audioManager = FindObjectOfType<AudioManager>();
        StartStory();
    }

    // Update is called once per frame
    void Update()
    {

    }
    // el => sirve para crear una funcon de eso pero con los parametros que vuenen en la misma linea 
    void StartStory()//Inicia nuestra historia
    {
        _story = new Story(_inkJsonAsset.text);
        _story.BindExternalFunction("ShowCharacter", (string name, string position, string mood) =>
        _characterManager.CreateCharacter(name, position, mood));

        _story.BindExternalFunction("HideCharacter", (string name) => _characterManager.HideCharacter(name));
        _story.BindExternalFunction("ChangeMood", (string name, string mood) => _characterManager.ChangeMood(name, mood));
        _story.BindExternalFunction("SwitchSong", () => _audioManager.SwitchSong());
        //  _story.BindExternalFunction("ChangeScene", (string scene) => ChangeScene(scene));

        DisplayNextLine();
    }

    public void DisplayNextLine()//mostrar siguente linea de texto de la historia
    {
        if (_story.canContinue)
        {
            string text = _story.Continue(); //recoge la siguiente línea
            text = text?.Trim(); //Recortar el espacio blanco del texto
            _textField.color = _normalTextColor;
            _textField.fontStyle = FontStyles.Normal;
            ResolveTags(_story.currentTags); //resuelve los tags que haya en la línea
            //_textField.text = text; // muestra en la cajita de texto el nuevo texto
            if (_displayNextLineCoroutine != null)
            {
                StopCoroutine(_displayNextLineCoroutine); //si la corutina está activamente encendida, párala antes de encenderla otra vez
            }
            _displayNextLineCoroutine = StartCoroutine(DisplayLineLetterByLetter(text)); //llama a la corutina que escribe letra a letra
        }
        else if (_story.currentChoices.Count > 0)
        {
            DisplaceChoices();
        }

        void DisplaceChoices()
        {
            if (_choiceButtonsContainer.GetComponentsInChildren<Button>().Length > 0) return;// comprueba si ya hay elecciones mostrandose

            for (int i = 0; i < _story.currentChoices.Count; i++)//cicla todas las elecciones
            {
                var choice = _story.currentChoices[i];
                var button = CreateChoiceButton(choice.text); // crea un bottn de eleccion

                button.onClick.AddListener(() => OnclickButtonChoice(choice));//añade al eschahador para que funcione el boton al hacer click
            }
        }

        Button CreateChoiceButton(string text)
        {
            var choiceButton = Instantiate(_choiceButtonPrefab);//crea el boton
            choiceButton.transform.SetParent(_choiceButtonsContainer.transform, false);//mete el boton en el grupo de eleciones

            var buttonText = choiceButton.GetComponentInChildren<TMP_Text>();//pillamos el texto a cambiar  de lo de ink
            buttonText.text = text;// cambiamos el texto

            return choiceButton;
        }

        void OnclickButtonChoice(Choice choice)
        {
            _story.ChooseChoiceIndex(choice.index);// le dica a ink cual de las elecciones se aha escogido
            ClearChoiceView();//quitar las otras elecciones de la pantalla
            _story.Continue();//puesto ante que DisplayNextLine(); para que se salte la linea del botton
            DisplayNextLine();

        }
        void ClearChoiceView()
        {
            if (_choiceButtonsContainer != null)
            {
                foreach (var button in _choiceButtonsContainer.GetComponentsInChildren<Button>()) //limpia todas las elecciones actuales
                {
                    Destroy(button.gameObject);
                }
            }
        }
    }
    private IEnumerator DisplayLineLetterByLetter(string line)
    {
        _textField.text = " "; //vacía el cuadro de texto

        foreach (char letter in line.ToCharArray()) //por cada letra de la línea, súmala al texto y espera la cantidad de tiempo "typingSpeed" para
        {                                           // escribir la siguiente línea
            if (Input.GetKeyDown(KeyCode.K)) // si pulsas la tecla K (en este caso), sáltate el letra a letra y muestra la línea entera de golpe.
            {
                _textField.text = line;
                break;
            }
            _textField.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }





    private void ResolveTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)// un bucle que recorra todos los tags actuales
        {
            string[] splitTag = tag.Split(":");// como los tags estan escritos en formato "ASUNTO:VALOR" , separalos
            if(splitTag.Length != 2)//si te da un numero distinto a 2, no esta bien separado
            {
                Debug.LogError("No se pitdo parsear el tag " + tag);
            }
            string tagKey = splitTag[0].Trim(); //mete el primer valor de la separacion en tagKey
            string tagValue = splitTag[1].Trim(); //y el segundo en tagvalue

            switch (tagKey)// se pueden añadir cosas para cambiar la musica la imagen etc
            {
                case Speaker_Tag:
                    textMeshProSpeaker.text = tagValue;
                    break;
                case Style_Tag:
                    if (tagValue == "pensamiento")
                    {
                        _textField.color = _pensamientoTextColor;
                        _textField.fontStyle = FontStyles.Italic;
                    }
                    break;
                default:
                    Debug.LogWarning("El tag entro pero no se puede resolver: " + tag);
                    break;
            }
        }

    }


  /*
    private void ChangeScene(string scene)
    {
        switch (scene)
        {
            case Bosque:
                if (scene == "Scene1")
                {

                }
                   
                        break;
        }
    }
  */
}

