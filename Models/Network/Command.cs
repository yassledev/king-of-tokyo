using System;
using System.Collections.Generic;
using System.Text;

namespace KingOfTokyo.Models.Network
{
    public enum Command
    {
        ServeurMessage, 
        Message, 
        CreateGame,
        JoinGame,
        UpdateMe,
        UpdatePlayer,
        StartGame,
        QuitGame,
        StartTurn,
        Roll,
        CardTurn,
        EndCard,
        EndRoll,
        SaveDice,
        UnsaveDice,
        EndTurn,
        StayNamek,
        ExitNamek,
        BuyCard
    }
}
