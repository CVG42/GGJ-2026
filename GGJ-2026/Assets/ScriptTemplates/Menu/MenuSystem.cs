using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    [Header("Navegación")]
    public GameObject panelMenu;     
    public GameObject panelOpciones; 
    public GameObject panelCreditos; 

    [Header("Configuración de Audio")]
    public Toggle toggleMusica;
    public Toggle toggleSonidos;

    // Menu Principal

    public void Jugar()
    {
        SceneManager.LoadScene("SampleScene"); // Cambiar nombre a la escena de inicio del juego
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    // Paneles de Navegación

    public void AbrirOpciones()
    {
        panelMenu.SetActive(false);
        panelOpciones.SetActive(true);
    }

    public void AbrirCreditos() 
    {
        panelMenu.SetActive(false);
        panelCreditos.SetActive(true);
    }

    // Cerrar Paneles

    public void CerrarOpciones()
    {
        panelOpciones.SetActive(false);
        panelMenu.SetActive(true);
    }

    public void CerrarCreditos() 
    {
        panelCreditos.SetActive(false);
        panelMenu.SetActive(true);
    }

    // Menu de audio

    public void CambiarMusica(bool encendido)
    {
        if (encendido) Debug.Log("Música ON");
        else Debug.Log("Música OFF");
    }

    public void CambiarSonidos(bool encendido)
    {
        if (encendido) Debug.Log("Sonidos ON");
        else Debug.Log("Sonidos OFF");
    }
}