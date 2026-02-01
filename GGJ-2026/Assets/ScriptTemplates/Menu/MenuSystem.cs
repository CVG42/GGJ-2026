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
    [Header("Configuración de Sonido")]
    public AudioSource musicaFondo;
    public AudioSource efectosSonido;
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

    // Funcion para el toggle de musica
    public void ControlarMusica(bool estado)
    {
        if (estado)
        {
            musicaFondo.mute = !estado;
            Debug.Log("Música activa: " + estado);
        }
        
    }

    // Funcion para el toggle de sonidos
    public void ControlarSonido(bool estado)
    {
        if (estado)
        {
            efectosSonido.mute = !estado;
            Debug.Log("Sonidos activos: " + estado);
        }
    }
}