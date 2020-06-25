using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GridDisplay : MonoBehaviour
{
    int constQuantidadeObjetoCena;
    int constObjetosLinha;
    int coluna;
    int linha;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        configurarGridUI();
    }
    public void configurarGrid(int posColuna,int posLinha, int quantObjetoJogo, int quantObjetosLinha)
    {
        coluna=posColuna;
        linha = posLinha;
        constQuantidadeObjetoCena = quantObjetoJogo;
        constObjetosLinha = quantObjetosLinha;
    }
    public void configurarGridUI()
    {
        float anchorValorMinX = 0 + coluna * (1f / constObjetosLinha);
        float anchorValorMaxX = 0 + (coluna + 1) * (1f / constObjetosLinha);
        //Debug.Log("Anchor X: " + anchorValorMinX + " , " + anchorValorMaxX);
        float anchorValorMinY = 0 + linha * (0.8f / (constQuantidadeObjetoCena / constObjetosLinha));
        float anchorValorMaxY = 0 + (linha + 1) * (0.8f / (constQuantidadeObjetoCena / constObjetosLinha));
        //Debug.Log("Anchor Y: " + anchorValorMinY + " , " + anchorValorMaxY);
        GetComponent<RectTransform>().anchorMin = new Vector2(anchorValorMinX, anchorValorMinY);
        GetComponent<RectTransform>().anchorMax = new Vector2(anchorValorMaxX, anchorValorMaxY);
        GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        GetComponent<RectTransform>().transform.position = new Vector3(0, 0, 0);
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0.5f, 0.5f);
    }
}
