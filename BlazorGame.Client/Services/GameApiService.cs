using System.Net.Http.Headers;
using System.Net.Http.Json;
using SharedModels;

namespace BlazorGame.Client
{
    public class GameApiService
    {
        private readonly HttpClient _http;

        public GameApiService(HttpClient http)
        {
            _http = http;
        }
        
        public void SetBearerToken(string token)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        
        public async Task<Dungeon?> GenerateDungeonAsync(int rooms = 4)
        {
            return await _http.GetFromJsonAsync<Dungeon>($"api/dungeons/generate?rooms={rooms}");
        }

        public async Task<GameState?> DoActionAsync(GameState state, string action)
        {
            var request = new { State = state, Action = action };
            var response = await _http.PostAsJsonAsync("api/game/action", request);
            return await response.Content.ReadFromJsonAsync<GameState>();
        }
        public async Task<GameSave?> SaveGameAsync(GameSave save)
        {
            var response = await _http.PostAsJsonAsync("api/saves", save);
            return await response.Content.ReadFromJsonAsync<GameSave>();
        }

        public async Task<List<GameSave>?> GetAllSavesAsync()
        {
            return await _http.GetFromJsonAsync<List<GameSave>>("api/saves");
        }
        
        public async Task<List<Player>?> GetAllPlayersAsync()
        {
            return await _http.GetFromJsonAsync<List<Player>>("api/players");
        }

    }
}
