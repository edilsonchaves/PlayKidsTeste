﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridGameControll : MonoBehaviour
{
    public ItemScript[] itensCena;
    public GameObject[] itensInstanciadosEmCena;
    public GameObject[] gridsInstanciadosEmCena;
    public GameObject objetoCenario;
    public GameObject gridGameObject;
    public int constQuantObjetosCena;
    public int constObjetosLinha;
    bool atualizandoGrid;
    public Canvas canvas;
    public AudioSource audioGameControll;
    // Start is called before the first frame update
    void Start()
    {
        SpawnGridJogo();
        atualizandoGrid = false;
        SistemaSpawnInicial();
        CorreçaoSpawn();
    }

    // Update is called once per frame
    /*
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !atualizandoGrid)
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 40))
            {
                if (hit.transform.gameObject.layer == 8)
                {
                    if (objetoSelecionado == null)
                    {
                        //Debug.Log("Encontrei um objeto");
                        objetoSelecionado = hit.transform.gameObject;
                    }
                    else
                    {
                        //Debug.Log("Encontrei um segundo Objeto");
                        int difLinha = objetoSelecionado.GetComponent<ItemDisplay>().posLinha - hit.transform.gameObject.GetComponent<ItemDisplay>().posLinha;
                        int difColuna = objetoSelecionado.GetComponent<ItemDisplay>().posColuna - hit.transform.gameObject.GetComponent<ItemDisplay>().posColuna;
                        if (Mathf.Abs(difLinha) + Mathf.Abs(difColuna) <= 1)
                        {
                            //Debug.Log("Realizo Troca Inicial");
                            realizarTrocaObjetos(objetoSelecionado, hit.transform.gameObject);

                            bool verificaAutenticidadeDaTroca = verificaMatchsGrid(objetoSelecionado, hit.transform.gameObject);
                            if (verificaAutenticidadeDaTroca)
                            {
                                //Debug.Log("Troca Realizada com sucesso");
                                atualizandoGrid = true;
                                StartCoroutine(Match(objetoSelecionado, hit.transform.gameObject));
                            }
                            else
                            {
                                //Debug.Log("Troca abortada");
                                realizarTrocaObjetos(hit.transform.gameObject, objetoSelecionado);
                            }
                            objetoSelecionado = null;
                        }
                        else
                        {
                            //Debug.Log("Nao Realizo Troca");
                            objetoSelecionado = null;
                        }

                    }

                }


            }
            else
            {
                objetoSelecionado = null;
            }
        }
    }*/
    void SpawnGridJogo()
    {
        gridsInstanciadosEmCena = new GameObject[constQuantObjetosCena];
        int coluna = 0; 
        int linha = 0;
        for (int i =0; i< constQuantObjetosCena; i++)
        {
            GameObject gridInstanciado = Instantiate(gridGameObject, Vector3.zero, Quaternion.identity);
            gridInstanciado.transform.SetParent(canvas.transform);
            gridInstanciado.name = "Grid Objeto " + i;
            float anchorValorMinX = 0 + coluna * (1f / constObjetosLinha);
            float anchorValorMaxX = 0 + (coluna+1) * (1f / constObjetosLinha);
            Debug.Log("Anchor X: "+ anchorValorMinX+" , " + anchorValorMaxX);
            float anchorValorMinY = 0 + linha * (0.8f/(constQuantObjetosCena / constObjetosLinha));
            float anchorValorMaxY = 0 + (linha+1) * (0.8f/(constQuantObjetosCena / constObjetosLinha));
            Debug.Log("Anchor Y: " + anchorValorMinY + " , " + anchorValorMaxY);
            gridInstanciado.GetComponent<RectTransform>().anchorMin = new Vector2(anchorValorMinX, anchorValorMinY);
            gridInstanciado.GetComponent<RectTransform>().anchorMax = new Vector2(anchorValorMaxX, anchorValorMaxY);
            gridInstanciado.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            gridInstanciado.GetComponent<RectTransform>().transform.position= new Vector3(0,0,0);
            gridInstanciado.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            gridsInstanciadosEmCena[i] = gridInstanciado;
            coluna++;
            if (i % constObjetosLinha == constObjetosLinha - 1)
            {
                coluna = 0;
                linha += 1;
            }
        }
        
    }


    #region Spawn Elementos Jogo

    void SistemaSpawnInicial()
    {
        itensInstanciadosEmCena = new GameObject[constQuantObjetosCena];
        int coluna = 0;
        int linha = 0;
        for (int i = 0; i < constQuantObjetosCena; i++)
        {
            InstanciarBala(gridsInstanciadosEmCena[i], i, linha, coluna);
            coluna++;
            if (i % constObjetosLinha == constObjetosLinha - 1)
            {
                coluna = 0;
                linha += 1;
            }
        }
    }


    void CorreçaoSpawn()
    {
        for (int i = 0; i < constQuantObjetosCena; i++)
        {

            MatchInfo objeto = coletarInformacaoMatch(itensInstanciadosEmCena[i]);
            if (objeto.getValidMatch())
            {
                ItemDisplay itemDisplay = itensInstanciadosEmCena[i].GetComponent<ItemDisplay>();
                int linha = itemDisplay.posLinha;
                int coluna = itemDisplay.posColuna;
                Destroy(itensInstanciadosEmCena[i]);
                InstanciarBala(gridsInstanciadosEmCena[i],i, linha, coluna);
                i--;
            }
        }
    }

    void InstanciarBala(GameObject paiGrid, int posicao, int valorLinha, int valorColuna)
    {
        GameObject objetoInstanciado = Instantiate(objetoCenario, Vector3.zero, Quaternion.identity) as GameObject;
        objetoInstanciado.name = "ObjetoCenaPos" + posicao;
        ConfigurarPosicaoBala(paiGrid,objetoInstanciado);
        itensInstanciadosEmCena[posicao] = objetoInstanciado;
        objetoInstanciado.GetComponent<ItemDisplay>().ConfigurandoItem(gridsInstanciadosEmCena[posicao],itensCena[Random.Range(0, itensCena.Length)], valorLinha, valorColuna);
    }
    void ConfigurarPosicaoBala(GameObject posGrid, GameObject objeto)
    {
        objeto.transform.SetParent(posGrid.transform);
        objeto.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        objeto.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        objeto.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }
    #endregion
   
    #region Match Jogo
 
    public bool verificaMatchsGrid(GameObject item1, GameObject item2)
    {
        List<GameObject> listaHorizontalObjeto1 = procuraHorizontal(item1);
        List<GameObject> listaVerticalObjeto1 = procuraVertical(item1);
        List<GameObject> listaHorizontalObjeto2 = procuraHorizontal(item2);
        List<GameObject> listaVerticalObjeto2 = procuraVertical(item2);

        if (listaHorizontalObjeto1.Count >= 3 || listaVerticalObjeto1.Count >= 3)
        {
            return true;
        }

        if (listaHorizontalObjeto2.Count >= 3 || listaVerticalObjeto2.Count >= 3)
        {
            return true;
        }
        return false;
    }

    List<GameObject> procuraHorizontal(GameObject itemReferenciaBusca)
    {
        ItemDisplay itemInfo = itemReferenciaBusca.GetComponent<ItemDisplay>();
        List<GameObject> listaObjetosIdenticosHorizontal = new List<GameObject>();
        listaObjetosIdenticosHorizontal.Add(itemReferenciaBusca);
        int posObjetoReferencia = itemInfo.posColuna + constObjetosLinha * itemInfo.posLinha;

        //Debug.Log("Posicao Objeto: " + posObjetoReferencia);
        //Debug.Log("Procurando antes do item de referencia Horizontal");
        //Debug.Log("Teste rapido Minimo:" + constObjetosLinha * itemInfo.posLinha);
        for (int i = posObjetoReferencia - 1; i >= constObjetosLinha * itemInfo.posLinha; i--)
        {
            //Debug.Log("Testando Objeto: " + i);
            if (itemReferenciaBusca.GetComponent<ItemDisplay>().idObjeto == itensInstanciadosEmCena[i].GetComponent<ItemDisplay>().idObjeto)
            {
                //Debug.Log("Objetos identicos");
                listaObjetosIdenticosHorizontal.Add(itensInstanciadosEmCena[i]);
            }
            else
            {
                i = (constObjetosLinha * itemInfo.posLinha) - 1;
            }
        }
        //Debug.Log("Procurando depois do item de referencia Horizontal");
        //Debug.Log("Teste rapido Maximo:" + constObjetosLinha * (itemInfo.posLinha+1));
        for (int i = posObjetoReferencia + 1; i < constObjetosLinha * (itemInfo.posLinha + 1); i++)
        {
            //Debug.Log("Testando Objeto: " + i);
            if (itemReferenciaBusca.GetComponent<ItemDisplay>().idObjeto == itensInstanciadosEmCena[i].GetComponent<ItemDisplay>().idObjeto)
            {
                listaObjetosIdenticosHorizontal.Add(itensInstanciadosEmCena[i]);
            }
            else
            {
                i = constObjetosLinha * (itemInfo.posLinha + 1);
            }
        }
        //Debug.Log("Resultado de objetos iguais do item referencia: "+ itemReferenciaBusca.name);
        for (int i = 0; i < listaObjetosIdenticosHorizontal.Count; i++)
        {
            //Debug.Log("Objeto identico: " + listaObjetosIdenticosHorizontal[i].name);
        }
        return listaObjetosIdenticosHorizontal;
    }

    List<GameObject> procuraVertical(GameObject itemReferenciaBusca)
    {
        ItemDisplay itemInfo = itemReferenciaBusca.GetComponent<ItemDisplay>();
        List<GameObject> listaObjetosIdenticosVertical = new List<GameObject>();
        listaObjetosIdenticosVertical.Add(itemReferenciaBusca);
        int posObjetoReferencia = itemInfo.posColuna + constObjetosLinha * itemInfo.posLinha;

        //Debug.Log("Posicao Objeto: " + posObjetoReferencia);
        //Debug.Log("Procurando antes do item de referencia Horizontal");
        int valorMin = itemInfo.posColuna;
        //Debug.Log("Teste rapido Minimo:" + valorMin);
        for (int i = posObjetoReferencia - constObjetosLinha; i >= valorMin; i -= constObjetosLinha)
        {
            //Debug.Log("Testando Objeto: " + i);
            if (itemReferenciaBusca.GetComponent<ItemDisplay>().idObjeto == itensInstanciadosEmCena[i].GetComponent<ItemDisplay>().idObjeto)
            {
                //Debug.Log("Objetos identicos");
                listaObjetosIdenticosVertical.Add(itensInstanciadosEmCena[i]);
            }
            else
            {
                i = valorMin - 1;
            }
        }
        //Debug.Log("Procurando depois do item de referencia Horizontal");
        int valorMaxProcura = constQuantObjetosCena - constObjetosLinha + itemInfo.posColuna;
        //Debug.Log("Teste rapido Maximo:" + valorMaxProcura);
        for (int i = posObjetoReferencia + constObjetosLinha; i <= valorMaxProcura; i += constObjetosLinha)
        {
            //Debug.Log("Testando Objeto: " + i);
            if (itemReferenciaBusca.GetComponent<ItemDisplay>().idObjeto == itensInstanciadosEmCena[i].GetComponent<ItemDisplay>().idObjeto)
            {
                listaObjetosIdenticosVertical.Add(itensInstanciadosEmCena[i]);
            }
            else
            {
                i = valorMaxProcura + 1;
            }
        }
        //Debug.Log("Resultado de objetos iguais do item referencia: " + itemReferenciaBusca.name);
        for (int i = 0; i < listaObjetosIdenticosVertical.Count; i++)
        {
            //Debug.Log("Objeto identico: " + listaObjetosIdenticosVertical[i].name);
        }
        return listaObjetosIdenticosVertical;

    }
  
    MatchInfo coletarInformacaoMatch(GameObject item)
    {
        //Debug.Log("Entrei aqui por: " + item.name);
        //Debug.Log("------------------------------------------------------------------------------------");
        MatchInfo match = new MatchInfo();
        match.matchItens = null;
        List<GameObject> hMatch = procuraHorizontal(item);
        List<GameObject> vMatch = procuraVertical(item);
        if (hMatch.Count >= 3 && hMatch.Count > vMatch.Count)
        {
            // Match horizontal
            match.startXMatch = GetMenorX(hMatch);
            match.endingXMatch = GetMaiorX(hMatch);
            match.startYMatch = match.endingYMatch = hMatch[0].GetComponent<ItemDisplay>().posLinha;
            match.matchItens = hMatch;
            //Debug.Log("Maximo e Minimo horizontal: " + GetMaiorX(hMatch) + " , " + GetMenorX(hMatch) + "Posicao em Linha: " + match.startYMatch);
        }
        else
        {
            if (vMatch.Count >= 3)
            {
                match.startYMatch = GetMenorY(hMatch);
                match.endingYMatch = GetMaiorY(hMatch);
                match.startXMatch = match.endingXMatch = hMatch[0].GetComponent<ItemDisplay>().posColuna;
                match.matchItens = vMatch;
                //Debug.Log("Maximo e Minimo vertical: " + GetMaiorY(vMatch) + " , " + GetMenorY(vMatch) + "Posicao em Coluna: " + match.startXMatch);
            }
        }

        return match;
    }

    public IEnumerator Match(GameObject itemA, GameObject itemB)
    {
        
        MatchInfo matchA = coletarInformacaoMatch(itemA);
        if (matchA.getValidMatch())
        {
            Debug.Log("Deu match A");
            yield return StartCoroutine(DestruindoElementos(matchA.matchItens));
            yield return StartCoroutine(AtualizandoGrid(matchA.matchItens));


        }
        MatchInfo matchB = coletarInformacaoMatch(itemB);
        if (matchB.getValidMatch())
        {
            Debug.Log("Deu match B");
            yield return StartCoroutine(DestruindoElementos(matchB.matchItens));
            yield return StartCoroutine(AtualizandoGrid(matchB.matchItens));

        }

        Debug.Log("Grid atualizado com sucesso");
        for (int i = 0; i < itensInstanciadosEmCena.Length; i++)
        {
            MatchInfo matchGridAtualizado = coletarInformacaoMatch(itensInstanciadosEmCena[i]);

            if (matchGridAtualizado.getValidMatch())
            {
                yield return StartCoroutine(DestruindoElementos(matchGridAtualizado.matchItens));
                yield return StartCoroutine(AtualizandoGrid(matchGridAtualizado.matchItens));
                i = 0;
            }
        }
        atualizandoGrid = false;
        yield break;
    }

    IEnumerator DestruindoElementos(List<GameObject> itens)
    {
        audioGameControll.Play();
        foreach (GameObject item in itens)
        {
            ItemDisplay itemDisplay = item.GetComponent<ItemDisplay>();
            yield return StartCoroutine(ItemDisplay.AnimMatch(item.transform, Vector3.zero, 0.5f));

        }
    }
    IEnumerator AtualizandoGrid(List<GameObject> itens)
    {
        bool atualizeiGrid = false;
        if (GetMaiorX(itens) == GetMenorX(itens))
        {
            //Atualizar grid com o match vertical
            //Debug.Log("Atualizar grid com o match vertical");
            int valorLinha = GetMaiorY(itens);
            int valorLinhaMenor = GetMenorY(itens);
            for (int i = valorLinha + 1; i < constQuantObjetosCena / constObjetosLinha; i++)
            {

                int posicaoObjeto = i * constObjetosLinha + GetMenorX(itens);
                GameObject objetoMudar = itensInstanciadosEmCena[posicaoObjeto];
                int novaLinha = valorLinhaMenor;
                int novaColuna = objetoMudar.GetComponent<ItemDisplay>().posColuna;
                int valorPosNome = valorLinhaMenor * constObjetosLinha + objetoMudar.GetComponent<ItemDisplay>().posColuna;
                string novoNome = "ObjetoCenaPos" + valorPosNome;
                GameObject novoPai = gridsInstanciadosEmCena[novaLinha*(constQuantObjetosCena/constObjetosLinha)+novaColuna];
                alterarPosicaoObjeto(objetoMudar, novaLinha, novaColuna, novoNome, novoPai.transform);
                valorLinhaMenor++;
                yield return new WaitForSeconds(0.2f);
            }
            int valorLinhaSpawn = (constQuantObjetosCena / constObjetosLinha) - itens.Count;

            foreach (GameObject item in itens)
            {
                int posicao = (valorLinhaSpawn) * constObjetosLinha + item.GetComponent<ItemDisplay>().posColuna;
                itensInstanciadosEmCena[posicao] = null;
                InstanciarBala(gridsInstanciadosEmCena[posicao],posicao, valorLinhaSpawn, item.GetComponent<ItemDisplay>().posColuna);
                Destroy(item);
                valorLinhaSpawn++;
                yield return new WaitForSeconds(0.2f);
            }
            atualizeiGrid = true;
        }
        else
        {
            //Atualizar o grid com o match horizontal
            //Debug.Log("Atualizar grid com o match horizontal");
            int valorLinha = GetMenorY(itens);
            for (int i = valorLinha + 1; i < constQuantObjetosCena / constObjetosLinha; i++)
            {
                foreach (GameObject item in itens)
                {
                    int posicaoObjeto = i * constObjetosLinha + item.GetComponent<ItemDisplay>().posColuna;
                    GameObject objetoMudar = itensInstanciadosEmCena[i * constObjetosLinha + item.GetComponent<ItemDisplay>().posColuna];
                    int novaLinha = objetoMudar.GetComponent<ItemDisplay>().posLinha - 1;
                    int novaColuna = objetoMudar.GetComponent<ItemDisplay>().posColuna;
                    int valorPosNome = (i - 1) * constObjetosLinha + item.GetComponent<ItemDisplay>().posColuna;
                    string novoNome = "ObjetoCenaPos" + valorPosNome;
                    GameObject novoPai = gridsInstanciadosEmCena[novaLinha * (constQuantObjetosCena / constObjetosLinha) + novaColuna];
                    alterarPosicaoObjeto(objetoMudar, novaLinha, novaColuna, novoNome,novoPai.transform);

                }
                yield return new WaitForSeconds(0.2f);
            }
            foreach (GameObject item in itens)
            {
                int posicao = ((constQuantObjetosCena / constObjetosLinha) - 1) * constObjetosLinha + item.GetComponent<ItemDisplay>().posColuna;
                itensInstanciadosEmCena[posicao] = null;
                InstanciarBala(gridsInstanciadosEmCena[posicao],posicao, (constQuantObjetosCena / constObjetosLinha) - 1, item.GetComponent<ItemDisplay>().posColuna);
                Destroy(item);
                yield return new WaitForSeconds(0.2f);

            }
            atualizeiGrid = true;
        }


        yield return atualizeiGrid;
    }
    int GetMenorX(List<GameObject> matchItens)
    {
        //Debug.Log("Teste Menor X");
        float[] indices = new float[matchItens.Count];
        GameObject obj = matchItens[0];
        for (int i = 0; i < matchItens.Count; i++)
        {
            ItemDisplay itemInfo = matchItens[i].GetComponent<ItemDisplay>();
            //Debug.Log("Posicao Coluna: " + itemInfo.posColuna);
            indices[i] = itemInfo.posColuna;
        }
        return (int)Mathf.Min(indices);
    }
    int GetMaiorX(List<GameObject> matchItens)
    {
        //Debug.Log("Teste Maior X");
        float[] indices = new float[matchItens.Count];
        GameObject obj = matchItens[0];
        for (int i = 0; i < matchItens.Count; i++)
        {
            ItemDisplay itemInfo = matchItens[i].GetComponent<ItemDisplay>();
            //Debug.Log("Posicao Coluna: " + itemInfo.posColuna);
            indices[i] = itemInfo.posColuna;
        }
        return (int)Mathf.Max(indices);
    }

    int GetMenorY(List<GameObject> matchItens)
    {
        //Debug.Log("Teste Menor Y");
        float[] indices = new float[matchItens.Count];
        GameObject obj = matchItens[0];
        for (int i = 0; i < matchItens.Count; i++)
        {
            ItemDisplay itemInfo = matchItens[i].GetComponent<ItemDisplay>();
            //Debug.Log("Posicao Linha: " + itemInfo.posLinha);
            indices[i] = itemInfo.posLinha;
        }
        return (int)Mathf.Min(indices);
    }
    int GetMaiorY(List<GameObject> matchItens)
    {
        //Debug.Log("Teste Maior Y");
        float[] indices = new float[matchItens.Count];
        GameObject obj = matchItens[0];
        for (int i = 0; i < matchItens.Count; i++)
        {
            ItemDisplay itemInfo = matchItens[i].GetComponent<ItemDisplay>();
            //Debug.Log("Posicao Linha: " + itemInfo.posLinha);
            indices[i] = itemInfo.posLinha;
        }
        return (int)Mathf.Max(indices);
    }
    #endregion

    #region Logica de Movimentação do Grid
    
    public void realizarTrocaObjetos(GameObject objetoPrimario, GameObject objetoSecundario)
    {
        Vector3 objetoPrimarioNovaPos = objetoSecundario.transform.position;
        string objetoPrimarioNovoNome = objetoSecundario.name;
        int novoPosLinhaObjetoPrimario = objetoSecundario.GetComponent<ItemDisplay>().posLinha;
        int novoPosColunaObjetoPrimario = objetoSecundario.GetComponent<ItemDisplay>().posColuna;
        Transform novoPaiPrimario = objetoSecundario.GetComponent<ItemDisplay>().pai.transform;
        alterarPosicaoObjeto(objetoSecundario, objetoPrimario.GetComponent<ItemDisplay>().posLinha, objetoPrimario.GetComponent<ItemDisplay>().posColuna, objetoPrimario.name, objetoPrimario.GetComponent<ItemDisplay>().pai.transform);
        alterarPosicaoObjeto(objetoPrimario, novoPosLinhaObjetoPrimario, novoPosColunaObjetoPrimario, objetoPrimarioNovoNome, novoPaiPrimario);


    }

    void alterarPosicaoObjeto(GameObject objeto, int novaPosicaoLinha, int novaPosicaoColuna, string novoNome,Transform novoPai)
    {
        string objetoNovoNome = novoNome;
        int novoPosLinhaObjeto = novaPosicaoLinha;
        int novoPosColunaObjeto = novaPosicaoColuna;
        ConfigurarPosicaoBala(novoPai.gameObject, objeto);
        objeto.name = objetoNovoNome;
        objeto.GetComponent<ItemDisplay>().alterarValorLinhaColunaObjeto(novaPosicaoLinha, novaPosicaoColuna);
        int posicaoObjeto = objeto.GetComponent<ItemDisplay>().posColuna + constObjetosLinha * objeto.GetComponent<ItemDisplay>().posLinha;
        objeto.transform.SetParent(novoPai);
        objeto.GetComponent<ItemDisplay>().pai = novoPai.gameObject;
        itensInstanciadosEmCena[posicaoObjeto] = objeto;
    }
    #endregion


}
