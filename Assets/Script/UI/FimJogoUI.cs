using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FimJogoUI : MonoBehaviour
{
    public Text pontuacao;
    // Start is called before the first frame update
    public void ConstruirResultado(int pontuacaoFinal,int fase)
    {
        pontuacao.text = "RESULTADO FINAL\n Pontuacao: "+pontuacaoFinal + " na Fase: " + fase;
    }
}
