using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllScript : MonoBehaviour
{
    public enum StatusJogo
    {Jogando,
     Match,
     Derrota,
    }
    StatusJogo status = new StatusJogo();
    public GameObject objetoSelecionado;
    public GridGameControll gridControll;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selecionandoObjeto(GameObject comidaSelecionada)
    {
        comidaSelecionada.GetComponent<ItemDisplay>().selecionado = true;
        if (objetoSelecionado == null)
        {
            objetoSelecionado = comidaSelecionada;
            
        }
        else
        {
            objetoSelecionado.GetComponent<ItemDisplay>().selecionado = false;
            comidaSelecionada.GetComponent<ItemDisplay>().selecionado = false;
            Debug.Log("Realizar swap: " + objetoSelecionado.name + " , " + comidaSelecionada.name);
            //Debug.Log("Encontrei um segundo Objeto");
            int difLinha = objetoSelecionado.GetComponent<ItemDisplay>().posLinha - comidaSelecionada.transform.gameObject.GetComponent<ItemDisplay>().posLinha;
            int difColuna = objetoSelecionado.GetComponent<ItemDisplay>().posColuna - comidaSelecionada.transform.gameObject.GetComponent<ItemDisplay>().posColuna;
            if (Mathf.Abs(difLinha) + Mathf.Abs(difColuna) <= 1)
            {
                Debug.Log("Realizo Troca Inicial");

                gridControll.realizarTrocaObjetos(objetoSelecionado, comidaSelecionada);
                bool verificaAutenticidadeDaTroca = gridControll.verificaMatchsGrid(objetoSelecionado, comidaSelecionada);
                if (verificaAutenticidadeDaTroca)
                {
                    Debug.Log("Troca Realizada com sucesso");

                    StartCoroutine(gridControll.Match(objetoSelecionado, comidaSelecionada));
                }
                else
                {
                    Debug.Log("Troca abortada");
                    gridControll.realizarTrocaObjetos(comidaSelecionada, objetoSelecionado);
                }
                objetoSelecionado = null;
            }
            else
            {
                Debug.Log("Nao Realizo Troca");
                objetoSelecionado = null;
            }
        }
    }
    public bool getStatusJogoJogando()
    {
        if (status == StatusJogo.Jogando)
        {
            return true;
        }

        return false;
    }

    public bool getStatusJogoMatch()
    {
        if (status == StatusJogo.Match)
        {
            return true;
        }

        return false;
    }

    public bool getStatusJogoDerrota()
    {
        if (status == StatusJogo.Derrota)
        {
            return true;
        }

        return false;
    }
}
