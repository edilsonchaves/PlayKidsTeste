using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonScript : MonoBehaviour
{
    public void ComecarJogo()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void RetornarMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void ReiniciarJogo()
    {
        SceneManager.LoadScene("GameScene");
;    }

    public void CreditosMenu()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void EmbaralharPecas()
    {
        GridGameControll grid = GameObject.Find("GridGameControll").GetComponent<GridGameControll>();
        grid.embararlharPecas();
    }
}
