using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class NextButtonScript : MonoBehaviour
{
    [SerializeField]
    private InkManager _inkmanager;
    void Start()
    {
        _inkmanager = FindObjectOfType<InkManager>();//vusco game manager en la escena

        Button miButton = GetComponent<Button>();

        miButton.onClick.AddListener(OnClick);// cojo el componente del boton y a este le añanado un evento para que cuando pulse el boton haga onclick

        if (_inkmanager == null )
        {
            Debug.LogError("No se Encontro Ink Manager");
        }
    }

  public void OnClick()
    {
        _inkmanager?.DisplayNextLine();
    }
 
}
