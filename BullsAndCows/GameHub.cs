using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace BullsAndCows
{
    public class GameHub : Hub
    {
        public async Task GetGuess(GameObject obj)
        {
            if (obj == null || obj.Possibilities == null) return;

            obj.Count++;
            if (obj.Possibilities.Count == 0)
            {
                obj = GameHelper.GetNewGameObject();
                await Clients.Caller.SendAsync("ReceiveMessage", obj, "Unable to guess your number!");
                await Clients.Caller.SendAsync("ReceiveMessage", obj, "Lets play again!");
            }

            obj.Guess = obj.Possibilities[0];
            await Clients.Caller.SendAsync("ReceiveMessage", obj, $"Attempt #{obj.Count}. My guess: {obj.Guess}");
            await Clients.Caller.SendAsync("ReceiveMessage", obj, "Enter the number of bulls and cows as a two-digit number: ");
        }

        public async Task SendAnswer(GameObject obj, string answer)
        {
            if (obj == null || obj.Possibilities == null) return;

            if (string.IsNullOrEmpty(answer) || answer.Length < 2)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", obj, $"Invalid response!");
                await Clients.Caller.SendAsync("ReceiveMessage", obj, "Enter the number of bulls and cows as a two-digit number: ");
                return;
            }

            var bulls = Convert.ToInt32($"{answer[0]}");
            var cows = Convert.ToInt32($"{answer[1]}");

            if (bulls == 4)
            {
                obj = GameHelper.GetNewGameObject();
                await Clients.Caller.SendAsync("ReceiveMessage", obj, $"Your number is: {obj.Guess}");
                await Clients.Caller.SendAsync("ReceiveMessage", obj, "Lets play again!");
                return;
            }

            for (var i = obj.Possibilities.Count - 1; i >= 0; i--)
            {
                var b = 0;
                var c = 0;
                for (var j = 0; j < 4; j++)
                {
                    if (obj.Possibilities[i][j] == obj.Guess[j])
                    {
                        b++;
                    }
                    else if (obj.Possibilities[i].Contains(obj.Guess[j]))
                    {
                        c++;
                    }
                }
                if (b != bulls || c != cows)
                {
                    obj.Possibilities.RemoveAt(i);
                }
            }

            await Clients.Caller.SendAsync("ReceiveMessage", obj);
        }

        (int, int) CalcBullsAndCows(string guess, string possibility)
        {
            var bulls = 0;
            var cows = 0;
            for (var i = 0; i < 4; i++)
            {
                if (guess[i] == possibility[i])
                {
                    bulls++;
                }
                else if (possibility.IndexOf(guess[i]) >= 0)
                {
                    cows++;
                }
            }
            return (bulls, cows);
        }
    }
}
