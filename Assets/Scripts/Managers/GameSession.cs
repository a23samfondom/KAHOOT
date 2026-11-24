// Conserva los datos necesarios durante la partida.
public static class GameSession
{
    // Cuestionario seleccionado por el jugador.
    public static QuizData SelectedQuiz { get; set; }

    // Nombre del jugador.
    public static string PlayerName { get; set; }
}