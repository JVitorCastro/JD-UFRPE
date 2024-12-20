using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    [SerializeField] private GameObject painelInicial;
    [SerializeField] private GameObject painelOpcoes;

    public void Jogar()
    {
        SceneManager.LoadScene(1);
    }

    public void AbrirOpcoes()
    {
        painelInicial.SetActive(false);
        painelOpcoes.SetActive(true);
    }
    
    public void FecharOpcoes()
    {
        painelInicial.SetActive(true);
        painelOpcoes.SetActive(false);
    }

    public void SairJogo()
    {
        Debug.Log("Sair do Jogo");
        Application.Quit();
    }
}
