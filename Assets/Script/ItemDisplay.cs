using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ItemDisplay : MonoBehaviour,IPointerDownHandler
{
    Image _rendererObjeto;
    public GameObject pai;
    public int idObjeto;
    public int posLinha, posColuna;
    public bool selecionado;
    public AudioSource audioObjeto;
    private void Awake()
    {
        _rendererObjeto = GetComponent<Image>();

    }
    void Update()
    {
        if (selecionado)
        {
            this.gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        }
        else
        {
            this.gameObject.GetComponent<Image>().color = new Color(1f,1f,1f);
        }
    }
    public void tocarSom()
    {
        audioObjeto.Play();
    }
    public void ConfigurandoItem(GameObject GridPai,ItemScript itemInfo, int linha, int coluna)
    {
        pai = GridPai;
        idObjeto = itemInfo.ID;
        _rendererObjeto.sprite = itemInfo.spriteObjeto;
        posLinha = linha;
        posColuna = coluna;
    }

    public void alterarValorLinhaColunaObjeto(int novoValorLinha, int novoValorColuna)
    {
        posLinha = novoValorLinha;
        posColuna = novoValorColuna;
    }

    public static IEnumerator AnimMatch(Transform objetoTam, Vector3 scaleFinal, float duracao)
    {
        Vector3 diferencaScale = scaleFinal - objetoTam.localScale;
        float alteracaoTamanho = diferencaScale.magnitude;
        diferencaScale.Normalize();
        float counter = 0;
        while (counter < duracao)
        {
            float novoTam = (Time.deltaTime * alteracaoTamanho) / duracao;
            objetoTam.localScale += diferencaScale * novoTam;
            counter += Time.deltaTime;
            yield return null;
        }
        objetoTam.localScale = scaleFinal;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        int posicaoObjeto = posColuna + posLinha * 6;
        Debug.Log("Cliquei no item: " + posicaoObjeto);
        tocarSom();
        GameControllScript gc = GameObject.Find("Main Camera").GetComponent<GameControllScript>();
        gc.selecionandoObjeto(this.gameObject);
    }
}
