using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obtener una referencia al script del jugador
            Movimiento playerMovement = collision.gameObject.GetComponent<Movimiento>();
            if (playerMovement != null)
            {
                // Llamar al método para disminuir las vidas del jugador
                playerMovement.DecreaseLives(1); // o la cantidad de vidas que desees
            }

            // Destruir el enemigo
            Destroy(gameObject);
        }
    }
}
