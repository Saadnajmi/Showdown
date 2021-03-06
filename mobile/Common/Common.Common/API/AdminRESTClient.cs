﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Admin.Common.API.Entities;
using Common.Common;
using ClientModel = Common.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ClientModels = Common.Common.Models;

namespace Admin.Common.API
{
	public class AdminRESTClient : IAnnoucementInteractor
    {
        HttpClient client;

        public String Token { get; set; }

		public AdminRESTClient()
		{
			client = new HttpClient();
		}

        public async Task<string> GetToken(string facebookAccessToken)
        {
			try
			{
				string jsonString = JsonConvert.SerializeObject(new { facebookAccessToken = facebookAccessToken }, Formatting.None);
				string response = await PostAsync("/accounts/login", jsonString, authenticated: false);

				var data = JObject.Parse(response);
				return ((string)data["token"]);
			}
			catch (Exception e)
			{
                Console.WriteLine(e.StackTrace);
				return "";
			}
        }

		public async Task<List<ClientModels.School>> GetSchools()
		{
			try
			{
				var jsonString = await RequestAsync("/core/schools");
				return ClientModels.School.FromJSONArray(jsonString);
			}
			catch (Exception e)
			{
				return new List<ClientModel.School>();
			}
		}

		public async Task<Event> GetEvent(int id)
		{
			try
			{
				var path = $"/admin/events/{id}";
				var jsonString = await RequestAsync(path);
				return Event.FromJSON(jsonString);
			}
			catch (Exception e)
			{
				return default(Event);
			}
		}

		public async Task<List<Event>> GetEvents()
		{
			try
			{
				var jsonString = await RequestAsync("/admin/events");
				return Event.FromJSONArray(jsonString);
			}
			catch (Exception e)
			{
				return new List<Event>();
			}
		}

		public async Task<Event> SaveEvent(Event e)
		{
			try
			{
				var jsonString = "";
				if (e.Id == null)
				{
					var path = $"/admin/events";
					jsonString = await PostAsync(path, JsonConvert.SerializeObject(e));
				}
				else
				{
					var path = $"/admin/events/{e.Id}";
					jsonString = await PutAsync(path, JsonConvert.SerializeObject(e));
				}

				return Event.FromJSON(jsonString);
			}
			catch (Exception)
			{
				return default(Event);
			}
		}

		public async Task DeleteEvent(Event e)
		{
			try
			{
				var path = $"/admin/events/{e.Id}";
				await DeleteAsync(path);
			}
			catch (Exception)
			{
				return;
			}
		}

		public async Task<Location> GetLocation(int id)
		{
			try
			{
				var path = $"/admin/locations/{id}";
				var jsonString = await RequestAsync(path);
				return Location.FromJSON(jsonString);
			}
			catch (Exception)
			{
				return default(Location);
			}
		}

		public async Task<Location> SaveLocation(Location locationToSave)
		{
			try
			{
				var jsonString = "";
				if (locationToSave.Id == null)
				{
					var path = $"/admin/locations";
					jsonString = await PostAsync(path, JsonConvert.SerializeObject(locationToSave));
				}
				else
				{
					var path = $"/admin/locations/{locationToSave.Id}";
					jsonString = await PutAsync(path, JsonConvert.SerializeObject(locationToSave));
				}

				return Location.FromJSON(jsonString);
			}
			catch (Exception)
			{
				return default(Location);
			}
		}

		public async Task DeleteLocation(Location l)
		{
			try
			{
				var path = $"/admin/locations/{l.Id}";
				await DeleteAsync(path);
			}
			catch (Exception)
			{
				return;
			}
		}

		public async Task<List<Location>> GetLocations()
		{
			try
			{
				var path = $"/admin/locations";
				var jsonString = await RequestAsync(path);
				return Location.FromJSONArray(jsonString);
			}
			catch (Exception)
			{
				return new List<Location>();
			}
		}

		public async Task<List<Game>> GetGames()
		{
			try
			{
				var jsonString = await RequestAsync("/admin/games");
				return Game.FromJSONArray(jsonString);
			}
			catch (Exception)
			{
				return new List<Game>();
			}
		}

        public async Task<List<ClientModel.Game>> GetScoreKeeperGames()
        {
			try
			{
				var jsonString = await RequestAsync("/admin/scorekeeper/games");
				return ClientModel.Game.FromJSONArray(jsonString);
			}
			catch (Exception)
			{
				return new List<ClientModel.Game>();
			}
        }

        public async Task<Game> SaveGame(Game g)
		{
			try
			{
				var jsonString = "";
				if (g.Id == null)
				{
					var path = $"/admin/games";
					jsonString = await PostAsync(path, JsonConvert.SerializeObject(g));
				}
				else
				{
					var path = $"/admin/games/{g.Id}";
					jsonString = await PutAsync(path, JsonConvert.SerializeObject(g));
				}

				return Game.FromJSON(jsonString);
			}
			catch (Exception)
			{
				return default(Game);
			}
		}

        public async Task EndGame(ClientModel.Game g)
        {
            var path = $"/admin/scorekeeper/games/{g.ID}/in-progress";
            await PutAsync(path, "{ \"in_progress\": false}");
        }

        public async Task DeleteGame(Game g)
		{
			try
			{
				var path = $"/admin/games/{g.Id}";
				await DeleteAsync(path);
			}
			catch (Exception)
			{
			}
		}

		public async Task<List<User>> GetUsers()
		{
			try
			{
				var jsonString = await RequestAsync("/admin/users");
				return User.FromJSONArray(jsonString);
			}
			catch (Exception)
			{
				return new List<User>();
			}
		}

		public async Task<List<ClientModel.Announcement>> GetAnnouncements()
		{
			try
			{
				var jsonString = await RequestAsync("/admin/announcements");
				return ClientModel.Announcement.FromJSONArray(jsonString);
			}
			catch (Exception)
			{
				return new List<ClientModel.Announcement>();
			}
		}

		public async Task<ClientModel.Announcement> CreateAnnouncement(ClientModel.Announcement announcement)
		{
			try
			{
				var jsonString = "";
				if (announcement.Id == null)
				{
					var path = $"/admin/announcements";
					jsonString = await PostAsync(path, JsonConvert.SerializeObject(announcement));
				}
				else
				{
					throw new Exception("Can't update announcements!");
				}

				return ClientModel.Announcement.FromJSON(jsonString);
			}
			catch (Exception)
			{
				return default(ClientModel.Announcement);
			}
		}

        public async Task<ClientModel.Score> GetScore(Game g)
        {
            var jsonString = await RequestAsync($"/admin/games/{g.Id}/scores");
            return ClientModel.Score.FromJSON(jsonString);
        }


        public async Task<ClientModel.Score> SaveScore(ClientModel.Game g, ClientModel.Score score)
        {
            var path = $"/admin/scorekeeper/games/{g.ID}";
            var jsonString = await PostAsync(path, JsonConvert.SerializeObject(score));

            return ClientModel.Score.FromJSON(jsonString);
        }

        private HttpRequestMessage BuildRequest(string path, string jsonBody, bool authenticated)
        {
            var url = $"{Secrets.BACKEND_URL}{path}.json";
            var request = new HttpRequestMessage() {RequestUri = new Uri(url)};
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (jsonBody != null) request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            if (authenticated) request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            return request;
        }

		private async Task<string> RequestAsync(string path, bool authenticated = true)
		{
		    var request = BuildRequest(path, null, authenticated);
		    request.Method = HttpMethod.Get;
		    var response = await client.SendAsync(request);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadAsStringAsync();
		}

        private async Task<string> PostAsync(string path, string jsonString, bool authenticated = true)
        {
		    var request = BuildRequest(path, jsonString, authenticated);
		    request.Method = HttpMethod.Post;
		    var response = await client.SendAsync(request);
			return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> PutAsync(string path, string jsonString, bool authenticated = true)
        {
		    var request = BuildRequest(path, jsonString, authenticated);
		    request.Method = HttpMethod.Put;
		    var response = await client.SendAsync(request);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadAsStringAsync();
        }

		private async Task DeleteAsync(string path, bool authenticated = true)
		{
			var request = BuildRequest(path, null, authenticated);
			request.Method = HttpMethod.Delete;
			var response = await client.SendAsync(request);
			response.EnsureSuccessStatusCode();
			return;
		}
    }
}