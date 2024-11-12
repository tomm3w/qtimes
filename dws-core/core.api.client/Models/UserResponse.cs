using Newtonsoft.Json;

namespace core.api.client.Models
{
	public class UserResponse
	{
		[JsonProperty("status")]
		public ApiResposeStatus Status { get; set; }

		[JsonProperty("type")]
		public ApiResponseType Type { get; set; }

		[JsonProperty("data")]
		public CoreUserData Data { get; set; }
	}

	public class CoreUserData
	{
		[JsonProperty("User")]
		public User User { get; set; }
	}
}
