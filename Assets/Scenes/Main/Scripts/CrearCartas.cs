﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrearCartas : MonoBehaviour {

	public GameObject CartaPrefab;
	public Transform CartasParent;
	private List<GameObject> cartas = new List<GameObject> ();
    public int ancho;

    public Material[] materiales;
	public Texture2D[] texturas;

	public int contadorClicks;
	public Text textoContadorIntentos;

	public Carta CartaMostrada;
	public bool sePuedeMostrar = true;

	public InterfazUsuario interfazUsuario;

	public int numParejasEncontradas;

    public AudioSource source;
    public AudioClip[] sonido;



    public void Reiniciar(){
		cartas.Clear ();
		GameObject[] cartasEli = GameObject.FindGameObjectsWithTag ("Carta");
		for (int i = 0; i < cartasEli.Length; i++) {
			DestroyImmediate  (cartasEli [i]);
		}

		contadorClicks = 0;
		textoContadorIntentos.text = "Intentos";
		CartaMostrada = null;
		sePuedeMostrar = true;
		numParejasEncontradas = 0;
		interfazUsuario.ReiniciarCronometro();
		interfazUsuario.ActivarCronometro();
        Crear ();
	}
   

    public void Crear()
    {
        ancho = interfazUsuario.dificultad;
		for(int i=0;i<ancho;i++){
			for(int x=0;x<ancho;x++){
				float factor =  9.0f/ancho;
				Vector3 posicionTemp = new Vector3 (x*factor, 0, i*factor);

				GameObject cartaTemp=Instantiate(CartaPrefab,posicionTemp,
					Quaternion.Euler(new Vector3(0,180,0)));

				cartaTemp.transform.localScale *= factor;

				cartas.Add (cartaTemp);

				cartaTemp.GetComponent<Carta> ().posicionOriginal = posicionTemp;

				cartaTemp.transform.parent = CartasParent;
                
			}
		}
		AsignarTexturas ();
		Barajar ();
	}

	void AsignarTexturas(){

		int[] arrayTemp =new int[texturas.Length];

		for (int i = 0; i <= texturas.Length-1; i++) {
			arrayTemp [i] = i;
		}


		for (int t = 0; t < arrayTemp.Length; t++ )
		{
			int tmp = arrayTemp[t];
			int r = Random.Range(t, arrayTemp.Length);
			arrayTemp[t] = arrayTemp[r];
			arrayTemp[r] = tmp;
		}
        
		int[] arrayDefinitivo = new int[(ancho*ancho)/2];

		for (int i = 0; i < arrayDefinitivo.Length ; i++) {
			arrayDefinitivo [i] = arrayTemp [i];
		}


		for(int i=0;i<cartas.Count ;i++){
			cartas[i].GetComponent<Carta> ().AsignarTextura (texturas[(arrayDefinitivo[i/2])] );
			cartas [i].GetComponent<Carta> ().idCarta = i / 2;
		}
	}

	void Barajar(){
		int aleatorio;

		for (int i = 0; i < cartas.Count; i++) {
			aleatorio = Random.Range (i, cartas.Count);

			cartas [i].transform.position = cartas [aleatorio].transform.position  ;
			cartas [aleatorio].transform.position  = cartas [i].GetComponent<Carta>().posicionOriginal;

			cartas [i].GetComponent<Carta> ().posicionOriginal = cartas [i].transform.position;
			cartas [aleatorio].GetComponent<Carta> ().posicionOriginal = cartas [aleatorio].transform.position;
		}
	}

	public void HacerClick(Carta _carta){
		if (CartaMostrada == null) {
			CartaMostrada = _carta;
		} else {
			contadorClicks++;
			ActualizarUI (); 
			if (CompararCartas (_carta.gameObject, CartaMostrada.gameObject)) {
                source.clip = sonido[0];
                source.Play();
                print ("Enhorabuena! Has encontrado una pareja!");
				numParejasEncontradas++;
				if (numParejasEncontradas == cartas.Count / 2)
                {
                    source.clip = sonido[2];
                    source.Play();
                    print ("Enhorabuena! Has encontrado todas las parejas!");
					interfazUsuario.MostrarMenuGanador ();
				}

			} else
            {
                source.clip = sonido[1];
                source.Play();
                _carta.EsconderCarta ();
				CartaMostrada.EsconderCarta();
			}
			CartaMostrada = null;

		}

	}

	public bool CompararCartas(GameObject carta1, GameObject carta2){
		bool resultado;
		if (carta1.GetComponent<Carta> ().idCarta  ==
			carta2.GetComponent<Carta> ().idCarta) {
			resultado = true;
		} else {
			resultado = false;
		}
		return resultado;
	}

    

    public void ActualizarUI(){
		textoContadorIntentos.text = "Intentos: " + contadorClicks;
	}
    
}
