﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ManagerPE : MonoBehaviour
{
    private string getPuntajeByGameUrl = "apidislexia.herokuapp.com/puntaje/findbygame";
    private string postPuntajeEditUrl = "apidislexia.herokuapp.com/puntaje/edit";
    private string getJuegoByNameUrl = "apidislexia.herokuapp.com/juego/findbyname";
    private string postJuegoCreateUrl = "apidislexia.herokuapp.com/puntaje/create";
    private string getPuntajeListaUrl = "apidislexia.herokuapp.com/puntaje/index";

    private string idjuego = "";
    private List<Puntaje> listaPuntaje;


    public Text puntos, fecha;
    public GameObject puntostab;
    bddislexia db = new bddislexia();
    public GameObject lettreOne, BoxOne, distrac1, distrac2, distrac3;
    public GameObject winText, btnsiguinte, btnir;

    public Text intentosj, totintentos;

    public GameObject intro;
    public GameObject mensaje, inicio;
    score sc = null;
    Puntaje sc2 = null;
    bool activa;
    public Canvas canvas;


   
    private string[] nombresPalabra1 ={ "3", "2945", "6", "7", "8", "15", "29", "42", "57", "63"};

    private string[] nombresImagen1 = { "3", "2945", "6", "7", "8", "15", "29", "42", "57", "63"};

    private string[] nombresPalabraDis1 = { "8", "3046", "3", "1", "0", "18", "93", "43", "87", "67" };
    private string[] nombresPalabraDis2 = { "0", "8046", "0", "2", "3", "48", "97", "74", "67", "68" };
    private string[] nombresPalabraDis3 = { "6", "28", "8", "4", "6", "46", "48", "71", "87", "93" };


    Vector3 lettreOneIni, distrac1Ini, distrac2Ini, distrac3Ini;

    string str = "";
    public string word;

    private Image palabra1;

    private Image distracIma1;
    private Image distracIma2;
    private Image distracIma3;

    public Image imag1;
    public string errores;

    bool oneCorrect = false;

    Vector3 iniScaleLettreOne;

    public AudioSource source;
    public AudioClip correct;
    public AudioClip incorrect;
    public AudioClip gana;

    public AudioClip[] sonido1;
    //public AudioClip[] sonido2;


    int cont = 0;
    int aux = 0;
    int intentos = 0;

    string txtinten = "INTENTOS:";
    //System.Random rnd = new System.Random();


    void Start()
    {
        StartCoroutine(GetJuegoByName(getJuegoByNameUrl, "PalabraEscondida"));
        aux++;
        /* List<score> lscore = db.consultarScore(login.getlog().id, "PalabraEscondida");
         errores = "" + lscore.Count();
         if (lscore.Count() > 0)
         {
             if (lscore[lscore.Count() - 1].pausa == "PAUSA")
             {
                 sc = lscore[lscore.Count() - 1];
                 cont = sc.nivel;
                 intentos = int.Parse(sc.puntos);
                 intentosj.text = txtinten + intentos;
             }
         }*/
        puntostab.SetActive(false);

        
        palabra1 = lettreOne.GetComponent<Image>();
        palabra1.sprite = Resources.Load<Sprite>("Imagenes/palabras/" + nombresPalabra1[cont]);

        distracIma1 = distrac1.GetComponent<Image>();
        distracIma1.sprite = Resources.Load<Sprite>("Imagenes/distractor/" + nombresPalabraDis1[cont]);

        distracIma2 = distrac2.GetComponent<Image>();
        distracIma2.sprite = Resources.Load<Sprite>("Imagenes/distractor/" + nombresPalabraDis2[cont]);

        distracIma3 = distrac3.GetComponent<Image>();
        distracIma3.sprite = Resources.Load<Sprite>("Imagenes/distractor/" + nombresPalabraDis3[cont]);

        imag1.sprite = Resources.Load<Sprite>("Imagenes/Figuras/" + nombresPalabra1[cont]);
        winText.SetActive(false);
        btnsiguinte.SetActive(true);

        lettreOneIni = lettreOne.transform.position;

        distrac1Ini = distracIma1.transform.position;
        distrac2Ini = distracIma2.transform.position;
        distrac3Ini = distracIma3.transform.position;

        iniScaleLettreOne = lettreOne.transform.localScale;

       
    }


    public void DragLettreOne()
    {
        lettreOne.transform.position = Input.mousePosition;
    }


    public void Dragdistractor1()
    {
        distrac1.transform.position = Input.mousePosition;
    }


    public void Dragdistractor2()
    {
        distrac2.transform.position = Input.mousePosition;
    }

    public void Dragdistractor3()
    {
        distrac3.transform.position = Input.mousePosition;
    }


    public void DropDistrac1()
    {

        intentos++;

        intentosj.text = txtinten + intentos;
        distrac1.transform.position = distrac1Ini;
        source.clip = incorrect;

    }

    public void DropDistrac2()
    {

        intentos++;

        intentosj.text = txtinten + intentos;
        distrac2.transform.position = distrac2Ini;
        source.clip = incorrect;


    }


    public void DropDistrac3()
    {

        intentos++;

        intentosj.text = txtinten + intentos;
        distrac3.transform.position = distrac3Ini;
        source.clip = incorrect;


    }

    public void DropLettreOne()
    {
        float Distance = Vector3.Distance(lettreOne.transform.position, BoxOne.transform.position);

        intentos++;

        intentosj.text = txtinten + intentos;
        if (Distance < 40 && oneCorrect == false)
        {
            lettreOne.transform.localScale = BoxOne.transform.localScale;
            lettreOne.transform.position = BoxOne.transform.position;
            oneCorrect = true;
            BoxOne.name = lettreOne.name;
            source.clip = sonido1[cont];
 
        }

        else
        {
            lettreOne.transform.position = lettreOneIni;
            source.clip = incorrect;

        }

    }


    public void Reload()
    {
        cont++;
        winText.SetActive(false);
        btnsiguinte.SetActive(false);
        palabra1.sprite = Resources.Load<Sprite>("Imagenes/palabras/" + nombresPalabra1[cont]);

        imag1.sprite = Resources.Load<Sprite>("Imagenes/Figuras/" + nombresPalabra1[cont]);


        distracIma1.sprite = Resources.Load<Sprite>("Imagenes/distractor/" + nombresPalabraDis1[cont]);
        distracIma2.sprite = Resources.Load<Sprite>("Imagenes/distractor/" + nombresPalabraDis2[cont]);
        distracIma3.sprite = Resources.Load<Sprite>("Imagenes/distractor/" + nombresPalabraDis3[cont]);
        str = "";


        oneCorrect = false;
        // twoCorrect = false;


        BoxOne.name = "1";

        lettreOne.transform.position = lettreOneIni;


        lettreOne.transform.localScale = iniScaleLettreOne;
        if(cont==10)
        {
            SceneManager.LoadScene("Principal");
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {

            activa = !activa;
            canvas.enabled = activa;
            Time.timeScale = (activa) ? 0 : 1f;
        }

        if (oneCorrect == true)
        {

            if (cont == 9)
            {

                totintentos.text = "SUS INTENTOS FUERON:" + intentos;
                winText.SetActive(true);

            }

            btnsiguinte.SetActive(true);

        }
    }


    public void btnbnext()
    {
        if (cont == 10)
        {
            source.clip = gana;
            SceneManager.LoadScene("Principal");
        }
        if (cont == 9)
        {
            if (sc2 != null)
            {

                sc2.estado = "COMPLETADO";
                sc2.nivel = aux;
                sc2.score = intentos;

                //db.Updatescore("PalabraEscondida", sc);
                Debug.Log(sc2.estudianteId._id);
                Debug.Log(sc2.juegoId._id);

                PostPuntaje postPuntaje = new PostPuntaje()
                {
                    score = sc2.score,
                    estado = sc2.estado,
                    nivel = sc2.nivel,
                    fecha = sc2.fecha,
                    estudianteId = sc2.estudianteId._id,
                    juegoId = sc2.juegoId._id
                };

                StartCoroutine(PostActualizarPuntaje(postPuntajeEditUrl, postPuntaje, sc2._id));
            }
            else
            {
                SceneManager.LoadScene("MenuPE");
                
                sc = new score(0, "" + intentos, "" + DateTime.Now, "completado", cont);

                //db.GuardarScore(sc, "PalabraEscondida");
                PostPuntaje postPuntaje = new PostPuntaje()
                {
                    score = intentos,
                    nivel = 1,
                    fecha = DateTime.Now.ToString(),
                    estado = "completado",
                    estudianteId = LoginE.getlog()._id,
                    juegoId = idjuego

                };

                StartCoroutine(PostAgregarPuntaje(postJuegoCreateUrl, postPuntaje));
            }
            SceneManager.LoadScene("MenuPE");
        }
        else
        {
            Reload();

        }

    }

    public void verscore()
    {
        /* List<score> lscore = db.consultarScore(login.getlog().id, "PalabraEscondida");
         string pun = "", fech = "";
         puntostab.SetActive(true);

         foreach (score item in lscore)
         {
             int valor = int.Parse(item.puntos);

             valor = 122 - valor;
             if (valor < 0)
             {
                 valor = 0;
             }

             if (valor > 100)
             {
                 pun += "***% \n";
             }
             else
             {
                 pun += valor + " % \n";
             }

             fech += item.fecha + "\n";
         }

         fecha.text = fech;
         puntos.text = pun;*/

        StartCoroutine(GetByGameListaPuntaje(getPuntajeByGameUrl));


        puntostab.SetActive(true);
    }

    public void salircore()
    {
        puntostab.SetActive(false);

    }

    public void ir()
    {
        mensaje.SetActive(false);
        inicio.SetActive(false);
    }


    public void Salir()
    {
        SceneManager.LoadScene("Principal");
    }

    public void verPuntajeForeach(List<Puntaje> listaPuntaje)
    {
        string pun = "", fech = "";

        foreach (Puntaje puntaje in listaPuntaje)
        {
            int valor = puntaje.score;
            Debug.Log("Prueba.h");
            Debug.Log(valor);
            valor = 110 - valor;
            if (valor < 0)
            {
                valor = 0;
            }

            if (valor > 100)
            {
                pun += "***% \n";
            }
            else
            {
                pun += valor + " % \n";
            }

            fech += puntaje.fecha + "\n";
        }
        fecha.text = fech;
        puntos.text = pun;

    }

    public void salirPausa()
    {

        if (sc != null)
        {
            sc.pausa = "PAUSA";
            sc.nivel = cont;
            sc.puntos = "" + intentos;
            db.Updatescore("PalabraEscondida", sc);
        }
        else
        {
            sc = new score(0, "" + intentos, "" + DateTime.Now, "PAUSA", cont);
            db.GuardarScore(sc, "PalabraEscondida");
        }
        SceneManager.LoadScene("PalabraEscondida");
    }

    public IEnumerator PostAgregarPuntaje(string url, PostPuntaje postPuntaje)
    {
        //recojo datos del tutor y del juego para guardar a la bdd

        var jsonData = JsonUtility.ToJson(postPuntaje);

        using (UnityWebRequest www = UnityWebRequest.Post(url, jsonData))
        {
            www.SetRequestHeader("content-type", "application/json");
            www.uploadHandler.contentType = "application/json";
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    // handle the result
                    var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    var msg = JsonUtility.FromJson<Msg>(result);
                    Debug.Log(msg.msg);
                }
                else
                {
                    //handle the problem
                    Debug.Log("Error! data couldn't get.");

                }
            }
        }
    }


    //LISTAR PUNTAJE CREADA POR MI

    public IEnumerator GetByGameListaPuntaje(string getPuntajeByGameListaUrl)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(getPuntajeByGameListaUrl + "/" + idjuego))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    result = "{\"result\":" + result + "}";
                    Debug.Log(result);
                    var puntajeLista = JsonHelper.FromJson<Puntaje>(result);
                    listaPuntaje = puntajeLista;
                    verPuntajeForeach(listaPuntaje);
                    errores = "" + puntajeLista.Count();

                    if (puntajeLista.Count() > 0)
                    {
                        if (puntajeLista[puntajeLista.Count() - 1].estado == "PAUSA")
                        {
                            sc2 = puntajeLista[puntajeLista.Count() - 1];
                            aux = sc2.nivel;

                            intentos = sc2.score;

                            intentosj.text = txtinten + intentos;
                        }

                    }
                }
                else
                {
                    //handle the problem
                    Debug.Log("Error! data couldn't get.");
                }
            }
        }
    }



    public IEnumerator GetJuegoByName(string getJuegoByNameUrl, string name)
    {

        using (UnityWebRequest www = UnityWebRequest.Get(getJuegoByNameUrl + '/' + name))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    Debug.Log(result);


                    var juego = JsonUtility.FromJson<Juego>(result);
                    idjuego = juego._id;
                    StartCoroutine(GetByGameListaPuntaje(getPuntajeByGameUrl));

                }
                else
                {
                    Debug.Log("Error! data couldn't get.");
                }
            }
        }

    }


    public IEnumerator PostActualizarPuntaje(string postPuntajeActualizarUrl, PostPuntaje postPuntaje, string id)
    {
        //recojo datos del tutor y del juego para guardar a la bdd

        var jsonData = JsonUtility.ToJson(postPuntaje);

        using (UnityWebRequest www = UnityWebRequest.Post(postPuntajeActualizarUrl + "/" + id, jsonData))
        {
            www.SetRequestHeader("content-type", "application/json");
            www.uploadHandler.contentType = "application/json";
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    // handle the result
                    var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    var msg = JsonUtility.FromJson<Msg>(result);
                    Debug.Log(msg.msg);
                }
                else
                {
                    //handle the problem
                    Debug.Log("Error! data couldn't get.");

                }
            }
        }
    }


}
