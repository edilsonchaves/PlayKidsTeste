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
    public GameObject buttonEmbaralhar;
    public GameObject fimFaseUI;
    public int tempoJogo;
    int pontuacaoJogador;
    int faseJogo;
    float contadorTempo;
    // Start is called before the first frame update
    void Start()
    {
        contadorTempo = tempoJogo;
        pontuacaoJogador = 0;
        faseJogo = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(gridControll.statusGrid != "Fim de Jogo")
        {
            contadorTempo -= Time.deltaTime;
            textoTempo.text = "" + Mathf.FloorToInt(contadorTempo);
            pontuacaoJogador =  gridControll.pontuacaoGrid;
            textoPontuacao.text = pontuacaoJogador + "/" + pontuacaoObjetivoFaseTotal;
        }
        else
        {
            contadorTempo = 0;
            textoTempo.text = "" + Mathf.FloorToInt(contadorTempo);
        }


        if (contadorTempo <= 0)
        {
            if (pontuacaoJogador < pontuacaoObjetivoFaseTotal)
            {
                if(gridControll.statusGrid != "Fim de Jogo")
                {
                    Debug.Log("Fim de Jogo");
                    gridControll.FimDeJogo();
                    fimFaseUI.SetActive(true);
                    fimFaseUI.GetComponent<FimJogoUI>().ConstruirResultado(pontuacaoJogador, faseJogo);
                }
            }
            else
            {
                contadorTempo = tempoJogo;
                faseJogo += 1;
                pontuacaoObjetivoFaseTotal = Mathf.RoundToInt(pontuacaoObjetivoFaseTotal * 2.5f);
            }
            
            
        }
        if(pontuacaoJogador > pontuacaoObjetivoFaseTotal)
        {
            textoPontuacao.color = Color.green;
        }
        else
        {
            textoPontuacao.color = Color.red;
        }

        if(gridControll.statusGrid== "Embaralhar")
        {
            buttonEmbaralhar.SetActive(true);
        }
        else{
            buttonEmbaralhar.SetActive(false);
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
