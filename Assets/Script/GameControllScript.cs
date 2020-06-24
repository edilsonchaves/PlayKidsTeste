using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameControllScript : MonoBehaviour
{
    GameObject objetoSelecionado;
    public GridGameControll gridControll;
    public int pontuacaoObjetivoFaseTotal;
    public Text textoPontuacao;
    public Text textoTempo;
    public int PontuacaoTotal;
    int pontuacaoJogador;
    int faseJogo;
    float contadorTempo;
    // Start is called before the first frame update
    void Start()
    {
        contadorTempo = 120;
        pontuacaoJogador = 0;
        faseJogo = 1;
    }

    // Update is called once per frame
    void Update()
    {
        contadorTempo -= Time.deltaTime;
        textoTempo.text = ""+Mathf.FloorToInt(contadorTempo);
        pontuacaoJogador = PontuacaoTotal + gridControll.pontuacaoGrid;
        textoPontuacao.text = pontuacaoJogador + "/" + pontuacaoObjetivoFaseTotal;

        if (contadorTempo <= 0)
        {
            contadorTempo = 120;
            faseJogo += 1;
            pontuacaoObjetivoFaseTotal *= 4;
        }
        if(pontuacaoJogador > pontuacaoObjetivoFaseTotal)
        {
            textoPontuacao.color = Color.green;
        }
        else
        {
            textoPontuacao.color = Color.red;
        }
    }

    public void selecionandoObjeto(GameObject comidaSelecionada)
    {
        if (gridControll.statusGrid=="Jogando")
        {
            comidaSelecionada.GetComponent<ItemDisplay>().selecionado = true;
            comidaSelecionada.GetComponent<ItemDisplay>().tocarSom();
            if (objetoSelecionado == null)
            {
                objetoSelecionado = comidaSelecionada;

            }
            else
            {
                objetoSelecionado.GetComponent<ItemDisplay>().selecionado = false;
                comidaSelecionada.GetComponent<ItemDisplay>().selecionado = false;
                //Debug.Log("Realizar swap: " + objetoSelecionado.name + " , " + comidaSelecionada.name);
                //Debug.Log("Encontrei um segundo Objeto");
                int difLinha = objetoSelecionado.GetComponent<ItemDisplay>().posLinha - comidaSelecionada.transform.gameObject.GetComponent<ItemDisplay>().posLinha;
                int difColuna = objetoSelecionado.GetComponent<ItemDisplay>().posColuna - comidaSelecionada.transform.gameObject.GetComponent<ItemDisplay>().posColuna;
                if (Mathf.Abs(difLinha) + Mathf.Abs(difColuna) <= 1)
                {
                    //Debug.Log("Realizo Troca Inicial");

                    gridControll.realizarTrocaObjetos(objetoSelecionado, comidaSelecionada);
                    bool verificaAutenticidadeDaTroca = gridControll.verificaMatchsGrid(objetoSelecionado, comidaSelecionada);
                    if (verificaAutenticidadeDaTroca)
                    {
                        //Debug.Log("Troca Realizada com sucesso");

                        StartCoroutine(gridControll.Match(objetoSelecionado, comidaSelecionada));
                    }
                    else
                    {
                        //Debug.Log("Troca abortada");
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
        
    }
}
